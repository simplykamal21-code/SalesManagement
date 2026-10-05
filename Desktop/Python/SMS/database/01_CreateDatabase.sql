/*
    Sales Management System - Database Creation Script
    Run this first, then 02_SeedData.sql
*/

IF DB_ID('SalesManagementDB') IS NULL
BEGIN
    CREATE DATABASE SalesManagementDB;
END
GO

USE SalesManagementDB;
GO

-- ========================================
-- User Master
-- ========================================
IF OBJECT_ID('dbo.UserMaster', 'U') IS NOT NULL DROP TABLE dbo.UserMaster;
CREATE TABLE dbo.UserMaster (
    UserId          INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(100)   NOT NULL,
    Username        NVARCHAR(50)    NOT NULL UNIQUE,
    Mobile          NVARCHAR(20)    NULL,
    PasswordHash    NVARCHAR(256)   NOT NULL,   -- BCrypt hash, never store plain text
    CreatedOn       DATETIME        NOT NULL DEFAULT GETDATE(),
    IsActive        BIT             NOT NULL DEFAULT 1
);
GO

-- ========================================
-- Customer Master
-- ========================================
IF OBJECT_ID('dbo.CustomerMaster', 'U') IS NOT NULL DROP TABLE dbo.CustomerMaster;
CREATE TABLE dbo.CustomerMaster (
    CustomerId      INT IDENTITY(1,1) PRIMARY KEY,
    Code            NVARCHAR(20)    NOT NULL UNIQUE,
    Name            NVARCHAR(150)   NOT NULL,
    Address         NVARCHAR(300)   NULL,
    Mobile          NVARCHAR(20)    NULL,
    Email           NVARCHAR(100)   NULL,
    Category        NVARCHAR(50)    NULL,       -- Retail / Wholesale / VIP etc
    CreatedOn       DATETIME        NOT NULL DEFAULT GETDATE(),
    IsActive        BIT             NOT NULL DEFAULT 1
);
GO

-- ========================================
-- Item Master
-- ========================================
IF OBJECT_ID('dbo.ItemMaster', 'U') IS NOT NULL DROP TABLE dbo.ItemMaster;
CREATE TABLE dbo.ItemMaster (
    ItemId          INT IDENTITY(1,1) PRIMARY KEY,
    Code            NVARCHAR(20)    NOT NULL UNIQUE,
    Description     NVARCHAR(200)   NOT NULL,
    ItemPhoto       NVARCHAR(MAX)   NULL,        -- Base64 string or URL
    Cost            DECIMAL(18,2)   NOT NULL DEFAULT 0,
    SalesPrice      DECIMAL(18,2)   NOT NULL DEFAULT 0,
    Unit            NVARCHAR(20)    NULL,
    Brand           NVARCHAR(100)   NULL,
    Supplier        NVARCHAR(150)   NULL,
    CreatedOn       DATETIME        NOT NULL DEFAULT GETDATE(),
    IsActive        BIT             NOT NULL DEFAULT 1
);
GO

-- ========================================
-- Sales Header
-- ========================================
IF OBJECT_ID('dbo.SalesHeader', 'U') IS NOT NULL DROP TABLE dbo.SalesHeader;
CREATE TABLE dbo.SalesHeader (
    InvoiceType         NVARCHAR(10)    NOT NULL DEFAULT 'INV',
    InvoiceNumber        INT             NOT NULL,
    InvoiceDate          DATETIME        NOT NULL DEFAULT GETDATE(),
    CustomerId            INT             NOT NULL,
    InvoiceDescription    NVARCHAR(300)   NULL,
    TotalAmount           DECIMAL(18,2)   NOT NULL DEFAULT 0,
    CreatedOn             DATETIME        NOT NULL DEFAULT GETDATE(),
    CONSTRAINT PK_SalesHeader PRIMARY KEY (InvoiceType, InvoiceNumber),
    CONSTRAINT FK_SalesHeader_Customer FOREIGN KEY (CustomerId) REFERENCES dbo.CustomerMaster(CustomerId)
);
GO

-- ========================================
-- Sales Details
-- ========================================
IF OBJECT_ID('dbo.SalesDetails', 'U') IS NOT NULL DROP TABLE dbo.SalesDetails;
CREATE TABLE dbo.SalesDetails (
    SalesDetailId   INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceType     NVARCHAR(10)    NOT NULL,
    InvoiceNumber   INT             NOT NULL,
    ItemId          INT             NOT NULL,
    Qty             DECIMAL(18,2)   NOT NULL,
    Price           DECIMAL(18,2)   NOT NULL,
    Amount          AS (Qty * Price) PERSISTED,
    CONSTRAINT FK_SalesDetails_Header FOREIGN KEY (InvoiceType, InvoiceNumber)
        REFERENCES dbo.SalesHeader(InvoiceType, InvoiceNumber) ON DELETE CASCADE,
    CONSTRAINT FK_SalesDetails_Item FOREIGN KEY (ItemId) REFERENCES dbo.ItemMaster(ItemId)
);
GO

CREATE INDEX IX_SalesDetails_Invoice ON dbo.SalesDetails(InvoiceType, InvoiceNumber);
CREATE INDEX IX_SalesHeader_Date ON dbo.SalesHeader(InvoiceDate);
GO

PRINT 'Database and tables created successfully.';
