using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders.Button;
public class OAButton
{
    public string Name { get; set; }

    public string Click { get; set; }

    public object HtmlAttributes { get; set; }

    public string Text { get; set; }

    public string Icon { get; set; }
}

public class OAButtonBuilder<T> : OAControlBuilder<T>
{
    private OAButton Container;

    public OAButtonBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OAButton();
    }

    public OAButtonBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OAButtonBuilder<T> Click(string click)
    {
        Container.Click = click;
        return this;
    }

    public OAButtonBuilder<T> Text(string text)
    {
        Container.Text = text;
        return this;
    }

    public OAButtonBuilder<T> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OAButtonBuilder<T> Icon(string icon)
    {
        Container.Icon = icon;
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        StringBuilder cssClasses = new StringBuilder();
        cssClasses.Append("btn");

        RouteValueDictionary attr = new RouteValueDictionary(Container.HtmlAttributes);

        string haClasses = string.Join(" ", attr.Where(m => m.Key == "class").Select(m => m.Value));
        if (!string.IsNullOrEmpty(haClasses))
        {
            cssClasses.Append($" {haClasses}");
            attr.Remove("class");
        }

        foreach (var a in attr)
        {
            htmlAttributes.Add(a.Key, a.Value);
        }

        htmlAttributes.Add("type", "button");
        htmlAttributes.Add("id", Container.Name);
        htmlAttributes.Add("onClick", Container.Click);
        TagBuilder button = new TagBuilder("button");

        button.AddCssClass(cssClasses.ToString());
        button.MergeAttributes(htmlAttributes);

        string space = "";

        if (!string.IsNullOrEmpty(Container.Icon))
        {
            space = " ";
            TagBuilder icon = new TagBuilder("span");
            icon.Attributes.Add("class", Container.Icon);
            button.InnerHtml.AppendHtml(icon);
        }

        button.InnerHtml.Append($"{space}{Container.Text}");


        button.WriteTo(writer, encoder);
    }

}
