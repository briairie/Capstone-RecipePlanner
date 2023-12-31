﻿CREATE PROCEDURE [dbo].[create_recipe]
	@apiId INT,
	@title NVARCHAR(150),
	@imageUrl NVARCHAR(150),
	@imageType NVARCHAR(50)
AS
	INSERT INTO [dbo].[recipe] (api_id, title, image_url, image_type) VALUES (@apiId, @title, @imageUrl, @imageType)
	DECLARE @recipeId INT = SCOPE_IDENTITY()
	EXEC [dbo].[get_recipe] @recipeId
	return @recipeId

