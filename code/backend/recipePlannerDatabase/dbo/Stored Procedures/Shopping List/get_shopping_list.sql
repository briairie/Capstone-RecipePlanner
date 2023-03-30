CREATE PROCEDURE [dbo].[get_shopping_list]
  @userId INT
AS
  SELECT 
		 [shopping_list_id]   AS ShoppingListId
		,[user_id]			  AS UserId
		,[ingredient_name]    AS IngredientName
		,[quantity]           AS Quantity
		,[unit_id]            AS UnitId
  FROM [dbo].[shopping_list] 
  WHERE [user_id] = @userId
