/*
Created: 10.03.2022
Modified: 10.03.2022
Model: SQLite 3.32
Database: SQLite 3.32
*/

-- Create tables section -------------------------------------------------

-- Table Card

CREATE TABLE Card
(
  CardPK bigint NOT NULL,
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
  CONSTRAINT PK_Card PRIMARY KEY (CardPK)
)
;

-- Table Product

CREATE TABLE Product
(
  ProductPK bigint NOT NULL,
  NameProd TEXT,
  Code int,
  Number int,
  CardPK bigint NOT NULL,
  CONSTRAINT PK_Product PRIMARY KEY (ProductPK,CardPK),
  CONSTRAINT CardToProduct
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
      ON DELETE RESTRICT
)
;

-- Table Calculation

CREATE TABLE Calculation
(
  CalcPK bigint NOT NULL,
  numberCalc int,
  dateCalc datetime,
  DishWeght float,
  ExtraChargePercent float,
  ExtraChargeMoney NUMERIC,
  Zaveduushiy TEXT,
  Sostavitel TEXT,
  Rukovoditel TEXT,
  CardPK bigint NOT NULL,
  CONSTRAINT PK_Calculation PRIMARY KEY (CalcPK,CardPK),
  CONSTRAINT CardToCalc
    FOREIGN KEY (CardPK)
    REFERENCES Card (CardPK)
      ON DELETE RESTRICT
)
;

-- Table ProdCalc

CREATE TABLE ProdCalc
(
  Norma float,
  price NUMERIC,
  summa NUMERIC,
  CalcFPK bigint NOT NULL,
  CardFPK bigint NOT NULL,
  ProductPK bigint,
  CardPK bigint,
  CONSTRAINT PK_ProdCalc PRIMARY KEY (CalcFPK,CardFPK),
  CONSTRAINT CalcToProdCalc
    FOREIGN KEY (CalcFPK, CardFPK)
    REFERENCES Calculation (CalcPK, CardPK)
      ON DELETE CASCADE,
  CONSTRAINT ProdToProdCalc
    FOREIGN KEY (ProductPK, CardPK)
    REFERENCES Product (ProductPK, CardPK)
      ON DELETE RESTRICT
)
;

CREATE INDEX IX_Relationship4
  ON ProdCalc (ProductPK,CardPK)
;

