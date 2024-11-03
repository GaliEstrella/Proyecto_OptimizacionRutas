<?php
$host = 'localhost';
$dbname = 'Kiosko';
$username = 'root';
$password = '';

$mysqli = new mysqli($host, $username, $password, $dbname);

if ($mysqli->connect_error) {
    die("Error en la conexión: " . $mysqli->connect_error);
}

// Función para registrar usuario
function registrarUsuario($nombre, $contraseña, $empresa, $correo) {
    global $mysqli;

    $stmt = $mysqli->prepare("INSERT INTO usuarios (Nom_Usuario, Contraseña, Nom_Empresa, Correo) VALUES (?, ?, ?, ?)");
    if ($stmt === false) {
        die("Error en la preparación de la consulta: " . $mysqli->error);
    }

    $stmt->bind_param("ssss", $nombre, $contraseña, $empresa, $correo);

    if ($stmt->execute()) {
        echo "Registro exitoso";
    } else {
        echo "Error al registrar el usuario: " . $stmt->error;
    }

    $stmt->close();
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
