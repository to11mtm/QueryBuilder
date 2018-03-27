using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder
{
    public class JoinClauseBuilder<T1,T2,TJoin> : IJoinBuiltQueryProvider<T1,T2,TJoin>
    {

        public string FirstTable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string SecondTableSelectCols { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string WhereClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string JoinClause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Parameter> Parameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Func<T1, TJoin> OuterKeySelector { get; set;}
        public Func<T2, TJoin> InnerKeySelector { get; set; }

        public JoinClauseBuilder<T1,T2,TJoin> JoinOn(Expression<Func<T1, TJoin>> outerKeySelector, Expression<Func<T2,TJoin>> innerKeySelector)
        {
            var outer = Helpers.GetName(outerKeySelector);
            var inner = Helpers.GetName(innerKeySelector);
            OuterKeySelector = outerKeySelector.Compile();
            InnerKeySelector = innerKeySelector.Compile();
            JoinClause = outer + " = " + inner;
            return this;
        }
    }
}
