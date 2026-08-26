/* ============================================================
   Kosa eCommerce - Idempotent Seed Data (SQL Server)
   Safe to run repeatedly: every insert is guarded with
   IF NOT EXISTS. No DROP / DELETE / ALTER statements.
   Requires the schema from the InitialKosa EF migration.
   Identity values are explicit (IDENTITY_INSERT) so the DB
   productId/categoryId/promotionId match the frontend contract
   (productId 1..21, categoryId 1..6, promotionId 1..2).
   ============================================================ */
USE [eCommerce];
GO
SET XACT_ABORT ON;
GO
BEGIN TRAN;
GO

-- Categories
SET IDENTITY_INSERT [dbo].[Categories] ON;
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE CategoryName = N'Vegetables')
  INSERT INTO [dbo].[Categories] (CategoryId, CategoryName, ImageUrl, Description, IsActive) VALUES (1, N'Vegetables', N'/images/categories/vegetable.webp', N'Crisp everyday vegetables selected for fresh home cooking.', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE CategoryName = N'Fresh Fruit')
  INSERT INTO [dbo].[Categories] (CategoryId, CategoryName, ImageUrl, Description, IsActive) VALUES (2, N'Fresh Fruit', N'/images/categories/fruits.webp', N'Bright seasonal fruit for snacks, juices, and family tables.', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE CategoryName = N'Dry Fruits')
  INSERT INTO [dbo].[Categories] (CategoryId, CategoryName, ImageUrl, Description, IsActive) VALUES (3, N'Dry Fruits', N'/images/categories/dry-fruits.webp', N'Pantry-friendly fruit picks with a naturally sweet finish.', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE CategoryName = N'Leafy Green')
  INSERT INTO [dbo].[Categories] (CategoryId, CategoryName, ImageUrl, Description, IsActive) VALUES (4, N'Leafy Green', N'/images/categories/leafy-green.webp', N'Fresh greens for salads, sides, and healthy daily meals.', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE CategoryName = N'Organic')
  INSERT INTO [dbo].[Categories] (CategoryId, CategoryName, ImageUrl, Description, IsActive) VALUES (5, N'Organic', N'/images/categories/organic.webp', N'Carefully selected organic groceries and clean ingredients.', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE CategoryName = N'Juices')
  INSERT INTO [dbo].[Categories] (CategoryId, CategoryName, ImageUrl, Description, IsActive) VALUES (6, N'Juices', N'/images/products/organic-drink-1.webp', N'Fresh natural juices for refreshing daily moments.', 1);
SET IDENTITY_INSERT [dbo].[Categories] OFF;
GO

-- Products
SET IDENTITY_INSERT [dbo].[Products] ON;
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Green Apple')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (1, 2, N'Green Apple', N'Crunchy green apples with a clean, tart sweetness.', N'Kg', 1.6, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Banana')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (2, 2, N'Banana', N'Naturally sweet bananas ready for breakfast and smoothies.', N'Kg', 1.4, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Mango')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (3, 2, N'Mango', N'Juicy mangoes with a rich tropical flavor.', N'Kg', 2.8, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Orange')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (4, 2, N'Orange', N'Fresh oranges with a bright citrus finish.', N'Kg', 1.4, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Pineapple')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (5, 2, N'Pineapple', N'Sweet pineapple for desserts, juices, and fruit bowls.', N'Piece', 2.75, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Strawberries')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (6, 2, N'Strawberries', N'Fresh strawberries packed for snacking and desserts.', N'Box', 2.8, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Tomato')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (7, 1, N'Tomato', N'Ripe tomatoes for salads, sauces, and daily meals.', N'Kg', 0.7, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Cucumber')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (8, 1, N'Cucumber', N'Cool cucumbers with a crisp, refreshing bite.', N'Kg', 0.85, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Carrot')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (9, 1, N'Carrot', N'Sweet carrots for soups, salads, and roasting.', N'Kg', 0.99, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Potato')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (10, 1, N'Potato', N'Reliable potatoes for baking, frying, and home cooking.', N'Kg', 0.7, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Onion')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (11, 1, N'Onion', N'Fresh onions for everyday flavor foundations.', N'Kg', 0.64, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Spinach')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (12, 4, N'Spinach', N'Tender spinach leaves for salads and warm dishes.', N'Bunch', 0.49, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Lettuce')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (13, 4, N'Lettuce', N'Fresh lettuce with crisp leaves for simple salads.', N'Box', 1.1, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Mint')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (14, 4, N'Mint', N'Fragrant mint for salads, fresh drinks, and warm cups.', N'Bunch', 0.35, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Coriander')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (15, 4, N'Coriander', N'Aromatic coriander for bright finishing flavor.', N'Bunch', 0.35, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Dried Fruit Mix')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (16, 3, N'Dried Fruit Mix', N'A naturally sweet dried fruit mix for snacks and lunch boxes.', N'Box', 4.99, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Organic Greens Box')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (17, 5, N'Organic Greens Box', N'A balanced organic produce box for simple weekly meals.', N'Box', 6.0, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Mint Tea Bundle')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (18, 6, N'Mint Tea Bundle', N'A fresh mint drink bundle for warm cups and cool infusions.', N'Bunch', 1.75, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Orange Juice')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (19, 6, N'Orange Juice', N'Fresh orange juice with a bright citrus taste.', N'Bottle', 1.8, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Strawberry Juice')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (20, 6, N'Strawberry Juice', N'Sweet strawberry juice made for a refreshing daily drink.', N'Bottle', 2.03, 1, GETDATE());
IF NOT EXISTS (SELECT 1 FROM [dbo].[Products] WHERE ProductName = N'Apple Juice')
  INSERT INTO [dbo].[Products] (ProductId, CategoryId, ProductName, Description, Unit, Price, IsActive, CreatedDate) VALUES (21, 6, N'Apple Juice', N'Rich apple juice with a bold natural flavor.', N'Bottle', 2.48, 1, GETDATE());
