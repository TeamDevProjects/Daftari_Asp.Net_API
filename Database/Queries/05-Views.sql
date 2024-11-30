-----------------------------------------
-----------------[ Views ]---------------
-----------------------------------------
ALTER view SectorsView as
SELECT Sectors.SectorId, Sectors.SectorName,SectorTypes.SectorTypeName
FROM   Sectors LEFT JOIN
       SectorTypes ON Sectors.SectorTypeId = SectorTypes.SectorTypeId 

-- UsersView => [ User, People,  Sectors,  SectorTypes,  BusinessTypes,  UserTotalAmounts ]
ALTER VIEW UsersView AS 
SELECT  Users.UserId, People.Name, People.Phone, People.City, People.Country, People.Address, Users.StoreName, Users.UserName,
		Sectors.SectorName, SectorTypes.SectorTypeName, BusinessTypes.BusinessTypeName, 
        Users.UserType, UserTotalAmounts.TotalAmount
FROM    Users LEFT JOIN
        UserTotalAmounts ON Users.UserId = UserTotalAmounts.UserId AND Users.UserId = UserTotalAmounts.UserId LEFT JOIN
        People ON Users.PersonId = People.PersonId LEFT JOIN
        Sectors ON Users.SectorId = Sectors.SectorId LEFT JOIN
        SectorTypes ON Sectors.SectorTypeId = SectorTypes.SectorTypeId LEFT JOIN
        BusinessTypes ON Users.BusinessTypeId = BusinessTypes.BusinessTypeId

-- ClientView
Alter VIEW ClientsView AS
SELECT  Clients.ClientId, Clients.UserId,People.Name, People.Phone, People.City, People.Country, People.Address,
ClientTotalAmounts.TotalAmount, PaymentDates.DateOfPayment, PaymentMethods.PaymentMethodName
FROM    Clients LEFT JOIN
        People ON Clients.PersonId = People.PersonId LEFT JOIN
        ClientTotalAmounts ON Clients.ClientId = ClientTotalAmounts.ClientId AND Clients.ClientId = ClientTotalAmounts.ClientId LEFT JOIN
        ClientPaymentDates ON Clients.ClientId = ClientPaymentDates.ClientId AND Clients.ClientId = ClientPaymentDates.ClientId LEFT JOIN
        PaymentDates ON ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId LEFT JOIN
		PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId

-- SupplierViews
ALTER VIEW SuppliersView AS
SELECT  Suppliers.SupplierId,Suppliers.UserId, People.Name, People.Phone, People.City, People.Country, People.Address,
		Suppliers.Notes, SupplierTotalAmounts.TotalAmount, PaymentDates.DateOfPayment, PaymentMethods.PaymentMethodName
FROM    Suppliers LEFT JOIN
        People ON Suppliers.PersonId = People.PersonId LEFT JOIN
        SupplierTotalAmounts ON Suppliers.SupplierId = SupplierTotalAmounts.SupplierId AND Suppliers.SupplierId = SupplierTotalAmounts.SupplierId LEFT JOIN
        SupplierPaymentDates ON Suppliers.SupplierId = SupplierPaymentDates.SupplierId AND Suppliers.SupplierId = SupplierPaymentDates.SupplierId LEFT JOIN
        PaymentDates ON SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId LEFT JOIN
        PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId

-- UserTransactionsView
ALTER VIEW UserTransactionsView AS
SELECT  UserTransactions.UserTransactionId, UserTransactions.UserId, TransactionTypes.TransactionTypeName,
		Transactions.Notes, Transactions.TransactionDate, Transactions.Amount, Transactions.ImageData, Transactions.ImageType
FROM    UserTransactions LEFT JOIN
        Transactions ON UserTransactions.TransactionId = Transactions.TransactionId AND UserTransactions.TransactionId = Transactions.TransactionId LEFT JOIN
        TransactionTypes ON Transactions.TransactionTypeId = TransactionTypes.TransactionTypeId

-- ClientsTransactionsView
ALTER VIEW ClientsTransactionsView AS
SELECT  ClientTransactions.ClientTransactionId, ClientTransactions.UserId, ClientTransactions.ClientId, Transactions.Notes,
		Transactions.TransactionDate, Transactions.Amount, TransactionTypes.TransactionTypeName, Transactions.ImageData, Transactions.ImageType
FROM    ClientTransactions LEFT JOIN
        Transactions ON ClientTransactions.TransactionId = Transactions.TransactionId AND ClientTransactions.TransactionId = Transactions.TransactionId LEFT JOIN
        TransactionTypes ON Transactions.TransactionTypeId = TransactionTypes.TransactionTypeId

-- SuppliersTransactionsView
ALTER VIEW SuppliersTransactionsView AS
SELECT  SupplierTransactions.SupplierTransactionId, Transactions.Notes, Transactions.TransactionDate, Transactions.Amount, TransactionTypes.TransactionTypeName,
		Transactions.ImageData, Transactions.ImageType, SupplierTransactions.UserId, SupplierTransactions.SupplierId
FROM    SupplierTransactions LEFT JOIN
        Transactions ON SupplierTransactions.TransactionId = Transactions.TransactionId AND SupplierTransactions.TransactionId = Transactions.TransactionId LEFT JOIN
        TransactionTypes ON Transactions.TransactionTypeId = TransactionTypes.TransactionTypeId

-- ClientPaymentDateView
ALTER VIEW ClientsPaymentDateView AS
SELECT  ClientPaymentDates.ClientPaymentDateId, ClientPaymentDates.UserId, People.Name, PaymentDates.DateOfPayment,
		PaymentMethods.PaymentMethodName, PaymentDates.Notes, ClientPaymentDates.ClientId, People.Phone
FROM    ClientPaymentDates LEFT JOIN
        PaymentDates ON ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND ClientPaymentDates.PaymentDateId = PaymentDates.PaymentDateId LEFT JOIN
        PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId LEFT JOIN
        Clients ON ClientPaymentDates.ClientId = Clients.ClientId AND ClientPaymentDates.ClientId = Clients.ClientId LEFT JOIN
        People ON Clients.PersonId = People.PersonId

-- SuppliersPaymentDateView
ALTER VIEW SuppliersPaymentDateView AS
SELECT  SupplierPaymentDates.SupplierPaymentDateId, SupplierPaymentDates.UserId ,SupplierPaymentDates.SupplierId, PaymentDates.DateOfPayment, PaymentDates.Notes,
		People.Name, People.Phone, PaymentMethods.PaymentMethodName
FROM    SupplierPaymentDates LEFT JOIN
        PaymentDates ON SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId AND SupplierPaymentDates.PaymentDateId = PaymentDates.PaymentDateId LEFT JOIN
	    PaymentMethods ON PaymentDates.PaymentMethodId = PaymentMethods.PaymentMethodId LEFT JOIN
	    Suppliers ON SupplierPaymentDates.SupplierId = Suppliers.SupplierId AND SupplierPaymentDates.SupplierId = Suppliers.SupplierId LEFT JOIN
	    People ON Suppliers.PersonId = People.PersonId


