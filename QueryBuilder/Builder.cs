using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace QueryBuilder
{
    public class Builder<T> : IBuiltQueryProvider<T>
    {

        public Builder(string paramCharacter)
        {
            ParamCharacter = paramCharacter;
            Parameters = new List<Parameter>();
            SetClauses = new List<string>();
            WhereClauseBuilder = new WhereClauseBuilder<T>(paramCount, Parameters, paramCharacter);
            OrderByClauseBuilder = new OrderByClauseBuilder<T>();
            InsertClauseBuilder = new InsertClauseBuilder<T>(Parameters,paramCharacter);
        }
        public string ParamCharacter { get; protected set; }
        public int paramCount = 0;
        public List<string> SetClauses { get; protected set; }
        public List<Parameter> Parameters { get; protected set; } 
        public string WhereClauses { get { return WhereClauseBuilder.WhereClause.ToString(); } }
        public string OrderByClauses { get { return OrderByClauseBuilder.OrderByClause.ToString(); } }
        public WhereClauseBuilder<T> WhereClauseBuilder;
        public OrderByClauseBuilder<T> OrderByClauseBuilder;
        public InsertClauseBuilder<T> InsertClauseBuilder;
        public Builder<T> SetValue<TField>(Expression<Func<T, TField>> selector, TField setValue)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + paramCount;
            SetClauses.Add(string.Format("{0} = {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = setValue });
            paramCount = paramCount + 1;
            return this;
        }
        public Builder<T> Where(Action<WhereClauseBuilder<T>> builder)
        {
            builder(WhereClauseBuilder);
            paramCount = WhereClauseBuilder.ParamCount;
            return this;
        }
        public Builder<T>OrderBy(Action<OrderByClauseBuilder<T>> order)
        {
            order(OrderByClauseBuilder);
            return this;
        }
        public Builder<T> Insert(Action<InsertClauseBuilder<T>> insert)
        {
            insert(InsertClauseBuilder);
            return this;
        }
    }
}
