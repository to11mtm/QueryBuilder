using System.Collections.Generic;

namespace QueryBuilder
{
    public interface IBuiltQueryProvider<T>
    {
        List<string> SetClauses { get; }
        List<Parameter> Parameters { get; }
        string WhereClauses { get; }
        string OrderByClauses { get; }
    }
}
