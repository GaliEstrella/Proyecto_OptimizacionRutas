<?php
$host = 'localhost'; // Servidor de base de datos
$dbname = 'Kiosko'; // Nombre de la base de datos
$username = 'root'; // Usuario de la base de datos
$password = ''; // Contraseña de la base de datos

$mysqli = new mysqli($host, $username, $password, $dbname);

if ($mysqli->connect_error) {
    die("Error en la conexión: " . $mysqli->connect_error);
} else {
    echo "Conexión exitosa con MySQLi";
}
?>
