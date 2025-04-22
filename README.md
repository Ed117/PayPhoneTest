#PayPhoneTest

PayPhoneTest es una API REST desarrollada en .NET 8 (C#) siguiendo los principios de Clean Architecture para gestionar transferencias de saldo entre billeteras en dólares.

##Estructura de carpetas

.gitignore             # Archivos y carpetas ignoradas por Git
README.md              # Documentación principal (este archivo)
DataBase               # Scripts SQL para inicialización de MySQL
PostmanCollection      # Exportación de la colección de pruebas en Postman
WalletTransfer         # Solución de Visual Studio (.sln) y proyectos
.gitignore            

##Tecnologías y versiones

Lenguaje: C#
Plataforma: .NET 8.0
Framework: ASP.NET Core Web API
ORM: Entity Framework Core (Pomelo MySQL provider) / Dapper (opcional)
Base de datos: MySQL 8+
Pruebas: xUnit, EF Core InMemory, ASP.NET Core WebApplicationFactory

##Características principales

CRUD completo para Wallets.
Transferencias con registro de débito/crédito en TransactionHistory.
Validaciones en base de datos y en servicios (montos positivos, fondos suficientes).
Arquitectura limpia (Domain / Application / Infrastructure / API).
Documentación automática con Swagger.
Colección de pruebas en Postman.
Este proyecto se entrega como prueba práctica de desarrollo backend.