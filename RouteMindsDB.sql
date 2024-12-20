USE [master]
GO
/****** Object:  Database [RouteMinds]    Script Date: 08/12/2024 02:09:24 p. m. ******/
CREATE DATABASE [RouteMinds]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RouteMinds', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SERVIDOR\MSSQL\DATA\RouteMinds.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RouteMinds_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SERVIDOR\MSSQL\DATA\RouteMinds_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RouteMinds] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RouteMinds].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RouteMinds] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RouteMinds] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RouteMinds] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RouteMinds] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RouteMinds] SET ARITHABORT OFF 
GO
ALTER DATABASE [RouteMinds] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RouteMinds] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RouteMinds] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RouteMinds] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RouteMinds] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RouteMinds] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RouteMinds] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RouteMinds] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RouteMinds] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RouteMinds] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RouteMinds] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RouteMinds] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RouteMinds] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RouteMinds] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RouteMinds] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RouteMinds] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RouteMinds] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RouteMinds] SET RECOVERY FULL 
GO
ALTER DATABASE [RouteMinds] SET  MULTI_USER 
GO
ALTER DATABASE [RouteMinds] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RouteMinds] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RouteMinds] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RouteMinds] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RouteMinds] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RouteMinds] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RouteMinds', N'ON'
GO
ALTER DATABASE [RouteMinds] SET QUERY_STORE = ON
GO
ALTER DATABASE [RouteMinds] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [RouteMinds]
GO
/****** Object:  Table [dbo].[Almacenes]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Almacenes](
	[ALMACENID] [int] NOT NULL,
	[NOMBRE] [nvarchar](255) NOT NULL,
	[LATITUD] [float] NOT NULL,
	[LONGITUD] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ALMACENID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[EmpresaId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmpresaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rutas]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rutas](
	[Almacen] [int] NOT NULL,
	[Ruta] [nvarchar](50) NOT NULL,
	[Tienda] [int] NOT NULL,
	[Tarimas] [int] NOT NULL,
	[Distancia] [float] NOT NULL,
 CONSTRAINT [PK_Rutas] PRIMARY KEY CLUSTERED 
(
	[Almacen] ASC,
	[Ruta] ASC,
	[Tienda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tiendas]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tiendas](
	[ALMACEN] [int] NOT NULL,
	[TIENDA] [int] NOT NULL,
	[NOMBRE] [nvarchar](50) NOT NULL,
	[LATITUD] [float] NOT NULL,
	[LONGITUD] [float] NOT NULL,
	[TARIMAS] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](255) NOT NULL,
	[EmpresaId] [int] NULL,
	[Correo] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Correo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_Usuarios_Empresas] FOREIGN KEY([EmpresaId])
REFERENCES [dbo].[Empresas] ([EmpresaId])
GO
ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_Usuarios_Empresas]
GO
/****** Object:  StoredProcedure [dbo].[AuthenticateUser]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE PROCEDURE [dbo].[AuthenticateUser]
    @Username NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    -- Set NOCOUNT ON to prevent extra result sets from interfering
    SET NOCOUNT ON;

    -- Query the Usuarios table to verify credentials
    SELECT 
        UsuarioId,
        Nombre,
        Correo,
        EmpresaId
    FROM 
        Usuarios
    WHERE 
        Nombre = @Username
        AND Password = @Password; -- Ensure passwords are stored securely (e.g., hashed)

    -- If no rows are returned, the authentication failed.
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateOrUpdateAlmacen]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAlmacen]
    @ALMACENID INT,
    @NOMBRE NVARCHAR(255),
    @LATITUD FLOAT,
    @LONGITUD FLOAT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validar si el ALMACENID ya existe
        IF EXISTS (SELECT 1 FROM [dbo].[Almacenes] WHERE ALMACENID = @ALMACENID)
        BEGIN
            -- Actualizar el registro existente
            UPDATE [dbo].[Almacenes]
            SET 
                NOMBRE = @NOMBRE,
                LATITUD = @LATITUD,
                LONGITUD = @LONGITUD
            WHERE ALMACENID = @ALMACENID;

            PRINT 'Registro actualizado correctamente.';
        END
        ELSE
        BEGIN
            -- Crear un nuevo registro
            INSERT INTO [dbo].[Almacenes] (ALMACENID, NOMBRE, LATITUD, LONGITUD)
            VALUES (@ALMACENID, @NOMBRE, @LATITUD, @LONGITUD);

            PRINT 'Registro creado correctamente.';
        END
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        PRINT 'Error al crear o actualizar el almacén.';
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateOrUpdateTienda]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateTienda]
    @ALMACEN INT,
    @TIENDA INT,
    @NOMBRE NVARCHAR(50),
    @LATITUD FLOAT,
    @LONGITUD FLOAT,
    @TARIMAS INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si la tienda ya existe
    IF EXISTS (SELECT 1 FROM Tiendas WHERE TIENDA = @TIENDA)
    BEGIN
        -- Actualizar los datos de la tienda
        UPDATE Tiendas
        SET 
            ALMACEN = @ALMACEN,
            NOMBRE = @NOMBRE,
            LATITUD = @LATITUD,
            LONGITUD = @LONGITUD,
            TARIMAS = @TARIMAS
        WHERE TIENDA = @TIENDA;

        PRINT 'Tienda actualizada correctamente.';
    END
    ELSE
    BEGIN
        -- Insertar nueva tienda
        INSERT INTO Tiendas (ALMACEN, TIENDA, NOMBRE, LATITUD, LONGITUD, TARIMAS)
        VALUES (@ALMACEN, @TIENDA, @NOMBRE, @LATITUD, @LONGITUD, @TARIMAS);

        PRINT 'Tienda creada correctamente.';
    END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteAlmacen]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_DeleteAlmacen]
    @ALMACENID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DELETE FROM [dbo].[Almacenes]
        WHERE ALMACENID = @ALMACENID;

        PRINT 'Eliminado correctamente';
    END TRY
    BEGIN CATCH
        -- Manejo de errores
        PRINT 'Error al eliminar el almacén.';
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteTienda]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteTienda]
    @TIENDA INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si la tienda existe
    IF EXISTS (SELECT 1 FROM Tiendas WHERE TIENDA = @TIENDA)
    BEGIN
        DELETE FROM Tiendas WHERE TIENDA = @TIENDA;
        PRINT 'Tienda eliminada correctamente.';
    END
    ELSE
    BEGIN
        THROW 50000, 'La tienda no existe.', 1;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_postRegistrarUsuario]    Script Date: 08/12/2024 02:09:24 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_postRegistrarUsuario]
    @NombreUsuario NVARCHAR(255),
    @NombreEmpresa NVARCHAR(255),
    @Correo NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Iniciar transacción
        BEGIN TRANSACTION;

        -- Insertar en la tabla Empresas
        INSERT INTO Empresas (Nombre)
        VALUES (@NombreEmpresa);

        -- Obtener el último EmpresaId generado
        DECLARE @EmpresaId INT;
        SET @EmpresaId = SCOPE_IDENTITY();

        -- Insertar en la tabla Usuarios
        INSERT INTO Usuarios (Nombre, EmpresaId, Correo, Password)
        VALUES (@NombreUsuario, @EmpresaId, @Correo, @Password);

        -- Confirmar transacción
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Revertir la transacción en caso de error
        ROLLBACK TRANSACTION;

        -- Manejar el error
        THROW;
    END CATCH
END
GO
USE [master]
GO
ALTER DATABASE [RouteMinds] SET  READ_WRITE 
GO
