namespace ShowLogger.Web.Builders.Grid;

public class OaGridColumnCustomBuilder<T> where T : class
{
    public OAGridColumnBuilder<T> Parent { get; set; }

    public OaGridColumnCustomBuilder(OAGridColumnBuilder<T> parent)
    {
        Parent = parent;
    }

    public void Custom(string custom)
    {
        Parent.Column += custom;
    }

    public void Edit()
    {
        Parent.Column += $"<a role='button' class='oa_grid_button ss-grid-{Parent.Parent.Grid.Name}-edit fas fa-pencil-alt' onclick='oa_grid.open_editor(this)'></a>";
    }

    public void Custom(string name, string iconCss, string onClick)
    {
        Parent.Column += $"<a role='button' role='button' class='oa_grid_button ss-grid-{Parent.Parent.Grid.Name}-{name} {iconCss}' onclick='{onClick}(this)' ></a>";
    }
}