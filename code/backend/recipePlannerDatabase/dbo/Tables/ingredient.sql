﻿CREATE TABLE [dbo].[ingredient]
(
	[ingredient_id] INT NOT NULL, 
    [ingredient_name] NVARCHAR(40) NOT NULL,
	PRIMARY KEY(ingredient_id, ingredient_name),
	UNIQUE(ingredient_name)
)
