/*
	1. Create stored procedures for the following entities for selecting and upserting (insert and update)
		- Supplier
		- ProductCategory
		- Product
		- Customer
		- Order
		- OrderItem


	Note: Each entity will have 2 stored procedure (for selecting and upserting). Use Merge statement for upserting

	Hint: We asume record will not be deleted from the tables
*/

--Select Supplier SP
CREATE PROCEDURE sp_SelectSupplier
AS
BEGIN
    SELECT SupplierID, SupplierName
    FROM Supplier;
END;

--Upsert Supplier SP
CREATE PROCEDURE sp_UpsertSupplier
    @SupplierID INT = NULL,
    @SupplierName VARCHAR(100)
AS
BEGIN
    MERGE Supplier AS target
    USING (SELECT @SupplierID, @SupplierName) AS source (SupplierID, SupplierName)
    ON target.SupplierName = source.SupplierName
    WHEN MATCHED THEN
        UPDATE SET SupplierName = source.SupplierName
    WHEN NOT MATCHED THEN
        INSERT (SupplierName)
        VALUES (source.SupplierName);
END


--Select Product Category sp
CREATE PROCEDURE sp_SelectProductCategory
AS
BEGIN
    SELECT CategoryID, CategoryName
    FROM ProductCategory;
END;

--upsert Product Category sp
CREATE PROCEDURE sp_UpsertProductCategory
    @ProductCategoryID INT = NULL,
    @CategoryName VARCHAR(100)
AS
BEGIN
    MERGE ProductCategory AS target
    USING (SELECT @ProductCategoryID, @CategoryName) AS source (ProductCategoryID, CategoryName)
    ON target.CategoryName = source.CategoryName
    WHEN MATCHED THEN
        UPDATE SET CategoryName = source.CategoryName
    WHEN NOT MATCHED THEN
        INSERT (CategoryName)
        VALUES (source.CategoryName);
END

--Select Prouct sp
CREATE PROCEDURE sp_SelectProduct
AS
BEGIN
    SELECT ProductID, ProductName, ProductCategoryID, SupplierID, Description, Price
    FROM Product;
END;

--Upsert Prodcut
CREATE PROCEDURE sp_UpsertProduct
    @ProductID INT = NULL,
    @ProductName VARCHAR(255),
    @ProductCategoryID INT,
    @SupplierID INT,
    @Description TEXT = NULL,
    @Price NUMERIC(10, 2)
AS
BEGIN
    MERGE Product AS target
    USING (SELECT @ProductID, @ProductName, @ProductCategoryID, @SupplierID, @Description, @Price) AS source (ProductID, ProductName, ProductCategoryID, SupplierID, Description, Price)
    ON target.ProductName = source.ProductName
    WHEN MATCHED THEN
        UPDATE SET ProductCategoryID = source.ProductCategoryID,
                   SupplierID = source.SupplierID,
                   Description = source.Description,
                   Price = source.Price
    WHEN NOT MATCHED THEN
        INSERT (ProductName, ProductCategoryID, SupplierID, Description, Price)
        VALUES (source.ProductName, source.ProductCategoryID, source.SupplierID, source.Description, source.Price);
END


-- Select Customer sp
CREATE PROCEDURE sp_SelectCustomer
AS
BEGIN
    SELECT CustomerID, Name, Address, PhoneNumber
    FROM Customer;
END;

--Upsert Customer sp
CREATE PROCEDURE sp_UpsertCustomer
    @CustomerID INT = NULL,
    @Name VARCHAR(100),
    @Address VARCHAR(255),
    @PhoneNumber VARCHAR(20) = NULL
AS
BEGIN
    MERGE Customer AS target
    USING (SELECT @CustomerID, @Name, @Address, @PhoneNumber) AS source (CustomerID, Name, Address, PhoneNumber)
    ON target.Name = source.Name AND target.Address = source.Address
    WHEN MATCHED THEN
        UPDATE SET PhoneNumber = source.PhoneNumber
    WHEN NOT MATCHED THEN
        INSERT (Name, Address, PhoneNumber)
        VALUES (source.Name, source.Address, source.PhoneNumber);
END



--Select Order sp
CREATE PROCEDURE sp_SelectOrder
AS
BEGIN
    SELECT OrderID, CustomerID, OrderStatusID, OrderDate, TotalCost
    FROM Orders;
END;

--Upsert Order SP
CREATE PROCEDURE sp_UpsertOrder
    @OrderID INT = NULL,
    @CustomerID INT,
    @OrderStatusID INT,
    @OrderDate DATE,
    @TotalCost NUMERIC(10, 2)
