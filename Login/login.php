<?php
// requirir el archivo que contiene la conexion a la base de datos
require "./database_connection.php";

// crear el objeto para la conexión a la base de datos
try {
    $db = new databaseConnection();
    $conn = $db->Create_connection();
} catch (Exception $e) {
    die("PDO Connection Error: " . $e->getMessage());
}

function ComprobarUsuario($conn,$password,$username){
    $stmt = $conn->prepare("SELECT verificar_usuario(:nomUsuario,:contraseña) AS resultado");
    $stmt->bindParam(':nomUsuario', $username, PDO::PARAM_STR);
    $stmt->bindParam(':contraseña', $password,PDO::PARAM_STR);
    $stmt->execute();

    $respuesta = $stmt->fetch(PDO::FETCH_ASSOC);

    return $response['resultado'] == 1;
}

$mensaje_error = "";

// Comprobar que el formulario ha sido enviado
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $username = $_POST['username'];
    $password = $_POST['password'];

    $response = ComprobarUsuario($conn , $password ,$username);

    if ($response) {
        header("Location: index.html");
        exit;
    }else {
        $mensaje_error = "Error en usuario y contraseña";
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
        <img src="img/fondo_login.jpg" alt="tienda blanca" style="width: 100%;height: 100%; object-fit: cover;">
    </div>
    <div style="height: 100%; width: 35%; display: flex; flex-direction: column; align-items: center; justify-content: space-between;" class="backgroundLogin">

    <section style="margin-top: 3rem;">
        <div style="height: auto; width: 100%; display: flex; justify-content: center;">
            <img src="img/Logo.png" style="height: auto; width: 90%; border-radius: 10px; margin-top: 20px;" />
        </div>
    </section>

    <section style="display: flex; flex-direction: column; align-items: center; background-color: white; margin: 8rem 4rem; padding: 2rem;border-radius: 2rem; width: 70%; ">
        <div class="input-field">
            <i class="fas fa-user"></i>
            <input type="text" name="username" placeholder="username" />
        </div>
        <div class="input-field">
            <i class="fas fa-lock"></i>
            <input type="password" name="password" placeholder="password" />
        </div>
        <input type="button" value="iniciar sesión" class="btn solid" onclick="window.location.href='index.html';" />
        <div class="social-text"> ¿No tienes una cuenta? <a href="register.html">Regístrate</a> </div>
        <p style="color: red;"><?= $mensaje_error ?></p>
    </section>
        
    </div>
</body>

</html>
