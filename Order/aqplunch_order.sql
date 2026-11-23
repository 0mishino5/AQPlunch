-- Base de datos del OrderService de AQPlunch
-- Crear primero la base: CREATE DATABASE aqplunch_order;

-- Tabla Pedidos
-- restaurant_id y table_id vienen de RestaurantService; no llevan FK porque esas tablas viven en otra base
-- table_number es una copia del numero de mesa para mostrarlo en la cocina sin consultar a RestaurantService
CREATE TABLE orders (
    order_id UUID PRIMARY KEY,
    restaurant_id UUID NOT NULL,
    table_id UUID NOT NULL,
    table_number INT NOT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'pendiente' CHECK (status IN ('pendiente', 'en_preparacion', 'listo', 'entregado', 'cancelado')),
    total NUMERIC(10, 2) NOT NULL CHECK (total >= 0),
    note TEXT,
    created_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now(),
    updated_at TIMESTAMP WITHOUT TIME ZONE DEFAULT now()
);

-- Tabla Detalle de pedidos
-- product_name y unit_price son copias del producto al momento del pedido: si despues cambia el precio, el pedido no cambia
CREATE TABLE order_details (
    order_detail_id UUID PRIMARY KEY,
    order_id UUID NOT NULL,
    product_id UUID NOT NULL,
    product_name VARCHAR(150) NOT NULL,
    unit_price NUMERIC(10, 2) NOT NULL CHECK (unit_price >= 0),
    quantity INT NOT NULL CHECK (quantity > 0),
    subtotal NUMERIC(10, 2) NOT NULL CHECK (subtotal >= 0),
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE
);
