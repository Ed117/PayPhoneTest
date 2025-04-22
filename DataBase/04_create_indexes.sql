-- Índices adicionales y constraints

-- Índice para búsquedas frecuentes por DocumentId
CREATE INDEX `idx_wallet_documentId`
  ON `Wallet`(`DocumentId`);

-- Índice para consulta de historial por billetera y fecha
CREATE INDEX `idx_th_wallet_createdAt`
  ON `TransactionHistory`(`WalletId`, `CreatedAt` DESC);