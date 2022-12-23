// <copyright file="JsonExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Patterns.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object me)
        {
            return JsonSerializer.Serialize(me, JsonExtensions.JsonSerializerOptions);
        }

        public static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions()
        {
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.Strict,
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true

        };

        /// <summary>
        /// Convert a string JSON to an object.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T">The return type (interfaces are not supported).</typeparam>
        /// <returns>An object.</returns>
        /// <remarks>The return type must not be an interface.</remarks>
        public static T JsonConvert<T>(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new JsonException("Cannot convert an empty string!");
            }
            else
            {
                return JsonSerializer.Deserialize<T>(value, JsonExtensions.JsonSerializerOptions) ?? Activator.CreateInstance<T>();
            }            
        }
            
    }
}
