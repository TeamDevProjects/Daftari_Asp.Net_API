------------------------------------------------
--------------[   Indexing   ]------------------
------------------------------------------------
-- CREATE INDEX idx_person_phone ON Users(UserName, Country);

-- 14 -> 19
-- People
CREATE INDEX idx_phone ON People(Phone);

-- Users
CREATE INDEX idx_username ON Users(UserName);
CREATE INDEX idx_password ON Users(PasswordHash);

-- UserTotalAmounts
CREATE INDEX idx_paymentdate ON PaymentDates(DateOfPayment);

-- UserTransactions
CREATE INDEX idx_user_transactionid ON UserTransactions(TransactionId);

-- ClientTransactions
CREATE INDEX idx_client_transactionid ON ClientTransactions(TransactionId);

-- SupplierTransactions
CREATE INDEX idx_supplier_transactionid ON SupplierTransactions(TransactionId);

-- SupplierTotalAmounts
CREATE INDEX idx_suppliertotal_amounts ON SupplierTotalAmounts(TotalAmount);

-- ClientTotalAmounts
CREATE INDEX idx_clienttotal_amounts ON ClientTotalAmounts(TotalAmount);

-- Clients
-- Suppliers
-- PaymentDates
-- ClientPaymentDates
-- SupplierPaymentDates
-- Transactions


---------------------------
-- add it to phone property
-- .IsUnique(true);