-- Datos de prueba del RestaurantService de AQPlunch
-- Los restaurant_id son los mismos que usa Identity para restaurante1 y restaurante2
-- Los qr_token son UUID al azar, como los que generara la API al crear una mesa

INSERT INTO restaurants (restaurant_id, name, address, phone) VALUES
('f1f1f1f1-0000-0000-0000-000000000001', 'Picanteria Los Volcanes', 'Av. Ejercito 101, Arequipa', '054000001'),
('f1f1f1f1-0000-0000-0000-000000000002', 'Cevicheria Mar Azul', 'Calle Mercaderes 202, Arequipa', '054000002');

INSERT INTO dining_tables (table_id, restaurant_id, table_number, qr_token) VALUES
('d1d1d1d1-0000-0000-0000-000000000001', 'f1f1f1f1-0000-0000-0000-000000000001', 1, '552062cd-de5f-4f05-afcf-ddc06784bdfd'),
('d1d1d1d1-0000-0000-0000-000000000002', 'f1f1f1f1-0000-0000-0000-000000000001', 2, 'ac3b6458-53a9-41ef-9c68-dbd9be68a7c0'),
('d1d1d1d1-0000-0000-0000-000000000003', 'f1f1f1f1-0000-0000-0000-000000000001', 3, 'a1779ef4-be85-4152-a098-49bbe0c68cd0'),
('d1d1d1d1-0000-0000-0000-000000000004', 'f1f1f1f1-0000-0000-0000-000000000002', 1, '82ab26c3-1625-4466-af6f-c2d316823d76'),
('d1d1d1d1-0000-0000-0000-000000000005', 'f1f1f1f1-0000-0000-0000-000000000002', 2, '7fb2ed16-1fad-402b-830e-406ecc7ac765');

INSERT INTO categories (category_id, restaurant_id, name) VALUES
('cacacaca-0000-0000-0000-000000000001', 'f1f1f1f1-0000-0000-0000-000000000001', 'Entradas'),
('cacacaca-0000-0000-0000-000000000002', 'f1f1f1f1-0000-0000-0000-000000000001', 'Platos de fondo'),
('cacacaca-0000-0000-0000-000000000003', 'f1f1f1f1-0000-0000-0000-000000000001', 'Bebidas'),
('cacacaca-0000-0000-0000-000000000004', 'f1f1f1f1-0000-0000-0000-000000000002', 'Ceviches'),
('cacacaca-0000-0000-0000-000000000005', 'f1f1f1f1-0000-0000-0000-000000000002', 'Bebidas');

-- El Chupe de camarones queda sin disponibilidad para probar que no sale en el menu
INSERT INTO products (product_id, category_id, name, description, price, is_available) VALUES
('e1e1e1e1-0000-0000-0000-000000000001', 'cacacaca-0000-0000-0000-000000000001', 'Rocoto relleno', 'Rocoto relleno de carne con pastel de papa', 25.00, TRUE),
('e1e1e1e1-0000-0000-0000-000000000002', 'cacacaca-0000-0000-0000-000000000002', 'Adobo arequipeno', 'Adobo de cerdo con pan de tres puntas', 30.00, TRUE),
('e1e1e1e1-0000-0000-0000-000000000003', 'cacacaca-0000-0000-0000-000000000002', 'Chupe de camarones', 'Sopa de camarones con huevo y queso', 38.00, FALSE),
('e1e1e1e1-0000-0000-0000-000000000004', 'cacacaca-0000-0000-0000-000000000003', 'Chicha morada', 'Vaso de chicha morada', 5.00, TRUE),
('e1e1e1e1-0000-0000-0000-000000000005', 'cacacaca-0000-0000-0000-000000000004', 'Ceviche clasico', 'Ceviche de pescado con camote y choclo', 32.00, TRUE),
('e1e1e1e1-0000-0000-0000-000000000006', 'cacacaca-0000-0000-0000-000000000004', 'Ceviche mixto', 'Ceviche de pescado y mariscos', 36.00, TRUE),
('e1e1e1e1-0000-0000-0000-000000000007', 'cacacaca-0000-0000-0000-000000000005', 'Limonada', 'Jarra de limonada', 12.00, TRUE);
