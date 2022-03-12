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
  Code INTEGER,
  Number INTEGER,
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
  numberCalc INTEGER,
  dateCalc datetime,
  DishWeght float,
  ExtraChargePercent float,
  ExtraChargeMoney INTEGER,
  Zaveduushiy TEXT,
  Sostavitel TEXT,
  Rukovoditel TEXT,
  CardPK INTEGER NOT NULL,
  --CONSTRAINT PK_Calculation PRIMARY KEY (CalcPK,CardPK),
  CONSTRAINT CalcPK UNIQUE (CalcPK),
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
  price INTEGER,
  summa INTEGER,
  CalcFPK INTEGER NOT NULL,
  CardFPK INTEGER NOT NULL,
  ProductPK INTEGER NOT NULL,
  CONSTRAINT PK_ProdCalc PRIMARY KEY (CalcFPK,CardFPK,ProductPK),
  CONSTRAINT CalcToProdCalc
    FOREIGN KEY (CalcFPK, CardFPK)
    REFERENCES Calculation (CalcPK, CardPK)
      ON DELETE CASCADE,
  CONSTRAINT ProdToProdCalc
    FOREIGN KEY (ProductPK)
    REFERENCES Product (ProductPK)
      ON DELETE CASCADE
)
;