SET IDENTITY_INSERT [dbo].[Products] OFF;
GO

-- ProductImages (image contract: /images/products/<file>.webp served from backend wwwroot)
SET IDENTITY_INSERT [dbo].[ProductImages] ON;
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 1 AND ImageUrl = N'/images/products/green-apple-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (1, 1, N'/images/products/green-apple-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 1 AND ImageUrl = N'/images/products/green-apple-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (2, 1, N'/images/products/green-apple-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 1 AND ImageUrl = N'/images/products/green-apple-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (3, 1, N'/images/products/green-apple-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 2 AND ImageUrl = N'/images/products/banana-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (4, 2, N'/images/products/banana-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 2 AND ImageUrl = N'/images/products/banana-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (5, 2, N'/images/products/banana-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 2 AND ImageUrl = N'/images/products/banana-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (6, 2, N'/images/products/banana-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 3 AND ImageUrl = N'/images/products/mango-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (7, 3, N'/images/products/mango-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 3 AND ImageUrl = N'/images/products/mango-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (8, 3, N'/images/products/mango-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 3 AND ImageUrl = N'/images/products/mango-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (9, 3, N'/images/products/mango-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 4 AND ImageUrl = N'/images/products/orange-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (10, 4, N'/images/products/orange-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 4 AND ImageUrl = N'/images/products/orange-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (11, 4, N'/images/products/orange-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 4 AND ImageUrl = N'/images/products/orange-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (12, 4, N'/images/products/orange-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 5 AND ImageUrl = N'/images/products/pineapple-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (13, 5, N'/images/products/pineapple-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 5 AND ImageUrl = N'/images/products/pineapple-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (14, 5, N'/images/products/pineapple-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 5 AND ImageUrl = N'/images/products/pineapple-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (15, 5, N'/images/products/pineapple-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 6 AND ImageUrl = N'/images/products/strawberries-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (16, 6, N'/images/products/strawberries-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 6 AND ImageUrl = N'/images/products/strawberries-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (17, 6, N'/images/products/strawberries-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 6 AND ImageUrl = N'/images/products/strawberries-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (18, 6, N'/images/products/strawberries-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 7 AND ImageUrl = N'/images/products/tomato-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (19, 7, N'/images/products/tomato-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 7 AND ImageUrl = N'/images/products/tomato-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (20, 7, N'/images/products/tomato-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 7 AND ImageUrl = N'/images/products/tomato-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (21, 7, N'/images/products/tomato-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 8 AND ImageUrl = N'/images/products/cucumber-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (22, 8, N'/images/products/cucumber-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 8 AND ImageUrl = N'/images/products/cucumber-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (23, 8, N'/images/products/cucumber-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 8 AND ImageUrl = N'/images/products/cucumber-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (24, 8, N'/images/products/cucumber-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 9 AND ImageUrl = N'/images/products/carrot-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (25, 9, N'/images/products/carrot-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 9 AND ImageUrl = N'/images/products/carrot-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (26, 9, N'/images/products/carrot-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 9 AND ImageUrl = N'/images/products/carrot-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (27, 9, N'/images/products/carrot-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 10 AND ImageUrl = N'/images/products/potato-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (28, 10, N'/images/products/potato-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 10 AND ImageUrl = N'/images/products/potato-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (29, 10, N'/images/products/potato-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 10 AND ImageUrl = N'/images/products/potato-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (30, 10, N'/images/products/potato-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 11 AND ImageUrl = N'/images/products/onion-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (31, 11, N'/images/products/onion-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 11 AND ImageUrl = N'/images/products/onion-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (32, 11, N'/images/products/onion-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 11 AND ImageUrl = N'/images/products/onion-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (33, 11, N'/images/products/onion-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 12 AND ImageUrl = N'/images/products/spinach-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (34, 12, N'/images/products/spinach-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 12 AND ImageUrl = N'/images/products/spinach-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (35, 12, N'/images/products/spinach-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 12 AND ImageUrl = N'/images/products/spinach-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (36, 12, N'/images/products/spinach-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 13 AND ImageUrl = N'/images/products/lettuce-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (37, 13, N'/images/products/lettuce-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 13 AND ImageUrl = N'/images/products/lettuce-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (38, 13, N'/images/products/lettuce-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 13 AND ImageUrl = N'/images/products/lettuce-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (39, 13, N'/images/products/lettuce-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 14 AND ImageUrl = N'/images/products/mint-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (40, 14, N'/images/products/mint-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 14 AND ImageUrl = N'/images/products/mint-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (41, 14, N'/images/products/mint-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 14 AND ImageUrl = N'/images/products/mint-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (42, 14, N'/images/products/mint-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 15 AND ImageUrl = N'/images/products/coriander-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (43, 15, N'/images/products/coriander-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 15 AND ImageUrl = N'/images/products/coriander-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (44, 15, N'/images/products/coriander-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 15 AND ImageUrl = N'/images/products/coriander-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (45, 15, N'/images/products/coriander-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 16 AND ImageUrl = N'/images/products/dried-fruit-mix-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (46, 16, N'/images/products/dried-fruit-mix-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 16 AND ImageUrl = N'/images/products/dried-fruit-mix-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (47, 16, N'/images/products/dried-fruit-mix-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 16 AND ImageUrl = N'/images/products/dried-fruit-mix-4.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (48, 16, N'/images/products/dried-fruit-mix-4.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 17 AND ImageUrl = N'/images/products/organic-greens-box-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (49, 17, N'/images/products/organic-greens-box-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 17 AND ImageUrl = N'/images/products/organic-greens-box-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (50, 17, N'/images/products/organic-greens-box-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 17 AND ImageUrl = N'/images/products/organic-greens-box-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (51, 17, N'/images/products/organic-greens-box-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 18 AND ImageUrl = N'/images/products/mint-tea-bundle-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (52, 18, N'/images/products/mint-tea-bundle-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 18 AND ImageUrl = N'/images/products/mint-tea-bundle-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (53, 18, N'/images/products/mint-tea-bundle-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 18 AND ImageUrl = N'/images/products/mint-tea-bundle-3.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (54, 18, N'/images/products/mint-tea-bundle-3.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 19 AND ImageUrl = N'/images/products/organic-drink-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (55, 19, N'/images/products/organic-drink-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 19 AND ImageUrl = N'/images/products/organic-drink-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (56, 19, N'/images/products/organic-drink-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 20 AND ImageUrl = N'/images/products/strawberry-juice-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (57, 20, N'/images/products/strawberry-juice-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 20 AND ImageUrl = N'/images/products/strawberry-juice-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (58, 20, N'/images/products/strawberry-juice-2.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 21 AND ImageUrl = N'/images/products/apple-juice-1.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (59, 21, N'/images/products/apple-juice-1.webp');
IF NOT EXISTS (SELECT 1 FROM [dbo].[ProductImages] WHERE ProductId = 21 AND ImageUrl = N'/images/products/apple-juice-2.webp')
  INSERT INTO [dbo].[ProductImages] (ImageId, ProductId, ImageUrl) VALUES (60, 21, N'/images/products/apple-juice-2.webp');
