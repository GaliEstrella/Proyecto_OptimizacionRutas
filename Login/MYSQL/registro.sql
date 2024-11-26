CREATE PROCEDURE obtener_o_agregar_empresa (
    IN p_nom_empresa VARCHAR(30),
    OUT p_id_empresa INT
)
BEGIN
    -- Verificar si la empresa ya existe
    SELECT Id_Empresa INTO p_id_empresa
    FROM empresa
    WHERE Nom_Empresa = p_nom_empresa
    LIMIT 1;

    -- Si la empresa no existe, insertarla
    IF p_id_empresa IS NULL THEN
        -- Insertar la empresa
        INSERT INTO empresa (Nom_Empresa)
        VALUES (p_nom_empresa);

        -- Obtener el Id_Empresa recién insertado
        SET p_id_empresa = LAST_INSERT_ID();
    END IF;
END




-- Funcion para registrar en la tabla de usuarios
CREATE PROCEDURE verificar_o_insertar_usuario (
    IN p_nom_usuario VARCHAR(50),
    IN p_contrasena VARCHAR(200),
    IN p_correo VARCHAR(100),
    IN p_id_empresa INT,
    OUT p_resultado BOOLEAN
)
BEGIN
    DECLARE usuario_existe INT;
    DECLARE correo_existe INT;

    -- Verificar si el nombre de usuario ya existe
    SELECT COUNT(*) INTO usuario_existe
    FROM usuarios
    WHERE Nom_Usuario = p_nom_usuario;

    -- Verificar si el correo ya existe
    SELECT COUNT(*) INTO correo_existe
    FROM usuarios
    WHERE Correo = p_correo;

    -- Si el usuario o el correo ya existen, retornar FALSE (0)
    IF usuario_existe > 0 OR correo_existe > 0 THEN
        SET p_resultado = FALSE;
    ELSE
        -- Insertar el nuevo usuario
        INSERT INTO usuarios (Nom_Usuario, Contraseña, Correo, Id_Empresa)
        VALUES (p_nom_usuario, p_contrasena, p_correo, p_id_empresa);

        -- Retornar TRUE (1) si se insertó correctamente
        SET p_resultado = TRUE;
    END IF;
END 




USE kiosko