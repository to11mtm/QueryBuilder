using System;
using System.Collections.Generic;

namespace QueryBuilder
{
    public interface IJoinBuiltQueryProvider<T1, T2,TJoin>
    {
        string WhereClause { get; set; }
        string JoinClause { get; set; }
        List<Parameter> Parameters { get; set; }
        Func<T1, TJoin> OuterKeySelector { get; set; }
        Func<T2, TJoin> InnerKeySelector { get; set; }
    }
}