SET IDENTITY_INSERT [dbo].[ProductImages] OFF;
GO

-- Promotions (active window covers 2026)
SET IDENTITY_INSERT [dbo].[Promotions] ON;
IF NOT EXISTS (SELECT 1 FROM [dbo].[Promotions] WHERE Title = N'Fruit Offer')
  INSERT INTO [dbo].[Promotions] (PromotionId, Title, Description, CategoryId, DiscountPercent, BannerImage, StartDate, EndDate, IsActive) VALUES (1, N'Fruit Offer', N'Save on selected fresh fruit picks for today.', 2, 20, N'/images/hero/organic-food.webp', '2026-01-01', '2026-12-31', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[Promotions] WHERE Title = N'Juice Offer')
  INSERT INTO [dbo].[Promotions] (PromotionId, Title, Description, CategoryId, DiscountPercent, BannerImage, StartDate, EndDate, IsActive) VALUES (2, N'Juice Offer', N'Save on selected natural juices and refreshing drinks.', 6, 10, N'/images/hero/juice-offer-1.webp', '2026-01-01', '2026-12-31', 1);
SET IDENTITY_INSERT [dbo].[Promotions] OFF;
GO

-- Demo user (login: demo@kosa.com / Kosa@123) - PBKDF2 hash verified against PasswordHasher format
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE Email = N'demo@kosa.com')
  INSERT INTO [dbo].[Users] (FullName, Email, Phone, PasswordHash, ProfileImage, IsActive, CreatedDate) VALUES (N'Kosa Demo User', N'demo@kosa.com', NULL, N'100000.R6e/fN1YivPDlg5CIYyvnA==.Zbj28wjhbYzGDjrrdSVMurpmvqGqdTKMYAdgWD7gwgA=', NULL, 1, GETDATE());
GO
COMMIT TRAN;
GO
PRINT 'Kosa seed data applied (idempotent).';
