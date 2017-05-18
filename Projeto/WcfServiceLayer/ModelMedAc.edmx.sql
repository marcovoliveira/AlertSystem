
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/09/2017 04:32:11
-- Generated from EDMX file: C:\Users\Marco\Source\Repos\ProjetoMedAC2\WcfServiceLayer\ModelMedAc.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProjectDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UtenteBP]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BPs] DROP CONSTRAINT [FK_UtenteBP];
GO
IF OBJECT_ID(N'[dbo].[FK_UtenteHR]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HRs] DROP CONSTRAINT [FK_UtenteHR];
GO
IF OBJECT_ID(N'[dbo].[FK_UtenteSPO]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SPOes] DROP CONSTRAINT [FK_UtenteSPO];
GO
IF OBJECT_ID(N'[dbo].[FK_AlertaUtente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Alertas] DROP CONSTRAINT [FK_AlertaUtente];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Utentes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Utentes];
GO
IF OBJECT_ID(N'[dbo].[BPs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BPs];
GO
IF OBJECT_ID(N'[dbo].[HRs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HRs];
GO
IF OBJECT_ID(N'[dbo].[SPOes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SPOes];
GO
IF OBJECT_ID(N'[dbo].[Alertas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Alertas];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Utentes'
CREATE TABLE [dbo].[Utentes] (
    [IdUtente] int IDENTITY(1,1) NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Apelido] nvarchar(max)  NOT NULL,
    [DataNasc] datetime  NULL,
    [SNS] int  NOT NULL,
    [Idade] nvarchar(max)  NULL,
    [Activo] bit  NOT NULL
);
GO

-- Creating table 'BPs'
CREATE TABLE [dbo].[BPs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Valor1] int  NOT NULL,
    [Valor2] int  NOT NULL,
    [Alerta] bit  NOT NULL,
    [Utente_IdUtente] int  NOT NULL
);
GO

-- Creating table 'HRs'
CREATE TABLE [dbo].[HRs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Valor] int  NOT NULL,
    [Alerta] bit  NOT NULL,
    [Utente_IdUtente] int  NOT NULL
);
GO

-- Creating table 'SPOes'
CREATE TABLE [dbo].[SPOes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Data] datetime  NOT NULL,
    [Valor] int  NOT NULL,
    [Alerta] bit  NOT NULL,
    [Utente_IdUtente] int  NOT NULL
);
GO

-- Creating table 'Alertas'
CREATE TABLE [dbo].[Alertas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tipo] nvarchar(max)  NULL,
    [Data] datetime  NOT NULL,
    [Parametro] nvarchar(max)  NULL,
    [Utente_IdUtente] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdUtente] in table 'Utentes'
ALTER TABLE [dbo].[Utentes]
ADD CONSTRAINT [PK_Utentes]
    PRIMARY KEY CLUSTERED ([IdUtente] ASC);
GO

-- Creating primary key on [Id] in table 'BPs'
ALTER TABLE [dbo].[BPs]
ADD CONSTRAINT [PK_BPs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HRs'
ALTER TABLE [dbo].[HRs]
ADD CONSTRAINT [PK_HRs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SPOes'
ALTER TABLE [dbo].[SPOes]
ADD CONSTRAINT [PK_SPOes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Alertas'
ALTER TABLE [dbo].[Alertas]
ADD CONSTRAINT [PK_Alertas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Utente_IdUtente] in table 'BPs'
ALTER TABLE [dbo].[BPs]
ADD CONSTRAINT [FK_UtenteBP]
    FOREIGN KEY ([Utente_IdUtente])
    REFERENCES [dbo].[Utentes]
        ([IdUtente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteBP'
CREATE INDEX [IX_FK_UtenteBP]
ON [dbo].[BPs]
    ([Utente_IdUtente]);
GO

-- Creating foreign key on [Utente_IdUtente] in table 'HRs'
ALTER TABLE [dbo].[HRs]
ADD CONSTRAINT [FK_UtenteHR]
    FOREIGN KEY ([Utente_IdUtente])
    REFERENCES [dbo].[Utentes]
        ([IdUtente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteHR'
CREATE INDEX [IX_FK_UtenteHR]
ON [dbo].[HRs]
    ([Utente_IdUtente]);
GO

-- Creating foreign key on [Utente_IdUtente] in table 'SPOes'
ALTER TABLE [dbo].[SPOes]
ADD CONSTRAINT [FK_UtenteSPO]
    FOREIGN KEY ([Utente_IdUtente])
    REFERENCES [dbo].[Utentes]
        ([IdUtente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UtenteSPO'
CREATE INDEX [IX_FK_UtenteSPO]
ON [dbo].[SPOes]
    ([Utente_IdUtente]);
GO

-- Creating foreign key on [Utente_IdUtente] in table 'Alertas'
ALTER TABLE [dbo].[Alertas]
ADD CONSTRAINT [FK_AlertaUtente]
    FOREIGN KEY ([Utente_IdUtente])
    REFERENCES [dbo].[Utentes]
        ([IdUtente])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlertaUtente'
CREATE INDEX [IX_FK_AlertaUtente]
ON [dbo].[Alertas]
    ([Utente_IdUtente]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------