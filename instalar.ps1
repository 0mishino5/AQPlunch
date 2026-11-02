param(
    [string]$PasswordPostgres,
    [string]$ClaveJwt,
    [string]$PrefijoBase = "aqplunch"
)

$ErrorActionPreference = "Continue"
$interactivo = [string]::IsNullOrEmpty($PasswordPostgres) -or [string]::IsNullOrEmpty($ClaveJwt)

function Salir([int]$codigo) {
    if ($script:interactivo) {
        Read-Host "Presiona Enter para cerrar" | Out-Null
    }
    exit $codigo
}

function Leer-Secreto([string]$mensaje) {
    $seguro = Read-Host -Prompt $mensaje -AsSecureString
    $puntero = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($seguro)
    try {
        return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($puntero)
    }
    finally {
        [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($puntero)
    }
}

function Ejecutar-Psql([string]$base, [string[]]$argumentos) {
    $salida = & $script:rutaPsql -h localhost -p 5432 -U postgres -w -d $base @argumentos 2>&1
    $texto = ($salida | ForEach-Object { "$_" }) -join "`n"
    return @{ Codigo = $LASTEXITCODE; Salida = $texto.Trim() }
}

Write-Host ""
Write-Host "=== Instalacion local de AQPlunch ===" -ForegroundColor Cyan
Write-Host "Crea las bases de datos, carga los datos de prueba y genera el appsettings.Development.json de cada servicio."
Write-Host ""

$psqlInstalado = Get-ChildItem "C:\Program Files\PostgreSQL\*\bin\psql.exe" -ErrorAction SilentlyContinue |
    Sort-Object { $_.Directory.Parent.Name -as [int] } -Descending |
    Select-Object -First 1

if ($psqlInstalado) {
    $rutaPsql = $psqlInstalado.FullName
}
else {
    $comando = Get-Command psql.exe -ErrorAction SilentlyContinue

    if (-not $comando) {
        Write-Host "No se encontro PostgreSQL (psql.exe). Instala PostgreSQL y vuelve a ejecutar este script." -ForegroundColor Red
        Salir 1
    }

    $rutaPsql = $comando.Source
}

Write-Host "PostgreSQL encontrado: $rutaPsql"
Write-Host ""

if ([string]::IsNullOrEmpty($PasswordPostgres)) {
    $PasswordPostgres = Leer-Secreto "Contrasena de tu PostgreSQL (usuario postgres)"
}

if ([string]::IsNullOrEmpty($ClaveJwt)) {
    $ClaveJwt = Leer-Secreto "Clave del JWT del grupo"
}

if ($ClaveJwt.Length -lt 32) {
    Write-Host "La clave del JWT debe tener al menos 32 caracteres. Pidela al grupo y vuelve a ejecutar el script." -ForegroundColor Red
    Salir 1
}

$env:PGPASSWORD = $PasswordPostgres
$env:PGCLIENTENCODING = "UTF8"

$prueba = Ejecutar-Psql "postgres" @("-tAc", "SELECT 1")

if ($prueba.Codigo -ne 0) {
    Write-Host "No se pudo entrar a PostgreSQL. Revisa que este encendido y que la contrasena sea correcta." -ForegroundColor Red
    Write-Host $prueba.Salida -ForegroundColor DarkGray
    Salir 1
}

Write-Host "Conexion a PostgreSQL correcta." -ForegroundColor Green
Write-Host ""

$servicios = @("Identity", "Restaurant", "Order", "Sales")
$hayErrores = $false

foreach ($servicio in $servicios) {
    $minuscula = $servicio.ToLower()
    $base = "$($PrefijoBase)_$minuscula"
    $carpeta = Join-Path $PSScriptRoot $servicio
    $scriptTablas = Join-Path $carpeta "aqplunch_$minuscula.sql"
    $scriptDatos = Join-Path $carpeta "insert_$minuscula.sql"
    $carpetaApi = Join-Path $carpeta "AQPlunch.$servicio.Api"

    Write-Host "--- $servicio ---" -ForegroundColor Cyan

    if (-not (Test-Path $scriptTablas)) {
        Write-Host "  Todavia no tiene su script de base de datos en esta rama. Se omite la base."
    }
    else {
        $existe = Ejecutar-Psql "postgres" @("-tAc", "SELECT 1 FROM pg_database WHERE datname = '$base'")

        if ($existe.Salida -ne "1") {
            $creada = Ejecutar-Psql "postgres" @("-qc", "CREATE DATABASE `"$base`"")

            if ($creada.Codigo -ne 0) {
                Write-Host "  No se pudo crear la base $($base): $($creada.Salida)" -ForegroundColor Red
                $hayErrores = $true
                continue
            }

            Write-Host "  Base $base creada."
        }

        $tablas = Ejecutar-Psql $base @("-tAc", "SELECT count(*) FROM information_schema.tables WHERE table_schema = 'public'")

        if ($tablas.Salida -ne "0") {
            Write-Host "  La base $base ya tenia tablas. No se toca."
        }
        else {
            $resultadoTablas = Ejecutar-Psql $base @("-v", "ON_ERROR_STOP=1", "-1", "-q", "-f", $scriptTablas)

            if ($resultadoTablas.Codigo -ne 0) {
                Write-Host "  Error al crear las tablas: $($resultadoTablas.Salida)" -ForegroundColor Red
                $hayErrores = $true
                continue
            }

            Write-Host "  Tablas creadas."

            if (Test-Path $scriptDatos) {
                $resultadoDatos = Ejecutar-Psql $base @("-v", "ON_ERROR_STOP=1", "-1", "-q", "-f", $scriptDatos)

                if ($resultadoDatos.Codigo -ne 0) {
                    Write-Host "  Error al cargar los datos de prueba: $($resultadoDatos.Salida)" -ForegroundColor Red
                    $hayErrores = $true
                    continue
                }

                Write-Host "  Datos de prueba cargados."
            }
        }
    }

    if (Test-Path $carpetaApi) {
        $passwordConexion = $PasswordPostgres

        if ($passwordConexion -match "[;']") {
            $passwordConexion = "'" + ($passwordConexion -replace "'", "''") + "'"
        }

        $configuracion = [ordered]@{
            ConnectionStrings = [ordered]@{
                DefaultConnection = "Host=localhost;Port=5432;Database=$base;Username=postgres;Password=$passwordConexion"
            }
            Jwt = [ordered]@{
                SecretKey = $ClaveJwt
            }
            Logging = [ordered]@{
                LogLevel = [ordered]@{
                    Default = "Information"
                    "Microsoft.AspNetCore" = "Warning"
                }
            }
        }

        $json = $configuracion | ConvertTo-Json -Depth 5
        $archivo = Join-Path $carpetaApi "appsettings.Development.json"
        [System.IO.File]::WriteAllText($archivo, $json, (New-Object System.Text.UTF8Encoding($false)))

        Write-Host "  appsettings.Development.json generado."
    }
}

Remove-Item Env:\PGPASSWORD -ErrorAction SilentlyContinue

Write-Host ""

if ($hayErrores) {
    Write-Host "Terminado con errores. Revisa los mensajes en rojo." -ForegroundColor Yellow
    Salir 1
}

Write-Host "Listo. Abre AQPlunch.sln en Rider, elige un servicio y dale Play." -ForegroundColor Green
Salir 0
