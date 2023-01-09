using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NonFactors.Mvc.Grid;
using ShowLogger.Web.Builders.Grid;
using System.Globalization;
using System.Linq.Expressions;

namespace ShowLogger.Web.Builders;
public static class OAGridBuilder
{
    //public static IHtmlGrid<T> SSGrid<T>(this IHtmlHelper html, ReadModel<T> model) 
    //    where T : class
    //{
    //    IHtmlGrid<T> grid = html.SSGrid(model.Data);

    //    if(!string.IsNullOrEmpty(model.Body.Name))
    //    {
    //        grid.NamedAndId(model.Body.Name);
    //    }

    //    if(model.Body.Creator != null)
    //    {
    //        grid.Creator(model.Body.Creator.Title, model.Body.Creator.PartialView, model.Body.Creator.UrlAction);
    //    }

    //    if (model.Body.Editor != null)
    //    {
    //        grid.Editor(model.Body.Editor.Title, model.Body.Editor.PartialView, model.Body.Editor.UrlAction);
    //    }

    //    if(model.Body.Columns != null)
    //    {
    //        grid.Build(model.Body.Columns);
    //    }

    //    return grid;
    //}

    public static IHtmlGrid<T> Columns<T>(this IHtmlGrid<T> grid, Action<IGridColumnsOf<T>> builder)
    {
        return grid.Build(builder)
            .Sortable()
            .Filterable();
    }

    public static IHtmlGrid<T> OAGrid<T>(this IHtmlHelper html, IEnumerable<T> source) where T : class
    {
        return html.Grid(source)
            .Pageable(pager =>
            {
                pager.PagesToDisplay = 5;
                pager.RowsPerPage = 10;
            })
            .Empty("No data found")
            .RowAttributed(m => new { model = JsonConvert.SerializeObject(m) })
            .HtmlAttributes(new { @class = "oa_grid_body" })
            ;
    }

    public static IHtmlGrid<T> Editor<T>(this IHtmlGrid<T> grid, string title, string? partialView, string? updateUrl, string updateFunc = "", string preUpdateFunc = "", string postUpdateFunc = "") where T : class
    {
        grid.Grid.Attributes.Add("editor_title", title);
        grid.Grid.Attributes.Add("editor_view", partialView);
        grid.Grid.Attributes.Add("editor_update_url", updateUrl);
        if (!string.IsNullOrEmpty(updateFunc))
        {
            grid.Grid.Attributes.Add("editor_func", updateFunc);
        }

        if (!string.IsNullOrEmpty(preUpdateFunc))
        {
            grid.Grid.Attributes.Add("pre_editor_func", preUpdateFunc);
        }

        if (!string.IsNullOrEmpty(postUpdateFunc))
        {
            grid.Grid.Attributes.Add("post_editor_func", postUpdateFunc);
        }

        return grid;
    }

    public static IHtmlGrid<T> CommandVisible<T>(this IHtmlGrid<T> grid, string commandVisibleFunc) where T : class
    {
        grid.Grid.Attributes.Add("command_visible_func", commandVisibleFunc);

        return grid;
    }

    public static IHtmlGrid<T> Creator<T>(this IHtmlGrid<T> grid, string title, string? partialView, string? createUrl, string createFunc = "", string preCreateFunc = "", string postCreateFunc = "") where T : class
    {
        grid.Grid.Attributes.Add("creator_title", title);
        grid.Grid.Attributes.Add("creator_view", partialView);
        grid.Grid.Attributes.Add("creator_create_url", createUrl);
        if (!string.IsNullOrEmpty(createFunc))
        {
            grid.Grid.Attributes.Add("creator_func", createFunc);
        }

        if (!string.IsNullOrEmpty(preCreateFunc))
        {
            grid.Grid.Attributes.Add("pre_creator_func", preCreateFunc);
        }

        if (!string.IsNullOrEmpty(postCreateFunc))
        {
            grid.Grid.Attributes.Add("post_creator_func", postCreateFunc);
        }

        return grid;
    }

    public static IHtmlGrid<T> IdProperty<T>(this IHtmlGrid<T> grid, string idProperty) where T : class
    {
        grid.Grid.Attributes.Add("id_prop", idProperty);

        return grid;
    }

    public static IHtmlGrid<T> Parent<T>(this IHtmlGrid<T> grid, string name, string valFunc) where T : class
    {
        grid.Grid.Attributes.Add("parent_name", name);
        grid.Grid.Attributes.Add("parent_val_func", valFunc);

        return grid;
    }

    public static IHtmlGrid<T> SubParent<T>(this IHtmlGrid<T> grid, string name, string value) where T : class
    {
        grid.Grid.Attributes.Add("sub_parent_name", name);
        grid.Grid.Attributes.Add("sub_parent_val", value);

        return grid;
    }

    public static IHtmlGrid<T> NamedAndId<T>(this IHtmlGrid<T> grid, string name) where T : class
    {
        grid.Named(name);
        grid.Id(name);

        return grid;
    }

    public static IHtmlGrid<T> HtmlAttributes<T>(this IHtmlGrid<T> grid, object htmlAttributes) where T : class
    {
        RouteValueDictionary attr = new RouteValueDictionary(htmlAttributes);

        foreach (var a in attr)
        {
            grid.Grid.Attributes.Add(a.Key, a.Value);
        }

        return grid;
    }

    public static IGridColumn<T, string> SSEdit<T>(this IGridColumnsOf<T> column) where T : class
    {
        return column.Add(m => $"<a role='button' class='oa_grid_button fas fa-pencil-alt' onclick='oa_grid.open_editor(this)'></a>").Encoded(false);
    }

    public static IGridColumnsOf<T> Commands<T>(this IGridColumnsOf<T> column, Action<OaGridColumnCustomBuilder<T>> customAction)
        where T : class
    {
        OAGridColumnBuilder<T> columnBuilder = new OAGridColumnBuilder<T>(column);
        customAction(new OaGridColumnCustomBuilder<T>(columnBuilder));

        column.Add(m => $"{columnBuilder.Column}").Encoded(false).AppendCss($"oa_column_commands {column.Grid.Name}_Commands");


        return column;
    }

    public static IGridColumn<T, TValue> OAColumn<T, TValue>(this IGridColumnsOf<T> column, Expression<Func<T, TValue>> expression)
        where T : class
    {
        return column.Add(expression).SetCssClass();
    }

    public static IGridColumn<T, TValue> SetCssClass<T, TValue>(this IGridColumn<T, TValue> column)
        where T : class
    {
        TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

        string name = myTI.ToTitleCase(column.Name).Replace("-", String.Empty);
        column.AppendCss($"{column.Grid.Name}_{name}");
        return column;
    }
}

