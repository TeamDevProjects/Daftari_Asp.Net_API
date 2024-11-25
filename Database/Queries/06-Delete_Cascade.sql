
-------------------------------------------------------
---------------------------------------------------------------
-----------------------------------------------------------------------

-- تعديل العلاقة بين ClientTransactions و Transactions
ALTER TABLE ClientTransactions
ADD CONSTRAINT FK_ClientTransactions_Transactions
FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId)
ON DELETE CASCADE;

-- تعديل العلاقة بين ClientTotalAmounts و Clients
ALTER TABLE ClientTotalAmounts
ADD CONSTRAINT FK_ClientTotalAmounts_Clients
FOREIGN KEY (ClientId) REFERENCES Clients(ClientId)
ON DELETE CASCADE;

-- تعديل العلاقة بين ClientPaymentDates و Clients
ALTER TABLE ClientPaymentDates
ADD CONSTRAINT FK_ClientPaymentDates_Clients
FOREIGN KEY (ClientId) REFERENCES Clients(ClientId)
ON DELETE CASCADE;


-- تعديل العلاقة بين ClientTransactions و Clients
ALTER TABLE ClientTransactions
ADD CONSTRAINT FK_ClientTransactions_Clients
FOREIGN KEY (ClientId) REFERENCES Clients(ClientId)
ON DELETE CASCADE;

-- تعديل العلاقة بين ClientPaymentDates و PaymentDates
ALTER TABLE ClientPaymentDates
ADD CONSTRAINT FK_ClientPaymentDates_PaymentDates
FOREIGN KEY (PaymentDateId) REFERENCES PaymentDates(PaymentDateId)
ON DELETE CASCADE;

-- تعديل العلاقة بين SupplierPaymentDates و Suppliers
ALTER TABLE SupplierPaymentDates
ADD CONSTRAINT FK_SupplierPaymentDates_Suppliers
FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
ON DELETE CASCADE;

-- تعديل العلاقة بين UserTransactions و Users
ALTER TABLE UserTransactions
ADD CONSTRAINT FK_UserTransactions_Users
FOREIGN KEY (UserId) REFERENCES Users(UserId)
ON DELETE CASCADE;


-- تعديل العلاقة بين UserTotalAmounts و Users
ALTER TABLE UserTotalAmounts
ADD CONSTRAINT FK_UserTotalAmounts_Users
FOREIGN KEY (UserId) REFERENCES Users(UserId)
ON DELETE CASCADE;

-- تعديل العلاقة بين Clients و Users
ALTER TABLE Clients
ADD CONSTRAINT FK_Clients_Users
FOREIGN KEY (UserId) REFERENCES Users(UserId)
ON DELETE CASCADE;

-- تعديل العلاقة بين SupplierTotalAmounts و Suppliers
ALTER TABLE SupplierTotalAmounts
ADD CONSTRAINT FK_SupplierTotalAmounts_Suppliers
FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
ON DELETE CASCADE;

-- تعديل العلاقة بين SupplierTransactions و Suppliers
ALTER TABLE SupplierTransactions
ADD CONSTRAINT FK_SupplierTransactions_Suppliers
FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
ON DELETE CASCADE;

-- تعديل العلاقة بين SupplierTransactions و Transactions
ALTER TABLE SupplierTransactions
ADD CONSTRAINT FK_SupplierTransactions_Transactions
FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId)
ON DELETE CASCADE;

-- تعديل العلاقة بين SupplierPaymentDates و PaymentDates
ALTER TABLE SupplierPaymentDates
ADD CONSTRAINT FK_SupplierPaymentDates_PaymentDates
FOREIGN KEY (PaymentDateId) REFERENCES PaymentDates(PaymentDateId)
ON DELETE CASCADE;

-- تعديل العلاقة بين UserTransactions و Transactions
ALTER TABLE UserTransactions
ADD CONSTRAINT FK_UserTransactions_Transactions
FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId)
ON DELETE CASCADE;

-------------------------------------------------------
---------------------------------------------------------------
-----------------------------------------------------------------------

-- تعديل العلاقة بين SupplierTransactions و Transactions
ALTER TABLE SupplierTransactions
ADD CONSTRAINT FK_SupplierTransactions_Transactions
FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId)
ON DELETE CASCADE;

