-- Datos de ejemplo o semilla (opcional)
USE `finance`;

INSERT INTO `Wallet` (`DocumentId`, `OwnerName`, `Balance`)
VALUES
  ('12345678', 'Alice Smith', 1000.00),
  ('87654321', 'Bob Johnson', 500.00),
  ('11223344', 'Carlos Méndez', 750.50),
  ('44332211', 'Diana López', 1200.00);

INSERT INTO `TransactionHistory` (`WalletId`, `Amount`, `Type`)
VALUES
  (1, 1000.00, 'Credit'),
  (1, 200.00, 'Debit'),
  (2, 500.00, 'Credit'),
  (3, 300.00, 'Credit'),
  (3, 50.25, 'Debit'),
  (4, 1200.00, 'Credit'),
  (4, 150.00, 'Debit');
