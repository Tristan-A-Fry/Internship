/*
	Using the TF_DB do the following
	
	1. You need to design the database for an e-commerce application. The application will have a list of product where prospect customers can create orders.
	The following information for the product must be defined:
		- ProductCategory (Required)
		- Supplier (Required)
		- ProductName (Required)
		- Description (Optional)
		- Price (Required)

	The prospect customer can create an account in the application and the following information must be store:
		- Name (Required)
		- Address (Required)
		- PhoneNumber (Optional)

	Each customer must be able to see all the orders (with the purchased products) they have submitted in the application. Each order must have the following information:
		- OrderStatus (Required) 
		- OrderDate	(Required)
		- TotalCost (Required)

	Notes: ProductCategory, Supplier and OrderStatus has to be defined as an independent entity in the database. Each will required at least the column Name.
	-The OrderStatus's primary key must be manually assigned when inserting a row since its ID have a business logic. 
	The OrderStatus table must have the following rows that represents the following: 1 - 'Pending', 2 - 'Shipped', 3 - 'Delivered'
	- All tables must have their primary key defined as INT. All PK must have their identity assigned automatically (except OrderStatus and many-to-many tables)
	- Relationships between tables must be defined using foreign keys
*/


--Stores different categories of products.
CREATE TABLE ProductCategory (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(100) NOT NULL
);

--Stores information about suppliers who provide products.
CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName VARCHAR(100) NOT NULL
);

--Stores detailed information about each product.
CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(255) NOT NULL,
    ProductCategoryID INT NOT NULL,
    SupplierID INT NOT NULL,
    Description TEXT,
    Price NUMERIC(10, 2) NOT NULL,
    FOREIGN KEY (ProductCategoryID) REFERENCES ProductCategory(CategoryID), --Foreign key referencing ProductCategory.CategoryID, specifies which category the product belongs to.
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
);

CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20)
);

CREATE TABLE OrderStatus (
    OrderStatusID INT PRIMARY KEY,
    StatusName VARCHAR(50) NOT NULL
);

-- Insert initial rows for OrderStatus
INSERT INTO OrderStatus (OrderStatusID, StatusName) VALUES
(1, 'Pending'),
(2, 'Shipped'),
(3, 'Delivered');

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    OrderStatusID INT NOT NULL,
    OrderDate DATE NOT NULL,
    TotalCost NUMERIC(10, 2) NOT NULL,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (OrderStatusID) REFERENCES OrderStatus(OrderStatusID)
);

CREATE TABLE OrderDetails (
    OrderID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    UnitPrice NUMERIC(10, 2) NOT NULL,
    PRIMARY KEY (OrderID, ProductID),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);





/*
	2. Create a script to generate data for each table
*/


INSERT INTO ProductCategory (CategoryName)
VALUES ('Electronics'),
       ('Clothing'),
       ('Books'),
       ('Home & Kitchen');


INSERT INTO Supplier (SupplierName)
VALUES ('Supplier A'),
       ('Supplier B'),
       ('Supplier C'),
       ('Supplier D');


INSERT INTO Product (ProductName, ProductCategoryID, SupplierID, Description, Price)
VALUES ('Laptop', 1, 1, 'High performance laptop', 1200.00),
       ('T-Shirt', 2, 2, 'Cotton T-Shirt', 25.00),
       ('Book - SQL Basics', 3, 3, 'Introduction to SQL', 35.00),
       ('Kitchen Blender', 4, 4, 'Multi-function blender', 75.00);


INSERT INTO Customer (Name, Address, PhoneNumber)
VALUES ('John Doe', '123 Main St, Anytown', '555-1234'),
       ('Jane Smith', '456 Elm St, Another Town', NULL),
       ('Michael Brown', '789 Oak St, Yet Another Town', '555-5678');


INSERT INTO Orders (CustomerID, OrderStatusID, OrderDate, TotalCost)
VALUES (1, 1, '2024-06-20', 1225.00),
       (2, 2, '2024-06-19', 50.00),
       (3, 3, '2024-06-18', 110.00);


INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice)
VALUES (1, 1, 1, 1200.00),
       (2, 2, 2, 50.00),
       (3, 3, 1, 35.00),
       (1, 4, 1, 25.00);






