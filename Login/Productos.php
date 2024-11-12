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

/**
 * Función que se encarga de extraer los datos de la tabla de productos
 * 
 * Retorna -> array
 */
function Extraer_productos(){
    global $conn;
    $stmt = $conn->prepare("SELECT * FROM productos");
    $stmt->execute();
    return $stmt->fetchAll(PDO::FETCH_ASSOC);
}

?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="./Productos.css">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</head>
<body>
    <!--Contiene el nav-->
    <header>
        <nav>
            <ul>
                <div class="left-links">
                    <li><a href="./index.html">Inicio</a></li>
                    <li><a href="./Productos.html">Productos</a></li>
                    <li><a href="#">Tiendas</a></li>
                    <li><a href="#">Camiones</a></li>
                    <li><a href="#">Rutas</a></li>
                </div>
                <div class="right-link">
                    <li><a class="logout" href="./login.html">Cerrar Sesión</a></li>
                </div>
            </ul>
        </nav>
    </header>
    <!--Termino del nav-->

    <section>
        <!-- Contendor del titulo de la pagina -->
        <div id="Titulo_pag">
            <h3>Productos</h3>
        </div>
        <div class="container mt-4">
            <table class="table table-bordered">
                <thead>
                    <!-- Titulos de las columnas -->
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Cantidad</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <?php
                    // Llamar a la función para obtener el array que contiene los productos
                    $productos = Extraer_productos();
                    // Bucle donde se recorre el array de productos
                    foreach($productos as $producto):
                    ?>
                        <tr>
                            <!-- Obtener los datos del campo Id_producto -->
                            <td><?= $producto['Id_Producto'] ?></td>
                            <!-- Obtener los datos del campo Nom_Producto' -->
                            <td><?= $producto['Nom_Producto'] ?></td>
                            <!-- Obtener los datos del campo Cantidad -->
                            <td><?= $producto['Cantidad'] ?></td>
                            <td>
                                <!-- Boton de eleminar -->
                                <button class="btn-31">
                                    <span class="text-container">
                                        <span class="text">Eliminar</span>
                                    </span>
                                </button>
                                <!-- Termino de boton de eliminar -->
                            </td>
                        </tr>
                    <!-- Terminar el bucle -->
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>
    </section>
</body>
</html>