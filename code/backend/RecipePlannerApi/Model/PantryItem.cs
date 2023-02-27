﻿using System.Diagnostics.CodeAnalysis;

namespace RecipePlannerApi.Model {
    [ExcludeFromCodeCoverage]
    public class PantryItem {
        public int? PantryId { get; set; }
        public int? UserId { get; set; }
        public int? IngredientId { get; set; }
        public string IngredientName { get; set; }
        public int Quantity { get; set; }
        public AppUnit Unit { get; set; }
    }
}
