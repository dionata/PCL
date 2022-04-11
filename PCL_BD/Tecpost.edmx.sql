
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/19/2019 11:49:47
-- Generated from EDMX file: C:\Users\plant\Documents\TECPOST\TECPOST_BD\Tecpost.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TECPOST];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ClientModelMeshModel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MeshModelSet] DROP CONSTRAINT [FK_ClientModelMeshModel];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientModelCADModel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CADModelSet] DROP CONSTRAINT [FK_ClientModelCADModel];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientModelCAMModel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CAMModelSet] DROP CONSTRAINT [FK_ClientModelCAMModel];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ClientModelSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ClientModelSet];
GO
IF OBJECT_ID(N'[dbo].[MeshModelSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeshModelSet];
GO
IF OBJECT_ID(N'[dbo].[CADModelSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CADModelSet];
GO
IF OBJECT_ID(N'[dbo].[CAMModelSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CAMModelSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ClientModelSet'
CREATE TABLE [dbo].[ClientModelSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Codigo] int  NOT NULL,
    [CPF] bigint  NOT NULL,
    [RG] bigint  NOT NULL,
    [DataNascimento] datetime  NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [Endereco] nvarchar(max)  NOT NULL,
    [Complemento] nvarchar(max)  NOT NULL,
    [Bairro] nvarchar(max)  NOT NULL,
    [Cidade] nvarchar(max)  NOT NULL,
    [Estado] nvarchar(max)  NOT NULL,
    [CEP] bigint  NOT NULL,
    [Telefone] bigint  NOT NULL,
    [Celular1] bigint  NOT NULL,
    [Celular2] bigint  NOT NULL,
    [Email] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MeshModelSet'
CREATE TABLE [dbo].[MeshModelSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Mesh] varbinary(max)  NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [ClientModel_Id] int  NOT NULL
);
GO

-- Creating table 'CADModelSet'
CREATE TABLE [dbo].[CADModelSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CAD] varbinary(max)  NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [ClientModel_Id] int  NOT NULL
);
GO

-- Creating table 'CAMModelSet'
CREATE TABLE [dbo].[CAMModelSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CAM] varbinary(max)  NOT NULL,
    [Nome] nvarchar(max)  NOT NULL,
    [ClientModel_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ClientModelSet'
ALTER TABLE [dbo].[ClientModelSet]
ADD CONSTRAINT [PK_ClientModelSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MeshModelSet'
ALTER TABLE [dbo].[MeshModelSet]
ADD CONSTRAINT [PK_MeshModelSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CADModelSet'
ALTER TABLE [dbo].[CADModelSet]
ADD CONSTRAINT [PK_CADModelSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CAMModelSet'
ALTER TABLE [dbo].[CAMModelSet]
ADD CONSTRAINT [PK_CAMModelSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ClientModel_Id] in table 'MeshModelSet'
ALTER TABLE [dbo].[MeshModelSet]
ADD CONSTRAINT [FK_ClientModelMeshModel]
    FOREIGN KEY ([ClientModel_Id])
    REFERENCES [dbo].[ClientModelSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientModelMeshModel'
CREATE INDEX [IX_FK_ClientModelMeshModel]
ON [dbo].[MeshModelSet]
    ([ClientModel_Id]);
GO

-- Creating foreign key on [ClientModel_Id] in table 'CADModelSet'
ALTER TABLE [dbo].[CADModelSet]
ADD CONSTRAINT [FK_ClientModelCADModel]
    FOREIGN KEY ([ClientModel_Id])
    REFERENCES [dbo].[ClientModelSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientModelCADModel'
CREATE INDEX [IX_FK_ClientModelCADModel]
ON [dbo].[CADModelSet]
    ([ClientModel_Id]);
GO

-- Creating foreign key on [ClientModel_Id] in table 'CAMModelSet'
ALTER TABLE [dbo].[CAMModelSet]
ADD CONSTRAINT [FK_ClientModelCAMModel]
    FOREIGN KEY ([ClientModel_Id])
    REFERENCES [dbo].[ClientModelSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientModelCAMModel'
CREATE INDEX [IX_FK_ClientModelCAMModel]
ON [dbo].[CAMModelSet]
    ([ClientModel_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------