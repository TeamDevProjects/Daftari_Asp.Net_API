
use Daftari3; 

---------------------------------
--[         1 Tables        ]--
---------------------------------

-- People
-- Users
-- Clients
-- Suppliers

CREATE TABLE People (
    PersonId INT IDENTITY(1,1) NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    Phone VARCHAR(15) NOT NULL,
    City NVARCHAR(50) NOT NULL,
    Country NVARCHAR(50) NOT NULL,
    Address NVARCHAR(100) NOT NULL,
    PRIMARY KEY (PersonId)
);

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) NOT NULL,
    PersonId INT NOT NULL REFERENCES dbo.People(PersonId),
    StoreName NVARCHAR(50) NOT NULL,
    UserName NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(250) NOT NULL,
    SectorId TINYINT NOT NULL REFERENCES Sectors(SectorId),
    RefreshToken NVARCHAR(255) DEFAULT ('') NULL,
    RefreshTokenExpiryTime DATETIME DEFAULT (GETDATE()) NULL,
    BusinessTypeId TINYINT NOT NULL REFERENCES BusinessTypes(BusinessTypeId),
    UserType NVARCHAR(50) DEFAULT ('user') NOT NULL,
    PRIMARY KEY (UserId)
);

CREATE TABLE Clients (
    ClientId INT IDENTITY(1,1) NOT NULL,
    PersonId INT NOT NULL REFERENCES dbo.People(PersonId),
    UserId INT NOT NULL REFERENCES dbo.Users(UserId),
    Notes NVARCHAR(500) NOT NULL,
    PRIMARY KEY (ClientId)
);

CREATE TABLE Suppliers (
    SupplierId INT IDENTITY(1,1) NOT NULL,
    PersonId INT NOT NULL REFERENCES dbo.People(PersonId),
    UserId INT NOT NULL REFERENCES dbo.Users(UserId),
    Notes NVARCHAR(500) NOT NULL,
    PRIMARY KEY (SupplierId)
);

