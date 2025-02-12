CREATE TABLE IF NOT EXISTS TankinfoModel (
    Id SERIAL PRIMARY KEY,
    Date TIMESTAMP NOT NULL,
    FuelAmount DECIMAL(10, 2),
    FuelPrice1 DECIMAL(10, 2),
    FuelPrice2 DECIMAL(10, 2),
    FuelType VARCHAR(50),
    NameGasStation1 VARCHAR(100),
    NameGasStation2 VARCHAR(100)
);
