using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.Common;
using System.Reflection;
using System.Linq.Expressions;

namespace QueryBuilder.Dapper
{
    public abstract class DapperQueryExecutor : IQueryExecutor
    {
        public DbConnection Connection { get;protected set;}
        /*public TReturn ExecuteInsert<T,TReturn>(IBuiltInsertProvider<T> builtInsertProvider)
        {
            var executedSql = string.Format("insert into {0} ({1}) values ({2}", ConstructedTypeCache.GetTableName<T>(), builtInsertProvider.Columns, builtInsertProvider.ValueParams);
            return InsertWithIDReturnImpl<T,TReturn>(executedSql, builtInsertProvider);
        }*/

        public IEnumerable<TReturn> ExecuteSelect<T, TReturn>(IBuiltQueryProvider<T> builtQueryProvider, Expression<Func<T, TReturn>> projection)
        {
            var projectionSelectCols = new StringBuilder();
            var expr = (projection.Body as NewExpression);
            var count = expr.Arguments.Count;
            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    projectionSelectCols.Append(" , ");
                }
                projectionSelectCols.AppendFormat("{0} as {1}", (expr.Arguments[i] as MemberExpression).Member.Name,
                (expr.Members[i] as MemberInfo).Name);
            }
            var executedSql = string.Format("select {0} from {1} where {2} {3}", projectionSelectCols.ToString(), ConstructedTypeCache.GetTableName<T>(), builtQueryProvider.WhereClauses, builtQueryProvider.OrderByClauses);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var param in builtQueryProvider.Parameters)
            {
                dynamicParameters.Add(param.ParameterName, param.ParameterValue);
            }
            return Connection.Query<TReturn>(executedSql, dynamicParameters);
        }
        public TReturn ExecuteInsert<T, TReturn>(IBuiltInsertProvider<T> builtInsertProvider, Expression<Func<T,TReturn>> idSelector)
        {
            var executedSql = string.Format("insert into {0} ({1}) values ({2}", ConstructedTypeCache.GetTableName<T>(), builtInsertProvider.Columns, builtInsertProvider.ValueParams);
            return InsertWithIDReturnImpl(executedSql,idSelector, builtInsertProvider);
        }
        public T ExecuteInsertWithFullObjectState<T, TReturn>(IBuiltInsertProvider<T> builtInsertProvider, Expression<Func<T, TReturn>> idSelector)
        {
            var executedSql = string.Format("insert into {0} ({1}) values ({2}", ConstructedTypeCache.GetTableName<T>(), builtInsertProvider.Columns, builtInsertProvider.ValueParams);
            return InsertWithFullObjectReturnImpl(executedSql, idSelector, builtInsertProvider);
        }
        public abstract TReturn InsertWithIDReturnImpl<T, TReturn>(string sqlToExecute,Expression<Func<T,TReturn>> idSelector, IBuiltInsertProvider<T> builtInsertProvider);
        //public abstract TReturn InsertWithIDReturnImpl<T, TReturn>(string sqlToExecute, IBuiltInsertProvider<T> builtInsertProvider);
        public abstract T InsertWithFullObjectReturnImpl<T,TReturn>(string sqlToExecute, Expression<Func<T,TReturn>> idSelector, IBuiltInsertProvider<T> builtInsertProvider);
        public IEnumerable<T> ExecuteQuery<T>(IBuiltQueryProvider<T> builtQueryProvider)
        {
            var executedSql = string.Format("select {0} from {1} where {2} {3}", ConstructedTypeCache.GetSelectCols<T>(), ConstructedTypeCache.GetTableName<T>(), builtQueryProvider.WhereClauses,builtQueryProvider.OrderByClauses);
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var param in builtQueryProvider.Parameters)
            {
                dynamicParameters.Add(param.ParameterName, param.ParameterValue);
            }
            return Connection.Query<T>(executedSql, dynamicParameters);
        }

        public IDictionary<T1, T2> ExecuteOneToOneJoinQuery<T1,T2,TJoin>(IJoinBuiltQueryProvider<T1,T2,TJoin> joinBuiltQueryProvider)
        {
            SqlMapper.GridReader reader = buildMultiReader(joinBuiltQueryProvider);
            var t1 = reader.Read<T1>();
            var t2 = reader.Read<T2>();
            return t1.Join(t2, joinBuiltQueryProvider.OuterKeySelector, joinBuiltQueryProvider.InnerKeySelector, (left, right) => new KeyValuePair<T1, T2>(left, right)).ToDictionary(q => q.Key, q => q.Value);
        }

        public IDictionary<T1,IEnumerable<T2>> ExecuteOneToManyJoinQuery<T1,T2,TJoin>(IJoinBuiltQueryProvider<T1,T2,TJoin> joinBuiltQueryProvider)
        {
            SqlMapper.GridReader reader = buildMultiReader(joinBuiltQueryProvider);

            var t1 = reader.Read<T1>();
            var t2 = reader.Read<T2>();
            return t1.GroupJoin(t2,
                joinBuiltQueryProvider.OuterKeySelector,
                joinBuiltQueryProvider.InnerKeySelector,
                (left, right) => new KeyValuePair<T1, IEnumerable<T2>>(left, right)).ToDictionary(q => q.Key, q => q.Value);
        }

        private SqlMapper.GridReader buildMultiReader<T1, T2, TJoin>(IJoinBuiltQueryProvider<T1, T2, TJoin> joinBuiltQueryProvider)
        {
            var executedSql = string.Format(
                            "select {0} from {1} join {2} on {3} where {4}; " +
                            "select {5} from {1} join {2} on {3} where {4}",
                            ConstructedTypeCache.GetSelectCols<T1>(),
                            ConstructedTypeCache.GetTableName<T1>(),
                            ConstructedTypeCache.GetTableName<T2>(),
                            joinBuiltQueryProvider.JoinClause,
                            joinBuiltQueryProvider.WhereClause,
                            ConstructedTypeCache.GetSelectCols<T2>());
            DynamicParameters dynamicParameters = new DynamicParameters();
            foreach (var param in joinBuiltQueryProvider.Parameters)
            {
                dynamicParameters.Add(param.ParameterName, param.ParameterValue);
            }
            var reader = Connection.QueryMultiple(executedSql, dynamicParameters);
            return reader;
        }

        public int ExecuteUpdate<T>(IBuiltQueryProvider<T> builtQueryProvider)
        {
            var executedSwl =
                string.Format("update {0} set {1} where {2}",
                ConstructedTypeCache.GetTableName<T>(),
                builtQueryProvider.SetClauses.Aggregate((left, right) => left + ", " + right),
                builtQueryProvider.WhereClauses);
            return 0;
        }
    }
}
