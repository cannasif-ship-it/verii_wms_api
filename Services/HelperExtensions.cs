using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
 
namespace WMS_WEBAPI.Services
{
    public class Filter
    {
        public string Column { get; set; } = string.Empty;
        public string Operator { get; set; } = "Equals";
        public string Value { get; set; } = string.Empty;
    }

    public class PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } = "Id";
        public string? SortDirection { get; set; } = "desc";
        public List<Filter>? Filters { get; set; } = new();
        public string FilterLogic { get; set; } = "and";
    }

    public static class QueryHelper
    {
        private static string ResolveColumnName(string column, IReadOnlyDictionary<string, string>? columnMapping)
        {
            if (columnMapping == null) return column;
            var mappingKey = columnMapping.Keys.FirstOrDefault(k => string.Equals(k, column, StringComparison.OrdinalIgnoreCase));
            return mappingKey != null ? columnMapping[mappingKey] : column;
        }

        private static (Expression expression, PropertyInfo property)? ResolvePropertyPath(Expression param, Type rootType, string path)
        {
            var parts = path.Split('.');
            Expression current = param;
            PropertyInfo? prop = null;
            Type currentType = rootType;

            foreach (var part in parts)
            {
                prop = currentType.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return null;
                current = Expression.Property(current, prop);
                currentType = prop.PropertyType;
            }

            return prop == null ? null : (current, prop);
        }

        public static IQueryable<T> ApplyFilters<T>(
            this IQueryable<T> query,
            List<Filter>? filters,
            string filterLogic = "and",
            IReadOnlyDictionary<string, string>? columnMapping = null)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "x");
            Expression? basePredicate = null;

            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty != null && (isDeletedProperty.PropertyType == typeof(bool) || isDeletedProperty.PropertyType == typeof(bool?)))
            {
                var isDeletedLeft = Expression.Property(param, isDeletedProperty);
                basePredicate = Expression.Equal(isDeletedLeft, Expression.Constant(false));
            }

            if (filters == null || filters.Count == 0)
            {
                if (basePredicate == null) return query;
                var defaultLambda = Expression.Lambda<Func<T, bool>>(basePredicate, param);
                return query.Where(defaultLambda);
            }

            bool useOr = string.Equals(filterLogic, "or", StringComparison.OrdinalIgnoreCase);
            Expression? filterPredicate = null;

            foreach (var filter in filters)
            {
                if (string.IsNullOrEmpty(filter.Value)) continue;

                var columnName = ResolveColumnName(filter.Column, columnMapping);
                var resolved = ResolvePropertyPath(param, typeof(T), columnName);
                if (resolved == null) continue;

                var (left, property) = resolved.Value;
                Expression? exp = null;
                var operatorLower = filter.Operator.ToLowerInvariant();

                if (property.PropertyType == typeof(string))
                {
                    var method = operatorLower switch
                    {
                        "contains" => typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                        "startswith" => typeof(string).GetMethod("StartsWith", new[] { typeof(string) }),
                        "endswith" => typeof(string).GetMethod("EndsWith", new[] { typeof(string) }),
                        _ => null
                    };
                    if (method != null)
                    {
                        exp = Expression.Call(left, method, Expression.Constant(filter.Value));
                    }
                    else
                    {
                        exp = Expression.Equal(left, Expression.Constant(filter.Value));
                    }
                }
                else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
                {
                    if (int.TryParse(filter.Value, out int val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
                {
                    if (long.TryParse(filter.Value, out long val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    if (decimal.TryParse(filter.Value, out decimal val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                {
                    if (DateTime.TryParse(filter.Value, out DateTime val))
                    {
                        exp = operatorLower switch
                        {
                            ">" or "gt" => Expression.GreaterThan(left, Expression.Constant(val)),
                            ">=" or "gte" => Expression.GreaterThanOrEqual(left, Expression.Constant(val)),
                            "<" or "lt" => Expression.LessThan(left, Expression.Constant(val)),
                            "<=" or "lte" => Expression.LessThanOrEqual(left, Expression.Constant(val)),
                            _ => Expression.Equal(left, Expression.Constant(val))
                        };
                    }
                }
                else if (property.PropertyType == typeof(bool) || property.PropertyType == typeof(bool?))
                {
                    if (bool.TryParse(filter.Value, out bool val))
                    {
                        exp = Expression.Equal(left, Expression.Constant(val));
                    }
                }
                else if (property.PropertyType.IsEnum)
                {
                    if (Enum.TryParse(property.PropertyType, filter.Value, true, out var enumVal))
                    {
                        exp = Expression.Equal(left, Expression.Constant(enumVal));
                    }
                }

                if (exp != null)
                {
                    filterPredicate = filterPredicate == null
                        ? exp
                        : useOr
                            ? Expression.OrElse(filterPredicate, exp)
                            : Expression.AndAlso(filterPredicate, exp);
                }
            }

            Expression? finalPredicate;
            if (basePredicate != null && filterPredicate != null)
            {
                finalPredicate = Expression.AndAlso(basePredicate, filterPredicate);
            }
            else
            {
                finalPredicate = basePredicate ?? filterPredicate;
            }

            if (finalPredicate == null) return query;
            var lambda = Expression.Lambda<Func<T, bool>>(finalPredicate, param);
            return query.Where(lambda);
        }

        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, bool desc)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                sortBy = "Id";
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var resolved = ResolvePropertyPath(parameter, typeof(T), sortBy);
            if (resolved == null)
            {
                resolved = ResolvePropertyPath(parameter, typeof(T), "Id");
                if (resolved == null) return query;
            }

            var (member, _) = resolved.Value;
            var keySelector = Expression.Lambda(
                typeof(Func<,>).MakeGenericType(typeof(T), member.Type),
                member,
                parameter
            );
            var methodName = desc ? "OrderByDescending" : "OrderBy";
            var call = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), member.Type },
                query.Expression,
                keySelector
            );
            return query.Provider.CreateQuery<T>(call);
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;
            int skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize);
        }

        public static IQueryable<T> ApplyPagedRequest<T>(this IQueryable<T> query, PagedRequest request, IReadOnlyDictionary<string, string>? columnMapping = null)
        {
            if (request == null) return query;

            query = query.ApplyFilters(request.Filters, request.FilterLogic, columnMapping);
            bool desc = string.Equals(request.SortDirection, "desc", StringComparison.OrdinalIgnoreCase)
                || string.Equals(request.SortDirection, "descending", StringComparison.OrdinalIgnoreCase);
            query = query.ApplySorting(request.SortBy ?? "Id", desc);
            query = query.ApplyPagination(request.PageNumber, request.PageSize);
            return query;
        }
    }
}
