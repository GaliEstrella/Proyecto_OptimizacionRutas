<?php
require "./database_connection.php";


try {
    $db = new databaseConnection();
    $conn = $db->Create_connection();
} catch (Exception $e) {
    die("PDO Connection Error: " . $e->getMessage());
}

// Función para registrar usuario
function registrarUsuario($nombre, $contraseña, $empresa, $correo) {
    global $conn;

    try {
        $stmt = $conn->prepare("INSERT INTO usuarios (Nom_Usuario,Contraseña, Nom_Empresa, Correo) Values (:nombre, :contraseña, :empresa, :correo) ");
        $stmt->bindParam(":name",$nombre);
        $stmt->bindParam(":contraseña",$contraseña);
        $stmt->bindParam(":empresa",$empresa);
        $stmt->bindParam(":correo",$correo);
        if ($stmt->execute()) {
            header("Location: login.html");
        }else {
            echo "Error al registrar el usuario: " . $stmt->errorInfo()[2];
        }

    } catch (Exception $e) {
        die("Error al conectarse a la base de datos: ". $e->getMessage());
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
    registrarUsuario($nombre, $contraseña, $empresa, $correo);
}
?>
