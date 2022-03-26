using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShowLogger.Web.Builders.Grid;

public class OAGridPageBuilder<T> : OAControlBuilder<T>
    where T : class
{
    public OAHtmlGrid<T> Container;

    public OAGridPageBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OAHtmlGrid<T>();
    }

    public OAGridPageBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OAGridPageBuilder<T> Read(string? read)
    {
        Container.Read = read;
        return this;
    }

    public OAGridPageBuilder<T> Init(string init)
    {
        Container.Init = init;
        return this;
    }

    public OAGridPageBuilder<T> ServerFilter(bool serverFilter)
    {
        Container.ServerFilter = serverFilter;
        return this;
    }

    public OAGridPageBuilder<T> Header(Action<OAGridButtonBuilder<T>> headerAction)
    {
        headerAction(new OAGridButtonBuilder<T>(this));
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        TagBuilder container = new TagBuilder("div");

        if (Container.Header)
        {
            TagBuilder headerDiv = new TagBuilder("div");
            headerDiv.AddCssClass("oa_grid_header");

            foreach (var button in Container.HeaderButtons)
            {
                TagBuilder buttonAnchor = new TagBuilder("a");
                buttonAnchor.Attributes.Add("role", "button");
                buttonAnchor.AddCssClass(button.CssClasses);
                buttonAnchor.Attributes.Add("onclick", button.Click);
                if (button.HtmlAttributes != null)
                {
                    foreach (var attr in button.HtmlAttributes)
                    {
                        buttonAnchor.Attributes.Add(attr.Key, attr.Value);
                    }
                }
                headerDiv.InnerHtml.AppendHtml(buttonAnchor);

            }

            if (Container.ServerFilter)
            {
                TagBuilder span = new TagBuilder("span");
                span.AddCssClass("oa_server_filter");

                TagBuilder innerSpan = new TagBuilder("span");
                innerSpan.Attributes.Add("style", "display: inline-block");
                span.InnerHtml.AppendHtml(innerSpan);

                TagBuilder countLabel = new TagBuilder("label");
                countLabel.InnerHtml.Append("Count: ");
                innerSpan.InnerHtml.AppendHtml(countLabel);

                _htmlHelper.OA().NumericTextBox().HtmlAttributes(new { name = "numCount" });

                TagBuilder countInput = new TagBuilder("input");
                countInput.Attributes.Add("Name", "numCount");
                countInput.Attributes.Add("type", "number");
                countInput.Attributes.Add("value", "500");
                innerSpan.InnerHtml.AppendHtml(countInput);

                TagBuilder offsetLabel = new TagBuilder("label");
                offsetLabel.InnerHtml.Append("Offset: ");
                innerSpan.InnerHtml.AppendHtml(offsetLabel);

                TagBuilder offsetInput = new TagBuilder("input");
                offsetInput.Attributes.Add("Name", "numOffset");
                offsetInput.Attributes.Add("type", "number");
                offsetInput.Attributes.Add("value", "0");
                innerSpan.InnerHtml.AppendHtml(offsetInput);

                TagBuilder afAnchor = new TagBuilder("a");
                afAnchor.Attributes.Add("role", "button");
                afAnchor.AddCssClass("oa_grid_button fas fa-check");
                afAnchor.Attributes.Add("onclick", $"oa_grid.apply_filter('{Container.Name}')");
                innerSpan.InnerHtml.AppendHtml(afAnchor);

                TagBuilder cfAnchor = new TagBuilder("a");
                cfAnchor.Attributes.Add("role", "button");
                cfAnchor.AddCssClass("oa_grid_button fas fa-trash");
                cfAnchor.Attributes.Add("onclick", $"oa_grid.clear_filter('{Container.Name}')");
                innerSpan.InnerHtml.AppendHtml(cfAnchor);

                headerDiv.InnerHtml.AppendHtml(span);
            }

            container.InnerHtml.AppendHtml(headerDiv);
        }

        TagBuilder gridDiv = new TagBuilder("div");
        gridDiv.Attributes.Add("id", $"{Container.Name}_partial");
        gridDiv.Attributes.Add("partial", Container.Read);

        if (!string.IsNullOrEmpty(Container.Init))
        {
            gridDiv.Attributes.Add("init_function", Container.Init);
        }

        if (Container.ServerFilter)
        {
            gridDiv.Attributes.Add("filter_function", "oa_grid.set_server_filter");
            gridDiv.Attributes.Add("total_function", "oa_grid.set_grid_total");
        }

        IHtmlContent gridPartial = _htmlHelper.PartialAsync("~/Views/Shared/Grid/_AjaxGrid.cshtml", Container.Read).Result;

        gridDiv.InnerHtml.AppendHtml(gridPartial);

        container.InnerHtml.AppendHtml(gridDiv);

        container.WriteTo(writer, encoder);

    }
}