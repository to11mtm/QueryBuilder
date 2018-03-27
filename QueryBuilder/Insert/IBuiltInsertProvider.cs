using System.Collections.Generic;

namespace QueryBuilder
{
    public interface IBuiltInsertProvider<T>
    {
        string Columns { get;  }
        string ValueParams { get; }
        List<Parameter> Parameters { get; }
    }
}
