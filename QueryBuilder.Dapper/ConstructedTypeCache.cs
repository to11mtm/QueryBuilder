using System;
using System.Linq;
using Dapper;
using System.Data;
using System.Collections.Concurrent;
using System.Reflection;

namespace QueryBuilder.Dapper
{
    public static class ConstructedTypeCache
    {
        public static string GetTableName<T>()
        {
            return typeof(T).Name;
        }
        public static ConcurrentDictionary<Type, string> SelectQueryCache = new ConcurrentDictionary<Type, string>();
           public static string GetSelectCols<T>()
        {
            
            return SelectQueryCache.GetOrAdd(typeof(T), (type) =>
            {
                var tableName = (typeof(T).Name);
                var members = type.GetMembers(BindingFlags.Instance | BindingFlags.Public).Where(q => q.MemberType == MemberTypes.Property || q.MemberType == MemberTypes.Field);
                return members.Select(mi => tableName + "." + mi.Name).Aggregate((left, right) => left + ", " + right);
            });
        }
    }
}
