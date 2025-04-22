CREATE TABLE `Wallet` (
  `Id`          INT               NOT NULL AUTO_INCREMENT,
  `DocumentId`  VARCHAR(50)       NOT NULL,
  `OwnerName`   VARCHAR(100)      NOT NULL,
  `Balance`     DECIMAL(18,2)     NOT NULL DEFAULT 0.00,
  `CreatedAt`   DATETIME(6)       NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
  `UpdatedAt`   DATETIME(6)       NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
                                ON UPDATE CURRENT_TIMESTAMP(6),
  PRIMARY KEY (`Id`)
);