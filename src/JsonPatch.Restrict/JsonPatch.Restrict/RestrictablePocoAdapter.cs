using Microsoft.AspNetCore.JsonPatch.Internal;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JsonPatch.Restrict
{
    /// <summary>
    /// An implementation of <see cref="PocoAdapter"/> that checks against a set of properties to determine if a property is writable or not.
    /// </summary>
    public class RestrictablePocoAdapter : PocoAdapter
    {
        private readonly HashSet<string> _allowedProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestrictablePocoAdapter"/> class.
        /// </summary>
        /// <param name="allowedProperties">A <see cref="HashSet{T}"/> containing all properties which are allowed to be changed by JSON Patch.</param>
        public RestrictablePocoAdapter(HashSet<string> allowedProperties)
        {
            _allowedProperties = allowedProperties;
        }

        /// <inheritdoc/>
        protected override bool TryGetJsonProperty(object target, IContractResolver contractResolver, string segment, out JsonProperty jsonProperty)
        {
            if (contractResolver.ResolveContract(target.GetType()) is JsonObjectContract jsonObjectContract)
            {
                var pocoProperty = jsonObjectContract
                    .Properties
                    .FirstOrDefault(p => string.Equals(p.PropertyName, segment, StringComparison.OrdinalIgnoreCase));

                if (pocoProperty != null &&
                    _allowedProperties.Any(a =>
                        a.Equals(pocoProperty.PropertyName, StringComparison.OrdinalIgnoreCase)
                        || a.StartsWith($"{pocoProperty.PropertyName}/", StringComparison.OrdinalIgnoreCase)
                        || a.EndsWith($"/{pocoProperty.PropertyName}", StringComparison.OrdinalIgnoreCase)
                        || a.Contains($"/{pocoProperty.PropertyName}/",  StringComparison.OrdinalIgnoreCase)))
                {
                    jsonProperty = pocoProperty;
                    return true;
                }
            }

            jsonProperty = null;
            return false;
        }
    }
}
