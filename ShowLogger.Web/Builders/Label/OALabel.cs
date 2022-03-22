using System.Linq.Expressions;

namespace ShowLogger.Web.Builders.Label;

public class OALabel<T, TValue>
{
    public Expression<Func<T, TValue>> Column { get; set; }

    public object HtmlAttributes { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }
}
