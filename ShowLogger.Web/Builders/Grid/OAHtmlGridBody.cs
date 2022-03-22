using NonFactors.Mvc.Grid;

namespace ShowLogger.Web.Builders.Grid;

[Serializable]
public class OAHtmlGridBody<U>
    where U : class
{
    public string Name { get; set; }

    //public string Read { get; set; }

    public OAGridEditor Creator { get; set; }

    public OAGridEditor Editor { get; set; }

    public Action<IGridColumnsOf<U>> Columns { get; set; }
}