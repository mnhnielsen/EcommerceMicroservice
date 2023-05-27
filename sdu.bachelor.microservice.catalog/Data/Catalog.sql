CREATE TABLE Brands(
    Id VARCHAR(36) PRIMARY KEY,
    Title VARCHAR(150) NOT NULL
);

INSERT INTO Brands VALUES("cd3eb3f1-0143-495b-9b90-9d1e8e46fbad", "Trek");
INSERT INTO Brands VALUES("e29de237-8203-4e3e-8066-4ac71d2c707f", "Factor");
INSERT INTO Brands VALUES("e57ed7c0-4cc5-4d12-a88b-ed9f2997d918", "Colnago");




CREATE TABLE Products(
    Id VARCHAR(36) PRIMARY KEY,
    BrandId VARCHAR(36) NOT NULL,
    Description VARCHAR (2000),
    Price DOUBLE NOT NULL,
    Stock INT NOT NULL,
    Title VARCHAR(150) NOT NULL,
    FOREIGN KEY (BrandId) REFERENCES Brands(Id)
);

INSERT INTO Products VALUES("7201fd50-25b9-4b7d-99a7-b367b73222f8", "cd3eb3f1-0143-495b-9b90-9d1e8e46fbad", "High-End aero bike for the flats", 10000.0, 2147483647, "Madone");
INSERT INTO Products VALUES("ab0f5a1f-9b48-4862-8e6a-bced8d20558e", "e29de237-8203-4e3e-8066-4ac71d2c707f", "For the mountains", 11000.0, 2147483647, "Vam");
INSERT INTO Products VALUES("d4b1d999-862d-4cf9-bcb7-b79de08768b9", "e57ed7c0-4cc5-4d12-a88b-ed9f2997d918", "Made for winning", 12000.0, 2147483647, "V4Rs");