AS
BEGIN
    MERGE Orders AS target
    USING (SELECT @OrderID, @CustomerID, @OrderStatusID, @OrderDate, @TotalCost) AS source (OrderID, CustomerID, OrderStatusID, OrderDate, TotalCost)
    ON target.OrderID = source.OrderID
    WHEN MATCHED THEN
        UPDATE SET CustomerID = source.CustomerID,
                   OrderStatusID = source.OrderStatusID,
                   OrderDate = source.OrderDate,
                   TotalCost = source.TotalCost
    WHEN NOT MATCHED THEN
        INSERT (CustomerID, OrderStatusID, OrderDate, TotalCost)
        VALUES (source.CustomerID, source.OrderStatusID, source.OrderDate, source.TotalCost);
END


--Select OrderDetials sp
CREATE PROCEDURE sp_SelectOrderDetails
AS
BEGIN
    SELECT OrderID, ProductID, Quantity, UnitPrice
    FROM OrderDetails;
END;

--Upsert OrderDetails sp
CREATE PROCEDURE sp_UpsertOrderDetails
    @OrderID INT,
    @ProductID INT,
    @Quantity INT,
    @UnitPrice NUMERIC(10, 2)
AS
BEGIN
    MERGE OrderDetails AS target
    USING (SELECT @OrderID, @ProductID, @Quantity, @UnitPrice) AS source (OrderID, ProductID, Quantity, UnitPrice)
    ON target.OrderID = source.OrderID AND target.ProductID = source.ProductID
    WHEN MATCHED THEN
        UPDATE SET Quantity = source.Quantity,
                   UnitPrice = source.UnitPrice
    WHEN NOT MATCHED THEN
        INSERT (OrderID, ProductID, Quantity, UnitPrice)
        VALUES (source.OrderID, source.ProductID, source.Quantity, source.UnitPrice);
END




/*
	2. Create a script that calls/uses all the stored procedures from the exercise #1
*/

-- Upsert/Select Suppliers
EXEC sp_UpsertSupplier @SupplierID = 10, @SupplierName = 'Supplier D';
EXEC sp_UpsertSupplier @SupplierID = 11, @SupplierName = 'Supplier E';
EXEC sp_SelectSupplier;


-- Upsert/Select Product Categories
EXEC sp_UpsertProductCategory @ProductCategoryID = NULL, @CategoryName = 'Electronics';
EXEC sp_UpsertProductCategory @ProductCategoryID = NULL, @CategoryName = 'Books';
EXEC sp_SelectProductCategory;

-- Upsert/Select Products
EXEC sp_UpsertProduct @ProductID = NULL, @ProductName = 'Console', @ProductCategoryID = 1, @SupplierID = 1, @Description = 'High-performance laptop', @Price = 999.99;
EXEC sp_UpsertProduct @ProductID = NULL, @ProductName = 'PC', @ProductCategoryID = 1, @SupplierID = 2, @Description = 'Latest model smartphone', @Price = 699.99;
EXEC sp_SelectProduct;

-- Upsert/Select Customers
EXEC sp_UpsertCustomer @CustomerID = NULL, @Name = 'John Doe', @Address = '123 Main St', @PhoneNumber = '555-1234';
EXEC sp_UpsertCustomer @CustomerID = NULL, @Name = 'Jane Smith', @Address = '456 Elm St', @PhoneNumber = '555-1234';
EXEC sp_SelectCustomer


-- Upsert/Select Orders
EXEC sp_UpsertOrder @OrderID = NULL, @CustomerID = 1, @OrderStatusID = 1, @OrderDate = '2024-06-21', @TotalCost = 999.99;
EXEC sp_UpsertOrder @OrderID = NULL, @CustomerID = 2, @OrderStatusID = 1, @OrderDate = '2024-06-22', @TotalCost = 699.99;
EXEC sp_SelectOrder;

-- Upsert/Select Order Items
EXEC sp_UpsertOrderDetails @OrderID = 1, @ProductID = 1, @Quantity = 1, @UnitPrice = 999.99;
EXEC sp_UpsertOrderDetails @OrderID = 2, @ProductID = 2, @Quantity = 1, @UnitPrice = 699.99;
EXEC sp_SelectOrderDetails;



/*
	3. Create a view (vw_SupplierSoldItemsPerYear) that display all sold items by supplier for each year

	Hint: use the following link to create a view
		- https://learn.microsoft.com/en-us/sql/relational-databases/views/create-views?view=sql-server-ver16
*/

CREATE VIEW vw_SupplierSoldItemsPerYear AS
SELECT 
    s.SupplierID,
    s.SupplierName,
    YEAR(o.OrderDate) AS Year,
    COUNT(od.ProductID) AS TotalSoldItems
FROM 
    Supplier s
JOIN 
    Product p ON s.SupplierID = p.SupplierID
JOIN 
    OrderDetails od ON p.ProductID = od.ProductID
JOIN 
    Orders o ON od.OrderID = o.OrderID
GROUP BY 
    s.SupplierID,
    s.SupplierName,
    YEAR(o.OrderDate);





