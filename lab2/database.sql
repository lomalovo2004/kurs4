CREATE DATABASE IF NOT EXISTS delivery_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE delivery_db;

CREATE TABLE IF NOT EXISTS categories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS products (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    StockQuantity INT NOT NULL,
    CategoryId INT NOT NULL,
    CONSTRAINT FK_products_categories
        FOREIGN KEY (CategoryId) REFERENCES categories(Id)
);

CREATE TABLE IF NOT EXISTS customers (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    Phone VARCHAR(30) NOT NULL,
    Email VARCHAR(150) NOT NULL
);

CREATE TABLE IF NOT EXISTS orders (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CreatedAt DATETIME NOT NULL,
    CustomerId INT NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Login VARCHAR(100) NOT NULL UNIQUE,
    PassHash VARCHAR(255) NOT NULL
);

INSERT INTO categories (Name)
SELECT 'Электроника'
WHERE NOT EXISTS (SELECT 1 FROM categories WHERE Name = 'Электроника');

INSERT INTO categories (Name)
SELECT 'Товары для дома'
WHERE NOT EXISTS (SELECT 1 FROM categories WHERE Name = 'Товары для дома');
