


use Daftari; 

-- select * from People
-- select * from Users
-- select * from Clients
-- select * from Suppliers

---------------------------------
--[         New Tables        ]--
---------------------------------
--> For User
--  BusinessTypes  انواع الانشطة   
--  SectorTypes          انواع القطاعات  
--  Sectors            القطاعات  

-- TransactionTypes
-- Transactions
-- UserTransactions
-- ClientTransactions
-- SupplierTransactions

Create Table BusinessTypes(
	BusinessTypeId int identity(1,1) not null,
	BusinessTypeName nvarchar(50) not null,

	primary key(BusinessTypeId)
)



Create Table SectorTypes(
	SectorTypeId int identity(1,1) not null,
	SectorTypeName nvarchar(50) not null,

	primary key(SectorTypeId)
)


Create Table Sectors(
	SectorId int identity(1,1) not null,
	SectorTypeId int references SectorTypes(SectorTypeId),
	SectorName nvarchar(50) not null,

	primary key(SectorId)
)


--------------------------------------------


Create Table TransactionTypes (
	TransactionTypeId tinyint identity(1,1) not null, --  tinyint [ 0 to 255 ]
	TransactionTypeName nvarchar(50) not null,

	primary key(TransactionTypeId)
)


Create Table Transactions (
	TransactionId int identity(1,1) not null,
	TransactionTypeId tinyint references TransactionTypes(TransactionTypeId),
	Notes nvarchar(500) NULL,
	TransactionDate datetime default GETDATE() not null,
	Amount decimal not null,
	ImageData VarBinary(MAX) NOT NULL,
	ImageType  NVARCHAR(10) Null CHECK (ImageType IN ('image/jpeg', 'image/png',NULL)),

	primary key(TransactionId)
)


-- UserTransactions
Create Table UserTransactions(
	UserTransactionId int identity(1,1) not null,
	TransactionId int references Transactions(TransactionId) not null,
	UserId int references Users(UserId) not null,
	TotalAmount decimal default 0 not null,

	primary key(UserTransactionId)
)


-- ClientTransactions
Create Table ClientTransactions(
	ClientTransactionId int identity(1,1) not null,
	TransactionId int references Transactions(TransactionId) not null,
	UserId int references Users(UserId) not null,
	ClientId int references Clients(ClientId) not null,
	TotalAmount decimal default 0 not null,

	primary key(ClientTransactionId)
)


-- SupplierTransactions
Create Table SupplierTransactions(
	SupplierTransactionId int identity(1,1) not null,
	TransactionId int references Transactions(TransactionId) not null,
	UserId int references Users(UserId) not null,
	SupplierId int references Suppliers(SupplierId) not null,
	TotalAmount decimal default 0 not null,

	primary key(SupplierTransactionId)
)

---------------------------------
--[       Updated Tables      ]--
---------------------------------

-- Users
Alter table Users add  PasswordHash NVARCHAR(250) UNIQUE NOT NULL;
Alter table Users add  UserName NVARCHAR(50) UNIQUE NOT NULL;

ALTER TABLE Users 
ADD BusinessTypeId INT NOT NULL 
REFERENCES BusinessTypes(BusinessTypeId);

ALTER TABLE Users 
ADD SectorId INT NOT NULL 
REFERENCES Sectors(SectorId);
---------------------------------
--[        Insert Data        ]--
---------------------------------

-- in TransactionTypes
INSERT INTO TransactionTypes (TransactionTypeName)
VALUES 
    ('Payment'),    --  دفع , اعطيت  
    ('Withdrawal')  --  سحب ,  اخذت   


-- in BusinessTypes
INSERT INTO BusinessTypes VALUES
    (N'Retail Sale'),
    (N'Wholesale Sale'),
    (N'Distribution'),
    (N'Manufacturing'),
    (N'Services');

-- in SectorTypes
INSERT INTO SectorTypes VALUES
    (N'Mobiles and Technology'),
    (N'Products and Goods'),
    (N'Construction'),
    (N'Fashion and Personal Care'),
    (N'Food and Beverages'),
    (N'Services');
	select * from SectorTypes
select * from SectorTypes
--in Sectors
INSERT INTO Sectors (SectorTypeId, SectorName) VALUES
    (1, N'Electronics'),
    (1, N'Mobile Recharge, Internet and Financial Services'),
    
    (2, N'Groceries'),
    (2, N'Furnishing and Decoration'),
    (2, N'Car Parts'),
    
    (2, N'Library'),
    (2, N'Agricultural Products'),
    (2, N'Pharmacy and Medical Supplies'),
    (2, N'Optical Retail'),
    (2, N'Agriculture'),                           
	(2, N'Other Products'),
    
    (3, N'Construction Services'),                 
    (3, N'Building Materials'),
    (3, N'Hardware & Tools Store'),

    (4, N'Clothing and Fashion'),
    (4, N'Cosmetics'),
	(4, N'Golden'),
    (4, N'Tailoring'),
    (4, N'Beauty and Hair Services'),
    
    (5, N'Cafe'),
    (5, N'Restaurants'),
    (5, N'Butcher'),
    (5, N'Fruit and Vegetable Store'),
    (5, N'Bakery'),
    (5, N'Fishing'),
    (5, N'Dairy Store'),
    (5, N'Party Caterer'),
    
    (6, N'Professional Services'),
    (6, N'Transportation and Logistics'),
    (6, N'Real Estate'),
    (6, N'Medical Services'),
    (6, N'Car Rental and Repair'),
    (6, N'Insurance');




-------------
-- Notes
-------------
-- [1]
-- [ use Stored Procedure ]
-- and trigger after insert 
-- add User     =>  User & Person
-- add Client   =>  Client & Person
-- add Supplier =>  Supplier & Person

-- and the same thing for Transactions ....

-- [2]
-- use Indexing 
-- use Views
-- custom function

