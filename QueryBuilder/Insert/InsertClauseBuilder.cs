using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace QueryBuilder
{
    public class InsertClauseBuilder<T>: IBuiltInsertProvider<T>
    {
        public List<Parameter> Parameters { get; protected set; }
        public string ParamCharacter;
        public StringBuilder ColBuilder;
        public StringBuilder ParamValueBuilder;
        public int ParamCount { get; protected set; }
        public string Columns { get { return ColBuilder.ToString(); } }
        public string ValueParams { get { return ParamValueBuilder.ToString(); } }

        public InsertClauseBuilder(List<Parameter> parameters, string paramCharacter)
        {
            ParamCount = 0;
            Parameters = parameters;
            ParamCharacter = paramCharacter;
            ParamValueBuilder = new StringBuilder();
            ColBuilder = new StringBuilder();
        }
        public InsertClauseBuilder<T> set<TField>(Expression<Func<T, TField>> selector, TField setValue)
        {
            var fieldName = Helpers.GetMemberInfo(selector).Name;
            var paramName = ParamCharacter + fieldName + ParamCount;
            if (ParamCount == 0)
            {
                ColBuilder.Append(string.Format("{0}", fieldName));
                ParamValueBuilder.Append(string.Format("{0}", paramName));
            }
            else
            {
                ColBuilder.Append(string.Format(", {0}", fieldName));
                ParamValueBuilder.Append(string.Format(", {0}", paramName));
            }
            Parameters.Add(new Parameter() { ParameterName = paramName, ParameterValue = setValue });
            ParamCount = ParamCount + 1;
            return this;
        }
    }
}
