-- Datos de prueba del OrderService de AQPlunch
-- Usan los mismos restaurantes, mesas y productos del script de datos de RestaurantService
-- El nombre y el precio de cada producto estan copiados en el detalle, como se guardan al hacer un pedido

INSERT INTO orders (order_id, restaurant_id, table_id, table_number, status, total, note) VALUES
('a0a0a0a0-0000-0000-0000-000000000001', 'f1f1f1f1-0000-0000-0000-000000000001', 'd1d1d1d1-0000-0000-0000-000000000002', 2, 'entregado', 35.00, NULL),
('a0a0a0a0-0000-0000-0000-000000000002', 'f1f1f1f1-0000-0000-0000-000000000001', 'd1d1d1d1-0000-0000-0000-000000000001', 1, 'en_preparacion', 60.00, 'Sin aji, por favor'),
('a0a0a0a0-0000-0000-0000-000000000003', 'f1f1f1f1-0000-0000-0000-000000000002', 'd1d1d1d1-0000-0000-0000-000000000004', 1, 'pendiente', 44.00, NULL);

INSERT INTO order_details (order_detail_id, order_id, product_id, product_name, unit_price, quantity, subtotal) VALUES
('b0b0b0b0-0000-0000-0000-000000000001', 'a0a0a0a0-0000-0000-0000-000000000001', 'e1e1e1e1-0000-0000-0000-000000000001', 'Rocoto relleno', 25.00, 1, 25.00),
('b0b0b0b0-0000-0000-0000-000000000002', 'a0a0a0a0-0000-0000-0000-000000000001', 'e1e1e1e1-0000-0000-0000-000000000004', 'Chicha morada', 5.00, 2, 10.00),
('b0b0b0b0-0000-0000-0000-000000000003', 'a0a0a0a0-0000-0000-0000-000000000002', 'e1e1e1e1-0000-0000-0000-000000000002', 'Adobo arequipeno', 30.00, 2, 60.00),
('b0b0b0b0-0000-0000-0000-000000000004', 'a0a0a0a0-0000-0000-0000-000000000003', 'e1e1e1e1-0000-0000-0000-000000000005', 'Ceviche clasico', 32.00, 1, 32.00),
('b0b0b0b0-0000-0000-0000-000000000005', 'a0a0a0a0-0000-0000-0000-000000000003', 'e1e1e1e1-0000-0000-0000-000000000007', 'Limonada', 12.00, 1, 12.00);
