use Daftari; 

---------------------------------
---[         3 Tables        ]---
---------------------------------

CREATE TABLE PaymentMethods (
    PaymentMethodId TINYINT IDENTITY(1,1) NOT NULL,  
    PaymentMethodName NVARCHAR(50) NOT NULL,     

    PRIMARY KEY (PaymentMethodId)                
);

CREATE TABLE PaymentDates (
    PaymentDateId INT IDENTITY(1,1) NOT NULL,   
    DateOfPayment DATE DEFAULT DATEADD(DAY, 30, GETDATE()) NOT NULL,                  
    TotalAmount DECIMAL(18, 2) DEFAULT 0 NOT NULL,             
    PaymentMethodId TINYINT NOT NULL default 1 REFERENCES PaymentMethods(PaymentMethodId), -- cash
	Notes NVARCHAR(500) NULL,        
	
    PRIMARY KEY (PaymentDateId)                  
);

CREATE TABLE SupplierPaymentDates (
    SupplierPaymentDateId INT IDENTITY(1,1) NOT NULL, 
    UserId INT REFERENCES Users(UserId) NOT NULL,                              
    SupplierId INT REFERENCES Suppliers(SupplierId) NOT NULL,                          
    PaymentDateId INT REFERENCES PaymentDates(PaymentDateId) NOT NULL,  

    PRIMARY KEY (SupplierPaymentDateId),              
);

CREATE TABLE ClientPaymentDates (
    ClientPaymentDateId INT IDENTITY(1,1) NOT NULL, 
	UserId INT REFERENCES Users(UserId) NOT NULL,                              
    ClientId INT REFERENCES Clients(ClientId) NOT NULL,  
	PaymentDateId INT REFERENCES PaymentDates(PaymentDateId) NOT NULL,  

    PRIMARY KEY (ClientPaymentDateId),              
);


CREATE TABLE ClientTotalAmounts (
    ClientTotalAmountId INT IDENTITY(1,1) NOT NULL,   
    ClientId INT REFERENCES Clients(ClientId) NOT NULL, 
	UserId INT REFERENCES Users(UserId) NOT NULL,                              
    TotalAmount DECIMAL(18, 2) DEFAULT 0 NOT NULL,             
    UpdateAt DATE DEFAULT GETDATE() NOT NULL,                  
	
    PRIMARY KEY (ClientTotalAmountId)                  
);


CREATE TABLE SupplierTotalAmounts (
    SupplierTotalAmountId INT IDENTITY(1,1) NOT NULL,   
    SupplierId INT REFERENCES Suppliers(SupplierId) NOT NULL,                          
	UserId INT REFERENCES Users(UserId) NOT NULL,                              
    TotalAmount DECIMAL(18, 2) DEFAULT 0 NOT NULL,             
    UpdateAt DATE DEFAULT GETDATE() NOT NULL,                  
	
    PRIMARY KEY (SupplierTotalAmountId)                  
);

CREATE TABLE UserTotalAmounts (
    UserTotalAmountId INT IDENTITY(1,1) NOT NULL,   
	UserId INT REFERENCES Users(UserId) NOT NULL,                              
    TotalAmount DECIMAL(18, 2) DEFAULT 0 NOT NULL,             
    UpdateAt DATE DEFAULT GETDATE() NOT NULL,                  
	
    PRIMARY KEY (UserTotalAmountId)                  
);


-------------------
-- Scaffold-DbContext "Server=.;Database=Daftari;User Id=sa;Password=sa123456;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Tables Users -Force



select * from People
select * from Clients
select * from Users
select * from Suppliers



select * from Transactions
select * from SupplierTransactions
select * from ClientTransactions
select * from UserTransactions


select * from ClientTotalAmounts
select * from SupplierTotalAmounts
select * from UserTotalAmounts

select * from PaymentDates
select * from ClientPaymentDates
select * from SupplierPaymentDates



