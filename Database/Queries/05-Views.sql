-----------------------------------------
-----------------[ Views ]---------------
-----------------------------------------
create view SectorsView as
SELECT Sectors.SectorId, Sectors.SectorName,SectorTypes.SectorTypeName
FROM   Sectors INNER JOIN
       SectorTypes ON Sectors.SectorTypeId = SectorTypes.SectorTypeId 

-- UsersView => [ User, People,  Sectors,  SectorTypes,  BusinessTypes,  UserTotalAmounts ]
CREATE VIEW UsersView AS 
SELECT  Users.UserId, People.Name, People.Phone, People.City, People.Country, People.Address, Users.StoreName, Users.UserName,
		Sectors.SectorName, SectorTypes.SectorTypeName, BusinessTypes.BusinessTypeName, 
        Users.UserType, UserTotalAmounts.TotalAmount
FROM    Users INNER JOIN
        UserTotalAmounts ON Users.UserId = UserTotalAmounts.UserId AND Users.UserId = UserTotalAmounts.UserId INNER JOIN
        People ON Users.PersonId = People.PersonId INNER JOIN
        Sectors ON Users.SectorId = Sectors.SectorId INNER JOIN
        SectorTypes ON Sectors.SectorTypeId = SectorTypes.SectorTypeId INNER JOIN
        BusinessTypes ON Users.BusinessTypeId = BusinessTypes.BusinessTypeId

-- ClientView
Alter VIEW ClientsView AS
SELECT  Clients.ClientId, People.Name, People.Phone, People.City, People.Country, People.Address,
ClientTotalAmounts.TotalAmount, PaymentDates.DateOfPayment, PaymentMethods.PaymentMethodName
FROM    Clients INNER JOIN
        People ON Clients.PersonId = People.PersonId INNER JOIN
        ClientTotalAmounts ON Clients.ClientId = ClientTotalAmounts.ClientId AND Clients.ClientId = ClientTotalAmounts.ClientId INNER JOIN
        ClientPaymentDates ON Clients.ClientId = ClientPaymentDates.ClientId AND Clients.ClientId = ClientPaymentDates.ClientId INNER JOIN
        PaymentDates ON ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId INNER JOIN
		PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId

-- SupplierViews
CREATE VIEW SuppliersView AS
SELECT  Suppliers.SupplierId, People.Name, People.Phone, People.City, People.Country, People.Address,
		Suppliers.Notes, SupplierTotalAmounts.TotalAmount, PaymentDates.DateOfPayment, PaymentMethods.PaymentMethodName
FROM    Suppliers INNER JOIN
        People ON Suppliers.PersonId = People.PersonId INNER JOIN
        SupplierTotalAmounts ON Suppliers.SupplierId = SupplierTotalAmounts.SupplierId AND Suppliers.SupplierId = SupplierTotalAmounts.SupplierId INNER JOIN
        SupplierPaymentDates ON Suppliers.SupplierId = SupplierPaymentDates.SupplierId AND Suppliers.SupplierId = SupplierPaymentDates.SupplierId INNER JOIN
        PaymentDates ON SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId INNER JOIN
        PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId

-- UserTransactionsView
CREATE VIEW UserTransactionsView AS
SELECT  UserTransactions.UserTransactionId, UserTransactions.UserId, TransactionTypes.TransactionTypeName,
		Transactions.Notes, Transactions.TransactionDate, Transactions.Amount, Transactions.ImageData, Transactions.ImageType
FROM    UserTransactions INNER JOIN
        Transactions ON UserTransactions.TransactionId = Transactions.TransactionId AND UserTransactions.TransactionId = Transactions.TransactionId INNER JOIN
        TransactionTypes ON Transactions.TransactionTypeId = TransactionTypes.TransactionTypeId

-- ClientsTransactionsView
CREATE VIEW ClientsTransactionsView AS
SELECT  ClientTransactions.ClientTransactionId, ClientTransactions.UserId, ClientTransactions.ClientId, Transactions.Notes,
		Transactions.TransactionDate, Transactions.Amount, TransactionTypes.TransactionTypeName, Transactions.ImageData, Transactions.ImageType
FROM    ClientTransactions INNER JOIN
        Transactions ON ClientTransactions.TransactionId = Transactions.TransactionId AND ClientTransactions.TransactionId = Transactions.TransactionId INNER JOIN
        TransactionTypes ON Transactions.TransactionTypeId = TransactionTypes.TransactionTypeId

-- SuppliersTransactionsView
CREATE VIEW SuppliersTransactionsView AS
SELECT  SupplierTransactions.SupplierTransactionId, Transactions.Notes, Transactions.TransactionDate, Transactions.Amount, TransactionTypes.TransactionTypeName,
		Transactions.ImageData, Transactions.ImageType, SupplierTransactions.UserId, SupplierTransactions.SupplierId
FROM    SupplierTransactions INNER JOIN
        Transactions ON SupplierTransactions.TransactionId = Transactions.TransactionId AND SupplierTransactions.TransactionId = Transactions.TransactionId INNER JOIN
        TransactionTypes ON Transactions.TransactionTypeId = TransactionTypes.TransactionTypeId

-- ClientPaymentDateView
CREATE VIEW ClientsPaymentDateView AS
SELECT  ClientPaymentDates.ClientPaymentDateId, ClientPaymentDates.UserId, People.Name, PaymentDates.DateOfPayment,
		PaymentMethods.PaymentMethodName, PaymentDates.Notes, ClientPaymentDates.ClientId, People.Phone
FROM    ClientPaymentDates INNER JOIN
        PaymentDates ON ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId INNER JOIN
        PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId INNER JOIN
        Clients ON ClientPaymentDates.ClientId = Clients.ClientId AND ClientPaymentDates.ClientId = Clients.ClientId INNER JOIN
        People ON Clients.PersonId = People.PersonId

-- SuppliersPaymentDateView
CREATE VIEW SuppliersPaymentDateView AS
SELECT  SupplierPaymentDates.SupplierPaymentDateId, SupplierPaymentDates.UserId ,SupplierPaymentDates.SupplierId, PaymentDates.DateOfPayment, PaymentDates.Notes,
		People.Name, People.Phone, PaymentMethods.PaymentMethodName
FROM    SupplierPaymentDates INNER JOIN
        PaymentDates ON SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId INNER JOIN
	    PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId INNER JOIN
	    Suppliers ON SupplierPaymentDates.SupplierId = Suppliers.SupplierId AND SupplierPaymentDates.SupplierId = Suppliers.SupplierId INNER JOIN
	    People ON Suppliers.PersonId = People.PersonId