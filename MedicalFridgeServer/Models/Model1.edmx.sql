
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/16/2019 03:04:23
-- Generated from EDMX file: D:\Учеба\III-Курс\АРКПЗ\Курсач\Back_End\MedicalFridgeServer\MedicalFridgeServer\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MedicalFridgeDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Fridges_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Fridges] DROP CONSTRAINT [FK_Fridges_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Indicators_Fridges]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Indicators] DROP CONSTRAINT [FK_Indicators_Fridges];
GO
IF OBJECT_ID(N'[dbo].[FK_Medicaments_Fridges]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Medicaments] DROP CONSTRAINT [FK_Medicaments_Fridges];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Fridges]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Fridges];
GO
IF OBJECT_ID(N'[dbo].[Indicators]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Indicators];
GO
IF OBJECT_ID(N'[dbo].[Medicaments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Medicaments];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Fridges'
CREATE TABLE [dbo].[Fridges] (
    [IdFridge] int IDENTITY(1,1) NOT NULL,
    [IdUser] int  NOT NULL,
    [Status] bit  NOT NULL
);
GO

-- Creating table 'Indicators'
CREATE TABLE [dbo].[Indicators] (
    [IdIndicators] int IDENTITY(1,1) NOT NULL,
    [IdFridge] int  NOT NULL,
    [Temperature] decimal(18,3)  NULL,
    [Humidity] decimal(18,3)  NULL,
    [DataTime] nchar(19)  NULL
);
GO

-- Creating table 'Medicaments'
CREATE TABLE [dbo].[Medicaments] (
    [IdMedicament] int IDENTITY(1,1) NOT NULL,
    [IdFridge] int  NOT NULL,
    [Name] nchar(40)  NOT NULL,
    [Amount] int  NOT NULL,
    [DataProduction] datetime  NOT NULL,
    [ExpirationDate] datetime  NOT NULL,
    [Price] decimal(18,2)  NOT NULL,
    [MinTemperature] decimal(18,2)  NULL,
    [MaxTemperature] decimal(18,2)  NULL,
    [Status] bit  NOT NULL,
    [DataAddInFridge] nchar(19)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [IdUser] int IDENTITY(1,1) NOT NULL,
    [Login] nchar(50)  NOT NULL,
    [Password] nchar(40)  NOT NULL,
    [NameOrganization] nchar(40)  NOT NULL,
    [Role] nchar(20)  NOT NULL,
    [Country] nchar(40)  NULL,
    [City] nchar(40)  NULL,
    [Address] nchar(40)  NULL,
    [Phone] nvarchar(50)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdFridge] in table 'Fridges'
ALTER TABLE [dbo].[Fridges]
ADD CONSTRAINT [PK_Fridges]
    PRIMARY KEY CLUSTERED ([IdFridge] ASC);
GO

-- Creating primary key on [IdIndicators] in table 'Indicators'
ALTER TABLE [dbo].[Indicators]
ADD CONSTRAINT [PK_Indicators]
    PRIMARY KEY CLUSTERED ([IdIndicators] ASC);
GO

-- Creating primary key on [IdMedicament] in table 'Medicaments'
ALTER TABLE [dbo].[Medicaments]
ADD CONSTRAINT [PK_Medicaments]
    PRIMARY KEY CLUSTERED ([IdMedicament] ASC);
GO

-- Creating primary key on [IdUser] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([IdUser] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IdUser] in table 'Fridges'
ALTER TABLE [dbo].[Fridges]
ADD CONSTRAINT [FK_Fridges_Users]
    FOREIGN KEY ([IdUser])
    REFERENCES [dbo].[Users]
        ([IdUser])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Fridges_Users'
CREATE INDEX [IX_FK_Fridges_Users]
ON [dbo].[Fridges]
    ([IdUser]);
GO

-- Creating foreign key on [IdFridge] in table 'Indicators'
ALTER TABLE [dbo].[Indicators]
ADD CONSTRAINT [FK_Indicators_Fridges]
    FOREIGN KEY ([IdFridge])
    REFERENCES [dbo].[Fridges]
        ([IdFridge])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Indicators_Fridges'
CREATE INDEX [IX_FK_Indicators_Fridges]
ON [dbo].[Indicators]
    ([IdFridge]);
GO

-- Creating foreign key on [IdFridge] in table 'Medicaments'
ALTER TABLE [dbo].[Medicaments]
ADD CONSTRAINT [FK_Medicaments_Fridges]
    FOREIGN KEY ([IdFridge])
    REFERENCES [dbo].[Fridges]
        ([IdFridge])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Medicaments_Fridges'
CREATE INDEX [IX_FK_Medicaments_Fridges]
ON [dbo].[Medicaments]
    ([IdFridge]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------