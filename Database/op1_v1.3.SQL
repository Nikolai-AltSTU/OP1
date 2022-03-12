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
        CONSTRAINT PK_Card PRIMARY KEY,
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
        CONSTRAINT PK_Product PRIMARY KEY,
  NameProd TEXT,
  Code int,
  Number int,
  CardPK INTEGER,
  CONSTRAINT Relationship3
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
      ON DELETE RESTRICT
)
;

CREATE INDEX IX_Relationship3
  ON Product (CardPK)
;

-- Table Calculation

CREATE TABLE Calculation
(
  CalcPK INTEGER NOT NULL
        CONSTRAINT PK_Calculation PRIMARY KEY,
  numberCalc int,
  dateCalc datetime,
  DishWeght float,
  ExtraChargePercent float,
  ExtraChargeMoney int,
  Zaveduushiy TEXT,
  Sostavitel TEXT,
  Rukovoditel TEXT,
  CardPK INTEGER NOT NULL,
  --CONSTRAINT PK_Calculation PRIMARY KEY (CalcPK,CardPK),
  CONSTRAINT Relationship1
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
      ON DELETE CASCADE
)
;

-- Table ProdCalc

CREATE TABLE ProdCalc
(
  Norma float,
  price int,
  summa int,
  ProdCalsPK INTEGER NOT NULL
        CONSTRAINT PK_ProdCalc PRIMARY KEY,
  CalcPK INTEGER NOT NULL,
  CardPK INTEGER NOT NULL,
  ProductPK INTEGER,
  --CONSTRAINT PK_ProdCalc PRIMARY KEY (ProdCalsPK,CalcPK,CardPK),
  CONSTRAINT Relationship2
    FOREIGN KEY (CalcPK, CardPK)
    REFERENCES Calculation (CalcPK, CardPK)
      ON DELETE CASCADE,
  CONSTRAINT Relationship4
    FOREIGN KEY (ProductPK)
    REFERENCES Product (ProductPK)
      ON DELETE RESTRICT
)
;

CREATE INDEX IX_Relationship4
  ON ProdCalc (ProductPK)
;

