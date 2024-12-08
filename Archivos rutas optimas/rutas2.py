import pandas as pd
import numpy as np
from geopy.distance import geodesic
import logging
from typing import List, Tuple, Dict
import pyodbc
from sqlalchemy import create_engine, text

# Configuración de logging
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')

# Configuración de la conexión a la base de datos
connection_string = "DRIVER={ODBC Driver 17 for SQL Server};SERVER=HP255G8\\SERVIDOR;DATABASE=RouteMinds;UID=sa;PWD=123;TrustServerCertificate=yes;"
engine = create_engine(f"mssql+pyodbc:///?odbc_connect={connection_string}")

def leer_datos():
    """Lee los datos de las tablas Almacenes y Tiendas."""
    logging.info("Leyendo datos de la base de datos...")
    try:
        with engine.connect() as connection:
            almacenes = pd.read_sql_query("SELECT * FROM Almacenes", connection)
            tiendas = pd.read_sql_query("SELECT * FROM Tiendas", connection)
        
        logging.info("Columnas en Almacenes: %s", almacenes.columns.tolist())
        logging.info("Columnas en Tiendas: %s", tiendas.columns.tolist())
        
        # Verificar si las columnas necesarias están presentes
        columnas_almacenes = ['ALMACENID', 'NOMBRE', 'LATITUD', 'LONGITUD']
        columnas_tiendas = ['ALMACEN', 'TIENDA', 'NOMBRE', 'LATITUD', 'LONGITUD', 'TARIMAS']
        
        for col in columnas_almacenes:
            if col not in almacenes.columns:
                raise KeyError(f"La columna '{col}' no está presente en Almacenes")
        
        for col in columnas_tiendas:
            if col not in tiendas.columns:
                raise KeyError(f"La columna '{col}' no está presente en Tiendas")
        
        return almacenes, tiendas
    except Exception as e:
        logging.error("Error al leer datos de la base de datos: %s", str(e))
        raise

def calcular_distancia(punto1: Dict[str, float], punto2: Dict[str, float]) -> float:
    """Calcula la distancia geodésica entre dos puntos."""
    return geodesic((punto1['LATITUD'], punto1['LONGITUD']), (punto2['LATITUD'], punto2['LONGITUD'])).km

def calcular_distancia_ruta(almacen: Dict[str, float], tiendas: List[Dict[str, float]]) -> float:
    """Calcula la distancia total de una ruta, incluyendo el retorno al almacén."""
    distancia_total = 0
    punto_actual = almacen
    for tienda in tiendas:
        distancia_total += calcular_distancia(punto_actual, tienda)
        punto_actual = tienda
    distancia_total += calcular_distancia(punto_actual, almacen)  # Retorno al almacén
    return distancia_total

def generar_rutas_optimizadas(almacen: Dict[str, float], tiendas: pd.DataFrame) -> List[Tuple[List[int], int, float]]:
    """Genera rutas optimizadas para un almacén y sus tiendas asociadas."""
    logging.info(f"Generando rutas optimizadas para almacén {almacen['ALMACENID']}")
    rutas = []
    tiendas_no_asignadas = tiendas.copy()
    
    while not tiendas_no_asignadas.empty:
        ruta_actual = []
        tarimas_total = 0
        
        # Seleccionar la primera tienda (la más lejana al almacén)
        distancias = tiendas_no_asignadas.apply(lambda row: calcular_distancia(almacen, row), axis=1)
        primera_tienda = tiendas_no_asignadas.loc[distancias.idxmax()]
        ruta_actual.append(primera_tienda)
        tarimas_total += primera_tienda['TARIMAS']
        tiendas_no_asignadas = tiendas_no_asignadas[tiendas_no_asignadas['TIENDA'] != primera_tienda['TIENDA']]
        
        while tarimas_total < 10 and not tiendas_no_asignadas.empty:
            # Encontrar la tienda más cercana a la última tienda en la ruta
            ultima_tienda = ruta_actual[-1]
            distancias = tiendas_no_asignadas.apply(lambda row: calcular_distancia(ultima_tienda, row), axis=1)
            tienda_cercana = tiendas_no_asignadas.loc[distancias.idxmin()]
            
            if tarimas_total + tienda_cercana['TARIMAS'] <= 10:
                ruta_actual.append(tienda_cercana)
                tarimas_total += tienda_cercana['TARIMAS']
                tiendas_no_asignadas = tiendas_no_asignadas[tiendas_no_asignadas['TIENDA'] != tienda_cercana['TIENDA']]
            else:
                break
        
        distancia_total = calcular_distancia_ruta(almacen, ruta_actual)
        rutas.append(([tienda['TIENDA'] for tienda in ruta_actual], tarimas_total, distancia_total))
    
    return rutas

