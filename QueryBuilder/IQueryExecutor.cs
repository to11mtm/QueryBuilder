using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder
{
    public interface IQueryExecutor
    {
        IEnumerable<T> ExecuteQuery<T>(IBuiltQueryProvider<T> builtQueryProvider);
        int ExecuteUpdate<T>(IBuiltQueryProvider<T> builtQueryProvider);
        TReturn ExecuteInsert<T, TReturn>(IBuiltInsertProvider<T> builtInsertProvider, Expression<Func<T, TReturn>> idSelector);
        T ExecuteInsertWithFullObjectState<T, TReturn>(IBuiltInsertProvider<T> builtInsertProvider, Expression<Func<T, TReturn>> idSelector);
        IDictionary<T1, IEnumerable<T2>> ExecuteOneToManyJoinQuery<T1, T2, TJoin>(IJoinBuiltQueryProvider<T1, T2, TJoin> joinBuiltQueryProvider);
        IDictionary<T1, T2> ExecuteOneToOneJoinQuery<T1, T2, TJoin>(IJoinBuiltQueryProvider<T1, T2, TJoin> joinBuiltQueryProvider);
    }
}
