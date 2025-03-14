﻿namespace CompetitiveTennis.Tournaments.Extensions;

using Contracts;

public static class EnumerableExtensions
{
    public static IEnumerable<T> PageFilterResult<T>(this IEnumerable<T> collection, PageQuery query)
        => collection != null ? 
            collection
                .Skip((query.Page - 1) * query.ItemsPerPage)
                .Take(query.ItemsPerPage) :
            new List<T>();
    
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        => collection is null || !collection.Any();
}