def guardar_resultados(resultados: List[Dict]):
    """Guarda los resultados en la tabla Rutas."""
    logging.info("Guardando resultados en la tabla Rutas...")
    try:
        with engine.connect() as connection:
            # Crear la tabla Rutas si no existe
            connection.execute(text("""
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Rutas' AND xtype = 'U')
                BEGIN
                    CREATE TABLE Rutas (
                        Almacen INT NOT NULL,
                        Ruta NVARCHAR(50) NOT NULL,
                        Tienda INT NOT NULL,
                        Tarimas INT NOT NULL,
                        Distancia FLOAT NOT NULL,
                        CONSTRAINT PK_Rutas PRIMARY KEY (Almacen, Ruta, Tienda),
                        CONSTRAINT FK_Rutas_Almacenes FOREIGN KEY (Almacen) REFERENCES Almacenes(ALMACENID)
                    )
                END
            """))
            
            # Truncar la tabla Rutas si existe
            connection.execute(text("IF OBJECT_ID('Rutas', 'U') IS NOT NULL TRUNCATE TABLE Rutas"))
            
            # Insertar los nuevos datos
            for resultado in resultados:
                # Convertir numpy.int64 a int estándar de Python
                resultado_converted = {
                    'Almacen': int(resultado['Almacen']),
                    'Ruta': resultado['Ruta'],
                    'Tienda': int(resultado['Tienda']),
                    'Tarimas': int(resultado['Tarimas']),
                    'Distancia': float(resultado['Distancia'])
                }
                connection.execute(text("""
                    INSERT INTO Rutas (Almacen, Ruta, Tienda, Tarimas, Distancia)
                    VALUES (:Almacen, :Ruta, :Tienda, :Tarimas, :Distancia)
                """), resultado_converted)
            
            # Commit the transaction
            connection.commit()
        
        logging.info(f"Se insertaron {len(resultados)} filas en la tabla Rutas.")
        
        # Verificar la inserción
        with engine.connect() as connection:
            count = connection.execute(text("SELECT COUNT(*) FROM Rutas")).scalar()
        logging.info(f"La tabla Rutas ahora contiene {count} filas.")
        
    except Exception as e:
        logging.error("Error al guardar los resultados en la base de datos: %s", str(e))
        raise

def main():
    try:
        almacenes, tiendas = leer_datos()
        
        resultados = []
        contador_rutas = 0  # Inicializar el contador de rutas
        
        # Agregar los almacenes como las primeras líneas
        for _, almacen in almacenes.iterrows():
            resultados.append({
                'Almacen': almacen['ALMACENID'],
                'Ruta': 'Almacen',
                'Tienda': almacen['ALMACENID'],
                'Tarimas': 10,
                'Distancia': 0
            })
        
        for _, almacen in almacenes.iterrows():
            tiendas_almacen = tiendas[tiendas['ALMACEN'] == almacen['ALMACENID']]
            if tiendas_almacen.empty:
                logging.warning(f"No hay tiendas asociadas al almacén {almacen['ALMACENID']}")
                continue
            
            rutas = generar_rutas_optimizadas(almacen, tiendas_almacen)
            
            for ruta, tarimas, distancia in rutas:
                contador_rutas += 1  # Incrementar el contador de rutas
                for tienda in ruta:
                    tarimas_tienda = tiendas_almacen.loc[tiendas_almacen['TIENDA'] == tienda, 'TARIMAS'].values[0]
                    resultados.append({
                        'Almacen': almacen['ALMACENID'],
                        'Ruta': f"ruta{contador_rutas}",
                        'Tienda': tienda,
                        'Tarimas': tarimas_tienda,
                        'Distancia': distancia
                    })
        
        # Guardar resultados en la base de datos
        guardar_resultados(resultados)
        logging.info("Proceso completado.")
    
    except Exception as e:
        logging.error("Se produjo un error durante la ejecución: %s", str(e))
if __name__ == "__main__":

    main()