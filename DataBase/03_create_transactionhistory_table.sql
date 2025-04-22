CREATE TABLE `TransactionHistory` (
  `Id`         INT               NOT NULL AUTO_INCREMENT,
  `WalletId`   INT               NOT NULL,
  `Amount`     DECIMAL(18,2)     NOT NULL,
  `Type`       ENUM('Credit','Debit') NOT NULL,
  `CreatedAt`  DATETIME(6)       NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`Id`),
  CONSTRAINT `fk_th_wallet`
    FOREIGN KEY (`WalletId`)
    REFERENCES `Wallet`(`Id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE,
  CONSTRAINT `chk_amount_positive`
    CHECK (`Amount` > 0)
);