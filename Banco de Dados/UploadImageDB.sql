-- Criação do banco de dados
CREATE DATABASE UploadImagemDB;
GO

-- Usar o banco recém-criado
USE UploadImagemDB;
GO

-- Criação da tabela Imagens
CREATE TABLE Imagens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(255) NOT NULL,
    Caminho NVARCHAR(500) NOT NULL
);
GO