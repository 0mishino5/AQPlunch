-- Base de datos del SalesService de AQPlunch
-- Crear primero la base: CREATE DATABASE aqplunch_sales;

-- Tabla Metodos de pago
CREATE TABLE payment_methods (
    payment_method_id UUID PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    is_active BOOLEAN NOT NULL DEFAULT TRUE
);

-- Tabla Ventas
-- order_id y restaurant_id vienen de OrderService y RestaurantService; no llevan FK porque esas tablas viven en otras bases
-- order_id es UNIQUE para que un mismo pedido no se cobre dos veces
-- registered_by es el userId del token de quien registro la venta
CREATE TABLE sales (
    sale_id UUID PRIMARY KEY,
    order_id UUID NOT NULL UNIQUE,
    restaurant_id UUID NOT NULL,
    payment_method_id UUID NOT NULL,
    registered_by UUID NOT NULL,
    total NUMERIC(10, 2) NOT NULL CHECK (total >= 0),
    sold_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
    FOREIGN KEY (payment_method_id) REFERENCES payment_methods(payment_method_id)
);

-- Tabla Detalle de ventas
-- Es una copia del detalle del pedido al momento de la venta, para que los reportes salgan solo con datos de esta base
CREATE TABLE sale_details (
    sale_detail_id UUID PRIMARY KEY,
    sale_id UUID NOT NULL,
    product_id UUID NOT NULL,
    product_name VARCHAR(150) NOT NULL,
    unit_price NUMERIC(10, 2) NOT NULL CHECK (unit_price >= 0),
    quantity INT NOT NULL CHECK (quantity > 0),
    subtotal NUMERIC(10, 2) NOT NULL CHECK (subtotal >= 0),
    FOREIGN KEY (sale_id) REFERENCES sales(sale_id) ON DELETE CASCADE
);
