-- Base de datos del IdentityService de AQPlunch
-- Crear primero la base: CREATE DATABASE aqplunch_identity;

-- Tabla Roles
CREATE TABLE roles (
    role_id UUID PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL UNIQUE
);

-- Tabla Usuarios
-- restaurant_id es el id del restaurante en RestaurantService; no lleva FK porque esa tabla vive en otra base
CREATE TABLE users (
    user_id UUID PRIMARY KEY,
    username VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    email VARCHAR(150) UNIQUE,
    full_name VARCHAR(150) NOT NULL,
    restaurant_id UUID NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now()
);

-- Tabla pivote para asignar roles a usuarios (muchos a muchos)
CREATE TABLE user_roles (
    user_id UUID NOT NULL,
    role_id UUID NOT NULL,
    assigned_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE
);
