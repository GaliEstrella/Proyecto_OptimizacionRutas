<?php
// Requerir el archivo que contiene la conexión a la base de datos
require "./database_connection.php";

try {
    $db = new databaseConnection();
    $conn = $db->Create_connection();
} catch (Exception $e) {
    die("PDO Connection Error: " . $e->getMessage());
}

// Variable para almacenar el mensaje de error
$mensaje_error = "";

// Función para registrar empresa
function registarEmpresa($empresa){
    global $conn;
    try {
        $stmt = $conn->prepare("CALL obtener_o_agregar_empresa(:nom_empresa, @id_empresa)");
        $stmt->bindParam(":nom_empresa",$empresa,PDO::PARAM_STR);
        $stmt->execute();

        $output = $conn->query("SELECT @id_empresa AS id_empresa")->fetch(PDO::FETCH_ASSOC);

        return $output['id_empresa'];
    } catch (PDOException $e) {
        echo "Error: " . $e->getMessage();
    }
}
// Función para registrar usuario
function registrarUsuario($nombre, $contraseña, $empresa, $correo) {
    global $conn;

    try {
        // Hash the password
        $hashedPassword = password_hash($contraseña, PASSWORD_DEFAULT);
        // Llamar a la funcion registrar empresa
        $id_empresa = registarEmpresa($empresa);

        // Preparamos la llamada al procedimiento almacenado
        $stmt = $conn->prepare("CALL verificar_o_insertar_usuario(:nom_usuario, :contrasena, 
                                :correo, :id_empresa, @resultado)");

        // Enlazamos los parámetros de entrada
        $stmt->bindParam(":nom_usuario", $nombre, PDO::PARAM_STR);
        $stmt->bindParam(":contrasena", $hashedPassword, PDO::PARAM_STR);
        $stmt->bindParam(":correo", $correo, PDO::PARAM_STR);
        $stmt->bindParam(":id_empresa", $id_empresa,  PDO::PARAM_INT);

        // Ejecutamos el procedimiento almacenado
        $stmt->execute();

         // Obtener el resultado
        $resultado = $conn->query("SELECT @resultado AS resultado")->fetch(PDO::FETCH_ASSOC);

        // Verificamos si el registro fue exitoso
        if ($resultado['resultado']) {
            return "";  // Registro exitoso
        } else {
            // Si hay un error, devolvemos el mensaje de error
            return "El nombre de usuario o el correo ya existen";
        }

    } catch (PDOException $e) {
        // Capturamos cualquier error y lo mostramos
        return "Error al conectar a la base de datos: " . $e->getMessage();
    }
}


// Verificar si el formulario fue enviado
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    // Recibir datos del formulario
    $nombre = $_POST['nombre'];
    $contraseña = $_POST['contraseña'];
    $empresa = $_POST['empresa'];
    $correo = $_POST['correo'];

    // Llamar a la función para registrar el usuario
    $resultado = registrarUsuario($nombre, $contraseña, $empresa, $correo);

    // Manejar el resultado
    if ($resultado === "") {
        // Registro exitoso, redirigir al login
        header("Location: login.php");
        exit;
    } else {
        // Mostrar mensaje de error
        $mensaje_error = $resultado;
    }
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <title>Kiosko Analytics</title>
    <link rel="stylesheet" href="login.css">
    <script src="https://kit.fontawesome.com/64d58efce2.js" crossorigin="anonymous"></script>
</head>
<body style="display: flex; flex-direction: row;" class="backgroundLogin">
    <div id="image-login" style="height: 100%;width: 65%; display: flex; align-items: center; justify-content: center;">
        <img src="img/fondo_login.jpg" alt="tienda blanca" style="width: 90%;height: 90%; object-fit: cover;">
    </div>
    <div style="height: 100%; width: 35%; display: flex; flex-direction: column; align-items: center; justify-content: space-between;" class="backgroundLogin">
        <section style="margin-top: 0rem;">
            <div style="height: auto; width: 100%; display: flex; justify-content: center; margin-top:10%;">
                <img src="img/Logo.png" style="height: auto; width: 90%; border-radius: 10px; margin-top: 20px;" />
            </div>
        </section>

        <section style="display: flex; flex-direction: column; align-items: center; background-color: white; margin: 0rem 4rem; padding: 2rem;border-radius: 2rem; width: 70%; margin-bottom: 30%; margin-top:10%;">
            <form method="POST">
                <div class="input-field">
                    <i class="fas fa-user"></i>
                    <input type="text" name="nombre" placeholder="Nombre de Usuario" required />
                </div>
                <div class="input-field">
                    <i class="fas fa-lock"></i>
                    <input type="password" name="contraseña" placeholder="Contraseña" required />
                </div>
                <div class="input-field">
                    <i class="fas fa-building"></i>
                    <input type="text" name="empresa" placeholder="Empresa" required />
                </div>
                <div class="input-field">
                    <i class="fas fa-envelope"></i>
                    <input type="email" name="correo" placeholder="correo@gmail.com" required />
                </div>
                <?php if ($mensaje_error !== ""): ?>
                    <p style="color: red;"><?= $mensaje_error ?></p>
                <?php endif; ?>
                <input type="submit" value="Registrarme" class="btn solid" />
            </form>
        </section>
    </div>
</body>
</html>
