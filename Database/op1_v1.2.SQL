/*
Created: 10.03.2022
Modified: 12.03.2022
Model: SQLite 3.32
Database: SQLite 3.32
*/

-- Create tables section -------------------------------------------------

-- Table Card

CREATE TABLE Card
(
  CardPK INTEGER NOT NULL
        CONSTRAINT CardPK PRIMARY KEY AUTOINCREMENT,
  OrganizationName TEXT,
  SubOrganization TEXT,
  DishName TEXT,
  OKUD TEXT,
  OKPO TEXT,
  OKDP TEXT,
  DishNumber TEXT,
  OperName TEXT,
  DocNumber TEXT,
  DateOfDoc datetime,
  CONSTRAINT CardPK UNIQUE (CardPK)
)
;

-- Table Product

CREATE TABLE Product
(
  ProductPK INTEGER NOT NULL
        CONSTRAINT ProductPK PRIMARY KEY AUTOINCREMENT,
  NameProd TEXT,
  Code int,
  Number int,
  CardPK INTEGER NOT NULL,
  CONSTRAINT ProductPK UNIQUE (ProductPK),
  CONSTRAINT CardToProduct
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
      ON DELETE RESTRICT
)
;

CREATE INDEX IX_CardToProduct
  ON Product (CardPK)
;

-- Table Calculation

CREATE TABLE Calculation
(
  CalcPK INTEGER NOT NULL
        CONSTRAINT CalcPK PRIMARY KEY AUTOINCREMENT,
  numberCalc int,
  dateCalc datetime,
  DishWeght float,
  ExtraChargePercent float,
  ExtraChargeMoney int,
  Zaveduushiy TEXT,
  Sostavitel TEXT,
  Rukovoditel TEXT,
  CardPK INTEGER NOT NULL,
  CONSTRAINT CalcPK UNIQUE (CalcPK),
  CONSTRAINT CardToCalc
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
      ON DELETE RESTRICT
)
;

CREATE INDEX IX_CardToCalc
  ON Calculation (CardPK)
;

-- Table ProdCalc

CREATE TABLE ProdCalc
(
  Norma float,
  price int,
  summa int,
  CalcFPK INTEGER NOT NULL,
  ProductPK INTEGER NOT NULL,
  CONSTRAINT CalcToProdCalc
    FOREIGN KEY (CalcFPK)
    REFERENCES Calculation (CalcPK)
      ON DELETE CASCADE,
  CONSTRAINT ProdToProdCalc
    FOREIGN KEY (ProductPK)
    REFERENCES Product (ProductPK)
      ON DELETE RESTRICT
)
;

CREATE INDEX IX_ProdToProdCalc
  ON ProdCalc (ProductPK)
;

CREATE INDEX IX_CalcToProdCalc
  ON ProdCalc (CalcFPK)
;

