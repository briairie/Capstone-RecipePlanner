﻿using Moq;
using RecipePlannerApi.Api.Interface;
using RecipePlannerApi.Dao.Interface;
using RecipePlannerApi.Model;
using RecipePlannerApi.Service.Interface;
using RecipePlannerApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipePlannerApiTests.TestServices.TestRecipeService {
    public class TestBuyIngredients {
        [Fact]
        public void TestValidRequest() {
            var userService = new Mock<IUserService>();
            var ingredientDao = new Mock<IIngredientDao>();
            var recipeApi = new Mock<IRecipeApi>();
            var measurementService = new Mock<IMeasurementService>();
            var shoppingService = new Mock<IShoppingListService>();

            userService.Setup(x => x.GetUserPantry(It.IsAny<int>()))
                .Returns(new List<PantryItem> { new PantryItem { IngredientName = "apple", Quantity = 5, UnitId = AppUnit.NONE }, new PantryItem { IngredientName = "pear", Quantity = 5, UnitId = AppUnit.NONE } });
            shoppingService.Setup(x => x.DeleteAllFromShoppingList(It.IsAny<List<ShoppingListIngredient>>()));
            userService.Setup(x => x.UpdatePantryItems(It.IsAny<List<PantryItem>>(), It.IsAny<int>())).Returns(new List<PantryItem>());
            var service = new RecipeService(userService.Object, ingredientDao.Object, recipeApi.Object, measurementService.Object, shoppingService.Object);

            var ingredients = new List<ShoppingListIngredient>() { new ShoppingListIngredient { IngredientId = 1 }, new ShoppingListIngredient { IngredientName = "pear" }, new ShoppingListIngredient() };

            var result = service.BuyIngredients(ingredients, 1);

            Assert.NotNull(result);
        }
    }
}
