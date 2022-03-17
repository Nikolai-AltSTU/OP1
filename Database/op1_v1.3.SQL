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
        CONSTRAINT PK_Card PRIMARY KEY AUTOINCREMENT,
  OrganizationName TEXT,
  SubOrganization TEXT,
  DishName TEXT,
  OKUD TEXT,
  OKPO TEXT,
  OKDP TEXT,
  DishNumber TEXT,
  OperName TEXT,
  DocNumber TEXT,
  DateOfDoc datetime
)
;

-- Table Product

CREATE TABLE Product
(
  ProductPK INTEGER NOT NULL
        CONSTRAINT PK_Product PRIMARY KEY AUTOINCREMENT,
  NameProd TEXT,
  Code int,
  Number int,
  CardPK INTEGER,
  CONSTRAINT Relationship3
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
)
;

CREATE INDEX IX_Relationship3
  ON Product (CardPK)
;

-- Table Calculation

CREATE TABLE Calculation
(
  CalcPK INTEGER NOT NULL
        CONSTRAINT PK_Calculation PRIMARY KEY AUTOINCREMENT,
  numberCalc int,
  dateCalc datetime,
  DishWeght float,
  ExtraChargePercent float,
  ExtraChargeMoney int,
  Zaveduushiy TEXT,
  Sostavitel TEXT,
  Rukovoditel TEXT,
  CardPK INTEGER NOT NULL,
  CONSTRAINT Relationship1
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
)
;

CREATE INDEX IX_Relationship1
  ON Calculation (CardPK)
;

-- Table ProdCalc

CREATE TABLE ProdCalc
(
  Norma float,
  price int,
  summa int,
  ProdCalsPK INTEGER NOT NULL
        CONSTRAINT PK_ProdCalc PRIMARY KEY AUTOINCREMENT,
  CalcPK INTEGER NOT NULL,
  ProductPK INTEGER,
  CONSTRAINT Relationship2
    FOREIGN KEY (CalcPK)
    REFERENCES Calculation (CalcPK),
  CONSTRAINT Relationship4
    FOREIGN KEY (ProductPK)
    REFERENCES Product (ProductPK)
)
;

CREATE INDEX IX_Relationship4
  ON ProdCalc (ProductPK)
;

CREATE INDEX IX_Relationship2
  ON ProdCalc (CalcPK)
;

