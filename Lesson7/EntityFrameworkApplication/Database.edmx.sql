
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/17/2020 15:26:13
-- Generated from EDMX file: C:\Users\Admin\Other\cod\Курс C# Уровень 3\Lesson7\EntityFrameworkApplication\Database.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Music];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Tracks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tracks];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Tracks'
CREATE TABLE [dbo].[Tracks] (
    [TrackId] int IDENTITY(1,1) NOT NULL,
    [ArtistName] nvarchar(max)  NOT NULL,
    [TrackName] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [TrackId] in table 'Tracks'
ALTER TABLE [dbo].[Tracks]
ADD CONSTRAINT [PK_Tracks]
    PRIMARY KEY CLUSTERED ([TrackId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------