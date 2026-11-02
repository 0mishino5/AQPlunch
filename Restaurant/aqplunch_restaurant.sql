-- Base de datos del RestaurantService de AQPlunch
-- Crear primero la base: CREATE DATABASE aqplunch_restaurant;

-- Tabla Restaurantes
CREATE TABLE restaurants (
    restaurant_id UUID PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    address VARCHAR(255),
    phone VARCHAR(20),
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now()
);

-- Tabla Mesas
-- qr_token es el valor que va en el codigo QR y en la etiqueta NFC de la mesa; es un UUID para que no se pueda adivinar
CREATE TABLE dining_tables (
    table_id UUID PRIMARY KEY,
    restaurant_id UUID NOT NULL,
    table_number INT NOT NULL,
    qr_token UUID NOT NULL UNIQUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    UNIQUE (restaurant_id, table_number),
    FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id) ON DELETE CASCADE
);

-- Tabla Categorias del menu
CREATE TABLE categories (
    category_id UUID PRIMARY KEY,
    restaurant_id UUID NOT NULL,
    name VARCHAR(100) NOT NULL,
    UNIQUE (restaurant_id, name),
    FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id) ON DELETE CASCADE
);

-- Tabla Productos
-- El restaurante de un producto se obtiene por su categoria
CREATE TABLE products (
    product_id UUID PRIMARY KEY,
    category_id UUID NOT NULL,
    name VARCHAR(150) NOT NULL,
    description TEXT,
    price NUMERIC(10, 2) NOT NULL CHECK (price >= 0),
    is_available BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
    FOREIGN KEY (category_id) REFERENCES categories(category_id) ON DELETE CASCADE
);
