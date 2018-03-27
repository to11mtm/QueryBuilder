using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace QueryBuilder
{
    

    public class Helpers
    {
        public static string GetName<T, TField>(Expression<Func<T, TField>> field)
        {
            return GetMemberInfo(field).Name;
        }


        public static MemberInfo GetMemberInfo<TSource, TField>(Expression<Func<TSource, TField>> Field)
        {
            return (Field.Body as MemberExpression ?? ((UnaryExpression)Field.Body).Operand as MemberExpression).Member;
        }
    }
}
