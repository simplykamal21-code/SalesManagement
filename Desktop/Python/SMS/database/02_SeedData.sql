/*
    Sales Management System - Seed Data
    Run after 01_CreateDatabase.sql
*/
USE SalesManagementDB;
GO

-- Default admin user. Password is "Admin@123" hashed with BCrypt.
-- (Generated with BCrypt.Net-Next, work factor 11 - see README for how to regenerate)
INSERT INTO dbo.UserMaster (Name, Username, Mobile, PasswordHash)
VALUES ('System Admin', 'admin', '0501234567', '$2a$11$wUZ9m1yQe1V8n2qk7Hn0eu6f6ZC1Xk8LxWc2u1Rr7hUvz2oQxwq0e');

-- Customers
INSERT INTO dbo.CustomerMaster (Code, Name, Address, Mobile, Email, Category) VALUES
('CUST001', 'Al Futtaim Trading LLC', 'Deira, Dubai', '0501112233', 'contact@alfuttaim.example', 'Wholesale'),
('CUST002', 'Sara Ahmed', 'Jumeirah, Dubai', '0559998877', 'sara.ahmed@example.com', 'Retail'),
('CUST003', 'Gulf Electronics Co.', 'Sharjah Industrial Area', '0567654321', 'sales@gulfelectronics.example', 'Wholesale'),
('CUST004', 'Mohammed Khalid', 'Al Ain', '0521239876', 'mkhalid@example.com', 'Retail'),
('CUST005', 'Bright Home Supplies', 'Abu Dhabi', '0544445566', 'info@brighthome.example', 'VIP');

-- Items
INSERT INTO dbo.ItemMaster (Code, Description, ItemPhoto, Cost, SalesPrice, Unit, Brand, Supplier) VALUES
('ITM001', 'Samsung 55" 4K Smart TV', NULL, 1450.00, 1899.00, 'PCS', 'Samsung', 'Gulf Electronics Distributors'),
('ITM002', 'LG Double Door Refrigerator 500L', NULL, 1980.00, 2499.00, 'PCS', 'LG', 'LG Gulf FZE'),
('ITM003', 'Philips LED Bulb 9W', NULL, 4.50, 8.00, 'PCS', 'Philips', 'Philips Middle East'),
('ITM004', 'Bosch Washing Machine 8kg', NULL, 950.00, 1299.00, 'PCS', 'Bosch', 'Bosch Home Appliances'),
('ITM005', 'Anker PowerBank 20000mAh', NULL, 65.00, 99.00, 'PCS', 'Anker', 'Anker Innovations'),
('ITM006', 'HP LaserJet Pro Printer', NULL, 420.00, 549.00, 'PCS', 'HP', 'HP Inc. Middle East'),
('ITM007', 'Dyson Cordless Vacuum V11', NULL, 1150.00, 1499.00, 'PCS', 'Dyson', 'Dyson Gulf'),
('ITM008', 'IKEA Office Chair', NULL, 180.00, 249.00, 'PCS', 'IKEA', 'IKEA Trading UAE');

-- Sample invoices
INSERT INTO dbo.SalesHeader (InvoiceType, InvoiceNumber, InvoiceDate, CustomerId, InvoiceDescription, TotalAmount)
VALUES ('INV', 1001, '2026-08-01', 1, 'Bulk order - August', 0),
       ('INV', 1002, '2026-08-05', 2, 'Retail purchase', 0),
       ('INV', 1003, '2026-08-12', 3, 'Electronics restock', 0);

INSERT INTO dbo.SalesDetails (InvoiceType, InvoiceNumber, ItemId, Qty, Price) VALUES
('INV', 1001, 1, 5, 1899.00),
('INV', 1001, 4, 3, 1299.00),
('INV', 1002, 3, 20, 8.00),
('INV', 1002, 5, 2, 99.00),
('INV', 1003, 2, 4, 2499.00),
('INV', 1003, 6, 6, 549.00),
('INV', 1003, 7, 2, 1499.00);

-- Recalculate header totals from detail lines
UPDATE h
SET h.TotalAmount = d.SumAmount
FROM dbo.SalesHeader h
INNER JOIN (
    SELECT InvoiceType, InvoiceNumber, SUM(Amount) AS SumAmount
    FROM dbo.SalesDetails
    GROUP BY InvoiceType, InvoiceNumber
) d ON d.InvoiceType = h.InvoiceType AND d.InvoiceNumber = h.InvoiceNumber;
GO

PRINT 'Seed data inserted.';
