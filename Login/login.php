<?php
require "./database_connection.php";

try {
    $db = new databaseConnection();
    $conn = $db->Create_connection();
} catch (Exception $e) {
    die("PDO Connection Error: " . $e->getMessage());
}

function ComprobarUsuario($conn, $username, $password) {
    try {
        // Obtener el hash de la contraseña para el usuario
        $stmt = $conn->prepare("SELECT Contraseña FROM usuarios WHERE Nom_Usuario = :nomUsuario");
        $stmt->bindParam(':nomUsuario', $username, PDO::PARAM_STR);
        $stmt->execute();

        $usuario = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($usuario) {
            // Comparar la contraseña ingresada con el hash almacenado
            return password_verify($password, $usuario['Contraseña']);
        }

        return false; // Usuario no encontrado
    } catch (PDOException $e) {
        die("Error al comprobar usuario: " . $e->getMessage());
    }
}

$mensaje_error = "";

// Procesar el formulario
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $username = $_POST['username'];
    $password = $_POST['password'];

    if (ComprobarUsuario($conn, $username, $password)) {
        header("Location: index.html");
        exit;
    } else {
        $mensaje_error = "Usuario o contraseña incorrectos";
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

        <!-- Formulario de inicio de sesión -->
        <form method="POST" action="" style="display: flex; flex-direction: column; align-items: center; background-color: white; margin: 8rem 4rem; padding: 2rem; border-radius: 2rem; width: 70%;">
            <div class="input-field">
                <i class="fas fa-user"></i>
                <input type="text" name="username" placeholder="username" required />
            </div>
            <div class="input-field">
                <i class="fas fa-lock"></i>
                <input type="password" name="password" placeholder="password" required />
            </div>
            <input type="submit" value="Iniciar Sesión" class="btn solid" />
            <div class="social-text"> ¿No tienes una cuenta? <a href="register.php">Regístrate</a> </div>
            <p style="color: red;"><?= $mensaje_error ?></p>
        </form>
    </div>
</body>

</html>

