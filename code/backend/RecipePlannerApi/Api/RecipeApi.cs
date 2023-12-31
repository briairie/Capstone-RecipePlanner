﻿using com.spoonacular;
using Org.OpenAPITools.Model;
using RecipePlannerApi.Api.Interface;
using RecipePlannerApi.Api.Requests;
using RecipePlannerApi.Model;

namespace RecipePlannerApi.Api;

/// <summary>Wrapper class for the spoonacular api</summary>
public class RecipeApi : IRecipeApi
{
    #region Data members

    private readonly IRecipesApi api;

    #endregion

    #region Constructors

    /// <summary>Initializes the <see cref="RecipeApi" /> class.</summary>
    public RecipeApi(IRecipesApi api)
    {
        this.api = api;
    }

    #endregion

    #region Methods

    /// <summary>Searches the recipes by ingredients.</summary>
    /// <param name="request">The request.</param>
    /// <returns>
    ///     Responce from api
    ///     <br />
    /// </returns>
    public List<SearchRecipesByIngredients200ResponseInner> SearchRecipesByIngredients(
        SearchRecipesByIngredientsRequest request)
    {
        return this.api.SearchRecipesByIngredients(request.ingredients, request.number, request.limitLicense,
            request.ranking, request.ignorePantry);
    }

    /// <summary>Gets the recipe information.</summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <returns>
    ///     <para>The information for the recipe</para>
    /// </returns>
    public RecipeInformation GetRecipeInformation(int recipeId)
    {
        var info = this.api.GetRecipeInformation(recipeId, false);
        var ingredients = new List<Ingredient>();
        var instructions = this.GetRecipeInstructions(recipeId);
        foreach (var item in info?.ExtendedIngredients)
        {
            var name = item.Name?.Length > 40 ? item.Name[..40] : item.Name;

            var ingredient = new Ingredient
            {
                IngredientId = item.Id,
                IngredientName = name,
                Quantity = (int)Math.Ceiling(item.Amount.Value),
                Unit = item.Unit
            };
            ingredients.Add(ingredient);
        }

        return new RecipeInformation
        {
            Summary = info.Summary,
            Ingredients = ingredients,
            Steps = instructions,
            Image = info.Image,
            ImageType = info.ImageType
        };
    }

    /// <summary>Gets the recipe instructions.</summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <returns>
    ///     <para>The instructionsfor the recipe</para>
    /// </returns>
    public List<RecipesStep> GetRecipeInstructions(int recipeId)
    {
        var analyzedInsturctions = this.api.GetAnalyzedRecipeInstructions(recipeId, false).FirstOrDefault();
        var instructions = new List<RecipesStep>();
        if (analyzedInsturctions != null)
        {
            foreach (var step in analyzedInsturctions?.Steps)
            {
                instructions.Add(new RecipesStep
                {
                    stepNumber = (int)step.Number,
                    instructions = step.Step
                });
            }
        }

        return instructions;
    }

    /// <summary>
    ///     Searches for the specified amount of recipes per page that match the filters in the request taking into
    ///     account the ingredients the passed ingredients
    /// </summary>
    /// <param name="request">The request to browse recipes.</param>
    /// <param name="ingredients">The ingredients to inform what which recipes to bring back.</param>
    /// <param name="perPage">The amount of recipes to return per page.</param>
    /// <returns>
    ///     <br />
    /// </returns>
    public BrowseRecipeResponse BrowseRecipes(BrowseRecipeRequest request, string ingredients, int perPage)
    {
        var searchRequest = new SearchRecipeRequest
        {
            query = request.Query,
            cuisine = request.Cuisine,
            diet = request.Diet,
            type = request.Type,
            offset = request.PageNumber * perPage,
            limitLicense = false,
            sort = "min-missing-ingredients",
            addRecipeInformation = false,
            number = perPage
        };

        var response = this.api.SearchRecipes(searchRequest);
        var recipes = new List<Recipe>();

        foreach (var item in response.Results)
        {
            recipes.Add(new Recipe
            {
                ApiId = item.Id,
                Image = item.Image,
                ImageType = item.ImageType,
                Title = item.Title
            });
        }

        return new BrowseRecipeResponse
        {
            recipes = recipes,
            page = response.Offset / perPage,
            totalNumberOfRecipes = response.TotalResults
        };
    }

    /// <summary>Converts measurement amounts with the Spoonacular api.</summary>
    /// <param name="request">The request to convert measurement amounts.</param>
    /// <returns>The conversion result if the is one</returns>
    public decimal? ConvertAmount(ConvertAmountRequest request)
    {
        var response = this.api.ConvertAmounts(request.IngredientName, request.SourceAmount, request.SourceUnit,
            request.TargetUnit);

        return response.TargetAmount;
    }

    /// <summary>Gets the recipe information.</summary>
    /// <param name="recipeId">The recipe identifier.</param>
    /// <returns>
    ///     <para>The information for the recipe</para>
    /// </returns>
    public List<Ingredient> GetRecipeIngredientsBulk(List<int> recipeIds)
    {
        var ids = string.Join(",", recipeIds);
        var bulkInfo = this.api.GetRecipeInformationBulk(ids, false);

        var ingredients = new List<Ingredient>();
        foreach (var info in bulkInfo)
        {
            foreach (var item in info?.ExtendedIngredients)
            {
                var existingIngredient = ingredients.Find(i => i.IngredientId == item.Id || i.IngredientName.Equals(item.Name));
                if (existingIngredient != null)
                {
                    existingIngredient.Quantity += (int)Math.Ceiling(item.Amount.Value);
                }
                else
                {
                    var name = item.Name?.Length > 40 ? item.Name[..40] : item.Name;
                    var ingredient = new Ingredient
                    {
                        IngredientId = item.Id,
                        IngredientName = name,
                        Quantity = (int)Math.Ceiling(item.Amount.Value),
                        Unit = item.Unit
                    };
                    ingredients.Add(ingredient);
                }
            }
        }

        return ingredients;
    }

    #endregion
}