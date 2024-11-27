
-- after clear tabels 
-- reset identity to start from 1 again

DBCC CHECKIDENT (People, RESEED, 0); 
DBCC CHECKIDENT ( Users, RESEED, 0);
DBCC CHECKIDENT ( Clients, RESEED, 0);
DBCC CHECKIDENT ( Suppliers, RESEED, 0);
DBCC CHECKIDENT ( PaymentDates, RESEED, 0);
DBCC CHECKIDENT ( ClientPaymentDates, RESEED, 0);
DBCC CHECKIDENT (  SupplierPaymentDates, RESEED, 0);
DBCC CHECKIDENT ( Transactions, RESEED, 0);
DBCC CHECKIDENT ( UserTransactions, RESEED, 0);
DBCC CHECKIDENT ( ClientTransactions, RESEED, 0);
DBCC CHECKIDENT ( SupplierTransactions, RESEED, 0);
DBCC CHECKIDENT ( UserTotalAmounts, RESEED, 0);
DBCC CHECKIDENT ( SupplierTotalAmounts, RESEED, 0);
DBCC CHECKIDENT ( ClientTotalAmounts, RESEED, 0);