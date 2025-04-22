# PayPhoneTest

PayPhoneTest es una API REST desarrollada en .NET 8 siguiendo los principios de Clean Architecture para gestionar transferencias de saldo entre billeteras en dólares. Incluye:

- CRUD completo para billeteras (Wallet).  
- Registro de historial de transacciones (TransactionHistory) con operaciones de crédito y débito.  
- Validaciones a nivel de base de datos y aplicación (montos positivos, fondos suficientes, existencia de billeteras).  
- Persistencia con Entity Framework Core y/o Dapper.  
- Pruebas unitarias e integración para garantizar la calidad del código.  
- Manejo de errores consistente y respuestas HTTP adecuadas.  
