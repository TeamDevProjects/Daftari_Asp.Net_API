


use Daftari; 

---------------------------------
--[         2 Tables        ]--
---------------------------------
--  BusinessTypes     
--  SectorTypes  
--  Sectors              

-- TransactionTypes
-- Transactions
-- UserTransactions
-- ClientTransactions
-- SupplierTransactions

Create Table BusinessTypes(
	BusinessTypeId tinyint identity(1,1) not null,
	BusinessTypeName nvarchar(50) not null,

	primary key(BusinessTypeId)
)


Create Table SectorTypes(
	SectorTypeId tinyint identity(1,1) not null,
	SectorTypeName nvarchar(50) not null,

	primary key(SectorTypeId)
)


Create Table Sectors(
	SectorId tinyint identity(1,1) not null,
	SectorTypeId tinyint references SectorTypes(SectorTypeId),
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
	ImageData VarBinary(MAX)  NULL,
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

select * from Transactions


BACKUP DATABASE Daftari3
TO DISK = 'E:\Backup\Daftari3.bak'