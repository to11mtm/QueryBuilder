using System;
using System.Threading.Tasks;
using Dapper;
using System.Linq.Expressions;

namespace QueryBuilder.Dapper
{
    public class SqlServerDapperQueryExecutor : DapperQueryExecutor
    {
        public override T InsertWithFullObjectReturnImpl<T,TReturn>(string sqlToExecute, Expression<Func<T,TReturn>> idSelector, IBuiltInsertProvider<T> builtInsertProvider)
        {
            var idCol = Helpers.GetName(idSelector);
            var withObjReturn = sqlToExecute + "\r\n" + "select " + ConstructedTypeCache.GetSelectCols<T>() + " from " + ConstructedTypeCache.GetTableName<T>() + " where " + idCol + " = SCOPE_IDENTITY();";
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var param in builtInsertProvider.Parameters)
            {
                dynamicParameters.Add(param.ParameterName, param.ParameterValue);
            }
            return Connection.QueryFirstOrDefault(withObjReturn);
        }

        public TReturn InsertWithIDReturnImpl<T, TReturn>(string sqlToExecute, IBuiltInsertProvider<T> builtInsertProvider)
        {
            var withIDReturn = sqlToExecute + "\r\n SELECT SCOPE_IDENTITY()";
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var param in builtInsertProvider.Parameters)
            {
                dynamicParameters.Add(param.ParameterName, param.ParameterValue);
            }
            return Connection.ExecuteScalar<TReturn>(withIDReturn, dynamicParameters);
        }

        public override TReturn InsertWithIDReturnImpl<T, TReturn>(string sqlToExecute, Expression<Func<T, TReturn>> idSelector, IBuiltInsertProvider<T> builtInsertProvider)
        {
            return InsertWithIDReturnImpl<T,TReturn>(sqlToExecute, builtInsertProvider);
        }
    }
}
