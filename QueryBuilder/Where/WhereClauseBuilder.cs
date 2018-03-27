using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder
{
    public class WhereClauseBuilder<T>
    {
        public int ParamCount { get; set; }
        public StringBuilder WhereClause = new StringBuilder();
        public List<Parameter> Parameters;
        public string ParamCharacter { get; protected set; }
        public WhereClauseBuilder(int paramCount, List<Parameter> paramList, string paramCharacter)
        {
            ParamCount = paramCount;
            Parameters = paramList;
            ParamCharacter = paramCharacter;
        }
        public WhereClauseBuilder<T> Where<TField>(Expression<Func<T, TField>> selector, TField setValue)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} = {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = setValue });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> BeginsWith<TField>(Expression<Func<T, TField>> selector, string beginsVal)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} like {1} + '%' ", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = beginsVal });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> EndsWith<TField>(Expression<Func<T, TField>> selector, string endsVal)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} like '%' + {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = endsVal });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> AnywhereInside<TField>(Expression<Func<T, TField>> selector, string insideVal)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} like '%' + {1} + '%' ", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = insideVal });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> GreaterThan<TField>(Expression<Func<T, TField>> selector, string greaterThan)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} > {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = greaterThan });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> GreaterThanOrEquals<TField>(Expression<Func<T, TField>> selector, string greaterThan)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} >= {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = greaterThan });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> LessThan<TField>(Expression<Func<T, TField>> selector, string lessThan)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} < {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = lessThan });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> LessThanOrEquals<TField>(Expression<Func<T, TField>> selector, string lessThan)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} <= {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = lessThan });
            ParamCount = ParamCount + 1;
            return this;
        }
        public WhereClauseBuilder<T> In<TField>(Expression<Func<T, TField>> selector, TField[] values)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            WhereClause.Append(string.Format("{0} in {1}", fieldName, paramName));
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = values });
            ParamCount = ParamCount + 1;
            return this;
        }

        public WhereClauseBuilder<T> And(Action builder)
        {
            WhereClause.Append(" and (");
            builder();
            WhereClause.Append(")");
            return this;
        }
        public WhereClauseBuilder<T> Or(Action builder)
        {
            WhereClause.Append(" or (");
            builder();
            WhereClause.Append(")");
            return this;
        }

        //Because we are playing hacky games with C# rules, let's not let our users accidentally trip themselves up.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public WhereClauseBuilder<T> and
        {
            get
            {
                WhereClause.Append(" and ");
                return this;
            }
        }
        //Because we are playing hacky games with C# rules, let's not let our users accidentally trip themselves up.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public WhereClauseBuilder<T> or
        {
            get
            {
                WhereClause.Append(" or ");
                return this;
            }
        }
    }
}
