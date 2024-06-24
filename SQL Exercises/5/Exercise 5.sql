/*
	1. Re-design the e-commerce database by assigning a name to all constraints (Primary key, foreign key, default values, unique)

	Example: Notice the naming convention for each constraint. PK = Primary key, FK = Foreign key, UQ = Unique key, DF = Default value. Please use the following link as reference:
		- https://www.c-sharpcorner.com/UploadFile/f0b2ed/what-is-naming-convention/

	CREATE TABLE [Order]
	(
		ID					INT NOT NULL IDENTITY(1,1),
		CustomerID			INT NOT NULL,
		OrderStatusID		INT NOT NULL,
		OrderDate			DATE NOT NULL,
		TotalCost			DECIMAL(19,4) NOT NULL,
		CONSTRAINT [PK_Order] PRIMARY KEY(ID),
		CONSTRAINT [FK_Order_CustomerID] FOREIGN KEY (CustomerID) REFERENCES Customer(ID),
		CONSTRAINT [FK_Order_OrderStatusID] FOREIGN KEY (OrderStatusID) REFERENCES OrderStatus(ID)
	)
*/

SELECT 
    tc.TABLE_NAME, 
    tc.CONSTRAINT_NAME, 
    tc.CONSTRAINT_TYPE 
FROM 
    INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
ORDER BY 
    tc.TABLE_NAME, 
    tc.CONSTRAINT_TYPE;

EXEC sp_rename 'PK__Customer__A4AE64B8B8222BFB', 'PK_Customer', 'OBJECT';
EXEC sp_rename 'PK__OrderDet__08D097C184F23518', 'PK_OrderDetails', 'OBJECT';
EXEC sp_rename 'PK__Orders__C3905BAF753B0CF2', 'PK_Orders', 'OBJECT';
EXEC sp_rename 'PK__OrderSta__BC674F41F17CBABF', 'PK_OrderStatus', 'OBJECT';
EXEC sp_rename 'PK__Product__B40CC6EDC006F4FD', 'PK_Product', 'OBJECT';
EXEC sp_rename 'PK__ProductC__19093A2B3C61A8C7', 'PK_ProductCategory', 'OBJECT';
EXEC sp_rename 'PK__Supplier__4BE6669466628866', 'PK_Supplier', 'OBJECT';
EXEC sp_rename 'FK__OrderDeta__Order__59FA5E80', 'FK_OrderDetails_OrderID', 'OBJECT';
EXEC sp_rename 'FK__OrderDeta__Produ__5AEE82B9', 'FK_OrderDetails_ProductID', 'OBJECT';
EXEC sp_rename 'FK__Orders__Customer__5629CD9C', 'FK_Orders_CustomerID', 'OBJECT';
EXEC sp_rename 'FK__Orders__OrderSta__571DF1D5', 'FK_Orders_OrderStatusID', 'OBJECT';
EXEC sp_rename 'FK__Product__Product__4E88ABD4', 'FK_Product_ProductCategoryID', 'OBJECT';
EXEC sp_rename 'FK__Product__Supplie__4F7CD00D', 'FK_Product_SupplierID', 'OBJECT';



/*
	2. Re-create the script to populate the data using the MERGE statement. You have to make sure that the script doesn't duplicate the data in all tables when executed multiple times.

	Hint: We assumed that each customer can submit 1 order per date
*/


--For mess ups
DROP TABLE Customer;
DROP TABLE OrderDetails;
DROP TABLE Orders;
DROP TABLE OrderStatus;
DROP TABLE Product;
DROP TABLE ProductCategory
DROP TABLE Supplier;

--ProductCategory merge
MERGE INTO ProductCategory AS target
USING (
    VALUES 
        ('Electronics'),
        ('Clothing'),
        ('Books')
) AS source (CategoryName)
ON target.CategoryName = source.CategoryName
WHEN NOT MATCHED THEN
    INSERT (CategoryName)
    VALUES (source.CategoryName);


-- Supplier Merge
MERGE INTO Supplier AS target
USING (
    VALUES 
        ('Supplier A'),
        ('Supplier B'),
        ('Supplier C')
) AS source (SupplierName)
ON target.SupplierName = source.SupplierName
WHEN NOT MATCHED THEN
    INSERT (SupplierName)
    VALUES (source.SupplierName);

--Order Status merge
MERGE INTO OrderStatus AS target
USING (
    VALUES 
        ('Pending'),
        ('Shipped'),
        ('Delivered')
) AS source (StatusName)
ON target.StatusName = source.StatusName
WHEN NOT MATCHED THEN
    INSERT (StatusName)
    VALUES (source.StatusName);


--Product Merge
MERGE INTO Product AS target
USING (
    VALUES 
        ('Product A', 1, 1, 'Description A', 100.00),
        ('Product B', 2, 2, 'Description B', 50.00),
        ('Product C', 3, 3, 'Description C', 75.00)
) AS source (ProductName, ProductCategoryID, SupplierID, Description, Price)
ON target.ProductName = source.ProductName
WHEN NOT MATCHED THEN
    INSERT (ProductName, ProductCategoryID, SupplierID, Description, Price)
    VALUES (source.ProductName, source.ProductCategoryID, source.SupplierID, source.Description, source.Price);

--Customer Merge
MERGE INTO Customer AS target
USING (
    VALUES 
        ('John Doe', '123 Main St', NULL),
        ('Jane Smith', '456 Oak Ave', '555-1234'),
        ('Michael Johnson', '789 Elm Blvd', '555-5678')
) AS source (Name, Address, PhoneNumber)
ON target.Name = source.Name
WHEN NOT MATCHED THEN
    INSERT (Name, Address, PhoneNumber)
    VALUES (source.Name, source.Address, source.PhoneNumber);

--Orders Merge
MERGE INTO Orders AS target
USING (
    VALUES 
        (1, 1, '2024-06-20', 250.00), 
        (2, 2, '2024-06-21', 125.00), 
        (3, 3, '2024-06-22', 150.00)  
) AS source (CustomerID, OrderStatusID, OrderDate, TotalCost)
ON target.CustomerID = source.CustomerID AND target.OrderDate = source.OrderDate
WHEN NOT MATCHED THEN
    INSERT (CustomerID, OrderStatusID, OrderDate, TotalCost)
    VALUES (source.CustomerID, source.OrderStatusID, source.OrderDate, source.TotalCost);

--OrderDetails Merge
MERGE INTO OrderDetails AS target
USING (
    VALUES 
        (1, 1, 2, 100.00),
        (1, 2, 1, 50.00),  
        (2, 2, 3, 50.00),  
        (3, 3, 1, 150.00)  
) AS source (OrderID, ProductID, Quantity, UnitPrice)
ON target.OrderID = source.OrderID AND target.ProductID = source.ProductID
WHEN NOT MATCHED THEN
    INSERT (OrderID, ProductID, Quantity, UnitPrice)
    VALUES (source.OrderID, source.ProductID, source.Quantity, source.UnitPrice);


