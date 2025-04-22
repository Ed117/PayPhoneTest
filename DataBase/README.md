#Carpeta DataBase

Este directorio contiene todos los scripts SQL necesarios para la creación la configuración y la inicialización de la base de datos MySQL requerida por WalletTransfer.API.

##Estructura de la carpeta

Cada archivo debe seguir el prefijo numérico para garantizar un orden de ejecución claro:

01_create_database.sql         -- Creación de la base de datos (por ejemplo, `finance`)
02_create_wallet_table.sql     -- Script de creación de la tabla `Wallet`
03_create_transactionhistory_table.sql  -- Script de creación de la tabla `TransactionHistory`
04_create_indexes.sql         -- Índices adicionales y constraints
05_seed_data.sql              -- Datos de ejemplo o semilla (opcional)

##Uso

Abrir MySQL Workbench y conectar al servidor MySQL.

(Opcional) Crear la base de datos si no existe:

CREATE DATABASE IF NOT EXISTS `finance` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `finance`;
