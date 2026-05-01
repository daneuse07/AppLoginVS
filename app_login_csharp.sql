-- 1. Crear y seleccionar la base de datos
DROP DATABASE IF EXISTS app_login_csharp;
CREATE DATABASE app_login_csharp
 DEFAULT CHARACTER SET utf8mb4
 DEFAULT COLLATE utf8mb4_unicode_ci;
USE app_login_csharp;
-- 2. Tabla principal de usuarios
CREATE TABLE usuarios (
 id INT NOT NULL AUTO_INCREMENT,
 nombre VARCHAR(100) NOT NULL,
 apellido VARCHAR(100) NOT NULL,
 email VARCHAR(150) NOT NULL UNIQUE,
 usuario VARCHAR(60) NOT NULL UNIQUE,
 contrasena VARCHAR(255) NOT NULL,
 foto MEDIUMBLOB DEFAULT NULL, -- Hasta 16 MB
 nivel_seg TINYINT DEFAULT 0, -- 0=Muy débil ... 4=Muy fuerte
 intentos TINYINT DEFAULT 0, -- Contador intentos fallidos
 bloqueado TINYINT(1) DEFAULT 0, -- 1 = cuenta bloqueada
 ultimo_acc DATETIME DEFAULT NULL,
 fecha_reg DATETIME DEFAULT CURRENT_TIMESTAMP,
 activo TINYINT(1) DEFAULT 1,
 PRIMARY KEY (id),
 INDEX idx_usuario (usuario),
 INDEX idx_email (email)
);
-- 3. Tabla de log de accesos
CREATE TABLE log_accesos (
 id INT NOT NULL AUTO_INCREMENT,
 usuario_id INT NOT NULL,
 tipo ENUM('LOGIN_OK','LOGIN_FAIL','LOGOUT') NOT NULL,
 ip_cliente VARCHAR(45) DEFAULT NULL,
 descripcion VARCHAR(200) DEFAULT NULL,
 fecha DATETIME DEFAULT CURRENT_TIMESTAMP,
 PRIMARY KEY (id),
 FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE,
 INDEX idx_fecha (fecha)
);
-- 4. Tabla de sesiones activas
CREATE TABLE sesiones (
 id INT NOT NULL AUTO_INCREMENT,
 usuario_id INT NOT NULL,
 token VARCHAR(100) NOT NULL UNIQUE,
 inicio DATETIME DEFAULT CURRENT_TIMESTAMP,
 expira DATETIME NOT NULL,
 activa TINYINT(1) DEFAULT 1,
 PRIMARY KEY (id),
 FOREIGN KEY (usuario_id) REFERENCES usuarios(id) ON DELETE CASCADE
);
-- ═══════════════════════════════════════════════════════════════
-- 5. Stored Procedure: Login con bloqueo automático
-- ═══════════════════════════════════════════════════════════════
DELIMITER $$
CREATE PROCEDURE sp_login(
 IN p_usuario VARCHAR(60),
 IN p_contrasena VARCHAR(255),
 OUT p_resultado VARCHAR(30)
)
BEGIN
 DECLARE v_id INT DEFAULT 0;
 DECLARE v_bloqueado TINYINT DEFAULT 0;
 DECLARE v_intentos TINYINT DEFAULT 0;
 DECLARE v_pass VARCHAR(255) DEFAULT '';
 DECLARE v_activo TINYINT DEFAULT 0;
 -- Buscar usuario
 SELECT id, bloqueado, intentos, contrasena, activo
 INTO v_id, v_bloqueado, v_intentos, v_pass, v_activo
 FROM usuarios WHERE usuario = p_usuario LIMIT 1;
 IF v_id = 0 THEN
 SET p_resultado = 'USUARIO_NO_EXISTE';
 ELSEIF v_activo = 0 THEN
 SET p_resultado = 'CUENTA_INACTIVA';
 ELSEIF v_bloqueado = 1 THEN
 SET p_resultado = 'CUENTA_BLOQUEADA';
 ELSEIF v_pass = p_contrasena THEN
 -- Credenciales correctas
 UPDATE usuarios SET intentos = 0, ultimo_acc = NOW()
 WHERE id = v_id;
 INSERT INTO log_accesos (usuario_id, tipo, descripcion)
 VALUES (v_id, 'LOGIN_OK', 'Acceso exitoso');
 SET p_resultado = 'OK';
 ELSE
 -- Contraseña incorrecta
 UPDATE usuarios SET intentos = intentos + 1 WHERE id = v_id;
 INSERT INTO log_accesos (usuario_id, tipo, descripcion)
 VALUES (v_id, 'LOGIN_FAIL', CONCAT('Intento ', v_intentos+1));
 IF v_intentos + 1 >= 5 THEN
 UPDATE usuarios SET bloqueado = 1 WHERE id = v_id;
 SET p_resultado = 'CUENTA_BLOQUEADA';
 ELSE
 SET p_resultado = 'CREDENCIALES_INVALIDAS';
 END IF;
 END IF;
END$$
DELIMITER ;
-- ═══════════════════════════════════════════════════════════════
-- 6. Vista para reportes (sin datos sensibles)
-- ═══════════════════════════════════════════════════════════════
CREATE VIEW v_usuarios_resumen AS
SELECT id, nombre, apellido, email, usuario,
 nivel_seg, intentos, bloqueado,
 ultimo_acc, fecha_reg, activo
 FROM usuarios;
-- ═══════════════════════════════════════════════════════════════
-- 7. Datos de prueba
-- ═══════════════════════════════════════════════════════════════
-- Contraseña Admin@2024 cumple los 5 requisitos de seguridad
INSERT INTO usuarios (nombre, apellido, email, usuario, contrasena, nivel_seg)
VALUES ('Administrador', 'Sistema', 'admin@sistema.com', 'admin', 'Admin@2024',
4);
-- Contraseña: Test#9876
INSERT INTO usuarios (nombre, apellido, email, usuario, contrasena, nivel_seg)
VALUES ('Carlos', 'Mendoza', 'carlos@demo.com', 'cmendoza', 'Test#9876', 3);
-- Verificar
SELECT id, nombre, usuario, nivel_seg, fecha_reg FROM usuarios;
SELECT * FROM usuarios;

-- ═══════════════════════════════════════════════════════════════
-- Script completado: base de datos app_login_csharp lista
-- ══════════════════════════════════════════════════════════════
SHOW DATABASES;