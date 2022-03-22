using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders.Textbox;

public class OATextArea<T>
    where T : class
{
    public Expression<Func<T, object>> Column { get; set; }

    public bool ReadOnly { get; set; }

    public object HtmlAttributes { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public int Rows { get; set; }

    public int Columns { get; set; }

    public OATextArea()
    {
        HtmlAttributes = new Dictionary<string, object>();
    }

}

public class OATextAreaBuilder<T> : OAControlBuilder<T>
        where T : class
{
    OATextArea<T> Container;

    public OATextAreaBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OATextArea<T>();
    }

    public OATextAreaBuilder(IHtmlHelper<T> htmlHelper, Expression<Func<T, object>> column) : base(htmlHelper)
    {
        Container = new OATextArea<T>
        {
            Column = column,
            Rows = 20,
            Columns = 100
        };
    }

    public OATextAreaBuilder<T> Readonly(bool readOnly)
    {
        Container.ReadOnly = readOnly;
        return this;
    }

    public OATextAreaBuilder<T> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OATextAreaBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OATextAreaBuilder<T> Value(string value)
    {
        Container.Value = value;
        return this;
    }

    public OATextAreaBuilder<T> Rows(int rows)
    {
        Container.Rows = rows;
        return this;
    }

    public OATextAreaBuilder<T> Columns(int columns)
    {
        Container.Columns = columns;
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        StringBuilder cssClasses = new StringBuilder();
        cssClasses.Append("form-control oa_textbox");

        if (Container.ReadOnly)
        {
            htmlAttributes.Add("readonly", "true");
            htmlAttributes.Add("disabled", "");
        }

        RouteValueDictionary attr = new RouteValueDictionary(Container.HtmlAttributes);

        string haClasses = string.Join(" ", attr.Where(m => m.Key == "class").Select(m => m.Value));
        if (!string.IsNullOrEmpty(haClasses))
        {
            cssClasses.Append($" {haClasses}");
            attr.Remove("class");
        }

        htmlAttributes.Add("rows", Container.Rows);
        htmlAttributes.Add("cols", Container.Columns);


        htmlAttributes.Add("class", cssClasses.ToString());


        foreach (var a in attr)
        {
            htmlAttributes.Add(a.Key, a.Value);
        }

        IHtmlContent element;
        if (Container.Column != null)
        {
            element = _htmlHelper.TextAreaFor(Container.Column, htmlAttributes);
        }
        else
        {
            element = _htmlHelper.TextArea(Container.Name, Container.Value, htmlAttributes);
        }

        element.WriteTo(writer, encoder);
    }

}
