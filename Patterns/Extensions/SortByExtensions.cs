// <copyright file="StartAndEndDate.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

namespace Patterns.Extensions
{

    public static class SortByExtensions
    {
        /// <summary>
        /// Get the sort order is ascending.
        /// </summary>
        /// <param name="sort"></param>
        /// <returns>Boolean - true list is ascending, false list is descending.</returns>
        public static bool IsAscending(this SortBy sort)
        {
            return sort.HasFlag(SortBy.Ascending);
        }

        /// <summary>
        /// Return simplified sort order (Name, Price or Populariry)
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static SortBy Simplified(this SortBy sort)
        {
            SortBy result = SortBy.Price;

            if (sort.HasFlag(SortBy.Price))
            {
                result = SortBy.Price;
            }
            else if (sort.HasFlag(SortBy.Name))
            {
                result = SortBy.Name;
            }
            else
            {
                result = SortBy.Popularity;
            }

            return result;
        }
    }
}
