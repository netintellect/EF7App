﻿using System.Diagnostics.CodeAnalysis;

namespace ServiceCruiser.Model.Validations.Core.Common.Utility
{
    /// <summary>
    /// Resolves string objects. 
    /// </summary>
    public interface IStringResolver
    {
        /// <summary>
        /// Returns a string represented by the receiver.
        /// </summary>
        /// <returns>The string object.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024", Justification = "May be computationally expensive.")]
        string GetString();
    }
}
