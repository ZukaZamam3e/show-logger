namespace ShowLogger.Web.Builders.Grid;

public class OAGridButtonBuilder<T>
    where T : class
{
    OAGridPageBuilder<T> Parent { get; set; }

    public OAGridButtonBuilder(OAGridPageBuilder<T> parent)
    {
        Parent = parent;
    }

    public void Creator()
    {
        Parent.Container.HeaderButtons.Add(new OAHtmlGridButton
        {
            CssClasses = "oa_grid_button fas fa-plus",
            Click = $"oa_grid.open_creator('{Parent.Container.Name}')"
        });
    }

    public void Refresh()
    {
        Parent.Container.HeaderButtons.Add(new OAHtmlGridButton
        {
            CssClasses = "oa_grid_button fas fa-sync-alt",
            Click = $"oa_grid.reload_grid('{Parent.Container.Name}')"
        });
    }

    public void Custom(string cssClasses, string eventName, object htmlAttributes = null)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        RouteValueDictionary attr = new RouteValueDictionary(htmlAttributes);

        foreach (var a in attr)
        {
            dictionary.Add(a.Key, a.Value.ToString());
        }

        Parent.Container.HeaderButtons.Add(new OAHtmlGridButton
        {
            CssClasses = $"oa_grid_button {cssClasses}",
            Click = eventName,
            HtmlAttributes = dictionary
        });
    }
}