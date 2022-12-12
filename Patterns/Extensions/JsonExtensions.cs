// <copyright file="JsonExtensions.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System.Text.Json;

namespace Patterns.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson(this object me)
        {
            return JsonSerializer.Serialize(me);
        }

        /// <summary>
        /// Convert a string JSON to an object.
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T">The return type (interfaces are not supported).</typeparam>
        /// <returns>An object.</returns>
        /// <remarks>The return type must not be an interface.</remarks>
        public static T JsonConvert<T>(this string value)
            where T : class
            => JsonSerializer.Deserialize<T>(value) ?? Activator.CreateInstance<T>();
    }
}
