-- Datos de prueba del IdentityService de AQPlunch
-- Contrasenas: admin = Admin123*, restaurante1 y restaurante2 = Restaurante123*
-- Los restaurant_id son los mismos que usara el script de datos de RestaurantService

INSERT INTO roles (role_id, role_name) VALUES
('11111111-1111-1111-1111-111111111111', 'Admin'),
('22222222-2222-2222-2222-222222222222', 'Restaurante');

INSERT INTO users (user_id, username, password_hash, email, full_name, restaurant_id) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', 'admin', '100000.J0cTty0KBz1JKOrqLqWr4w==.ZFMB3WRXL/1KdbSrnbkshlH+pEQOnoF8p2gmqEd+wy0=', 'admin@aqplunch.com', 'Administrador AQPlunch', NULL),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', 'restaurante1', '100000.vahqUmKW8FDrXNNPj6aJ5A==.wwG666y1CcgRDbOCU/1Esm/dyAB5oMyQXjNQAF1QVCw=', 'restaurante1@aqplunch.com', 'Encargado Restaurante 1', 'f1f1f1f1-0000-0000-0000-000000000001'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', 'restaurante2', '100000.SrNHJ65JM+0LfVNg1XygOg==.gN7ilDpxkmKCjFGCNpbJdp7Dq/36jOWVm1BFCjtcqFU=', 'restaurante2@aqplunch.com', 'Encargado Restaurante 2', 'f1f1f1f1-0000-0000-0000-000000000002');

INSERT INTO user_roles (user_id, role_id) VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa', '11111111-1111-1111-1111-111111111111'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb', '22222222-2222-2222-2222-222222222222'),
('cccccccc-cccc-cccc-cccc-cccccccccccc', '22222222-2222-2222-2222-222222222222');
