using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder
{
    public class OrderByClauseBuilder<T>
    {
        public List<OrderByClause> Clauses { get; protected set; }
        public StringBuilder OrderByClause { get; protected set; }
        public OrderByClauseBuilder()
        {
            Clauses = new List<OrderByClause>();
            OrderByClause = new StringBuilder();
        }
        public OrderByClauseBuilder<T> By<TKey>(Expression<Func<T, TKey>> outerKeySelector, OrderByDirectionEnum direction)
        {
            var fieldName = Helpers.GetName(outerKeySelector);
            var clause = new OrderByClause() { Direction = direction == OrderByDirectionEnum.Asc ? "ASC" : "DESC", FieldName = fieldName };
            Clauses.Add(clause);
            OrderByClause.Append(string.Format("order by {0} {1}", clause.FieldName, clause.Direction));
            return this;
        }
        public OrderByClauseBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> outerKeySelector, OrderByDirectionEnum direction)
        {
            var fieldName = Helpers.GetName(outerKeySelector);
            var clause = new OrderByClause() { Direction = direction == OrderByDirectionEnum.Asc ? "ASC" : "DESC", FieldName = fieldName };
            Clauses.Add(clause);
            OrderByClause.Append(string.Format(", then by {0} {1}", clause.FieldName, clause.Direction));
            return this;
        }
    }
}
