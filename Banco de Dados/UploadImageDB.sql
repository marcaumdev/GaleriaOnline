-- Cria��o do banco de dados
CREATE DATABASE UploadImagemDB;
GO

-- Usar o banco rec�m-criado
USE UploadImagemDB;
GO

-- Cria��o da tabela Imagens
CREATE TABLE Imagens (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(255) NOT NULL,
    Caminho NVARCHAR(500) NOT NULL
);
GO