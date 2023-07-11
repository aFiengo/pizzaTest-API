DROP DATABASE IF EXISTS PizzeriaDB;
CREATE DATABASE PizzeriaDB;

USE PizzeriaDB;

-- Create Pizza table
CREATE TABLE Pizza (
  Id CHAR(36) PRIMARY KEY,
  Name VARCHAR(255),
  SmallImageUrl VARCHAR(255),
  LargeImageUrl VARCHAR(255),
  Description VARCHAR(255)
);

-- Create Toppings table
CREATE TABLE Topping (
  Id CHAR(36) PRIMARY KEY,
  Name VARCHAR(255)
);

-- Create PizzaToppings table
CREATE TABLE PizzaTopping (
  PizzaId CHAR(36),
  ToppingId CHAR(36),
  PRIMARY KEY (PizzaId, ToppingId),
  FOREIGN KEY (PizzaId) REFERENCES Pizza(Id),
  FOREIGN KEY (ToppingId) REFERENCES Topping(Id)
);





