﻿using System.Diagnostics.CodeAnalysis;

namespace RecipePlannerApi.Dao.Request {
    /// <summary>
    ///  Id data transfer object
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IdDto {

        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }
    }
}
