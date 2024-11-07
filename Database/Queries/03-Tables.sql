

use Daftari2; 

---------------------------------
--[         3 Tables        ]--
---------------------------------

CREATE TABLE PaymentMethods (
    PaymentMethodId TINYINT IDENTITY(1,1) NOT NULL,  
    PaymentMethodName NVARCHAR(50) NOT NULL,     

    PRIMARY KEY (PaymentMethodId)                
);

CREATE TABLE PaymentDates (
    PaymentDateId INT IDENTITY(1,1) NOT NULL,   
    PaymentDate DATE DEFAULT DATEADD(DAY, 30, GETDATE()) NOT NULL,                  
    Amount DECIMAL(18, 2) DEFAULT 0 NOT NULL,             
    PaymentMethod NVARCHAR(50) default 1 NOT NULL,         
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

-------------------
-- Scaffold-DbContext "Server=.;Database=Daftari;User Id=sa;Password=sa123456;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entities -Tables Users -Force


delete from People

select * from People
select * from Users