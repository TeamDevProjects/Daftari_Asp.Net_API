
--error to can not be able to add this data again



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

INSERT INTO PaymentMethods(PaymentMethodName)
VALUES
    ('Cash'),
    ('Credit Card'),
    ('Bank Transfer'),
    ('PayPal'),
    ('Cheque'),
    ('Wire Transfer');

