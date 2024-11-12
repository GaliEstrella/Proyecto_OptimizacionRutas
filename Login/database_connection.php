<?php
    /**
     * Clase para realizar la conexión con la base de datos
     */

    class databaseConnection{
        private $host = "127.0.0.4";
        private $user = "root";
        private $database = "kiosko";
        private $password = "1421";
        
        /**
         * Función publica donde se genera la conexión a la base 
         * de datos
         * 
         * Retorna -> Objeto de conexión
         */
        public function Create_connection(){
            $conn = $this->Connection();
            return $conn;
        }
        
        /**
         * Función para conectarnos a la base de datos 
         * 
         * Retorna -> Objeto de conexión
         */
        private function Connection(){
            
            try {
                $conn = new PDO("mysql:host={$this->host};dbname={$this->database}", $this->user, $this->password);
                $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
                return $conn;
            } catch (PDOException $e) {
                // Manejo de errores - puedes lanzar una excepción o registrar el error
                throw new Exception("PDO Connection Error: " . $e->getMessage());
            }
        }
    }
?>