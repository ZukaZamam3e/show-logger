using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ShowLogger.Web.Builders.Textbox;
public class OATextbox<T>
    where T : class
{
    public Expression<Func<T, object>> Column { get; set; }

    public bool ReadOnly { get; set; }

    public object HtmlAttributes { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public OATextbox()
    {
        HtmlAttributes = new Dictionary<string, object>();
    }

}

public class OATextboxBuilder<T> : OAControlBuilder<T>
        where T : class
{
    OATextbox<T> Container;

    public OATextboxBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OATextbox<T>();
    }

    public OATextboxBuilder(IHtmlHelper<T> htmlHelper, Expression<Func<T, object>> column) : base(htmlHelper)
    {
        Container = new OATextbox<T>
        {
            Column = column
        };
    }

    public OATextboxBuilder<T> Readonly(bool readOnly)
    {
        Container.ReadOnly = readOnly;
        return this;
    }

    public OATextboxBuilder<T> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OATextboxBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OATextboxBuilder<T> Value(string value)
    {
        Container.Value = value;
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


        htmlAttributes.Add("class", cssClasses.ToString());


        foreach (var a in attr)
        {
            htmlAttributes.Add(a.Key, a.Value);
        }

        IHtmlContent element = Container.Column != null
            ? _htmlHelper.TextBoxFor(Container.Column, htmlAttributes)
            : _htmlHelper.TextBox(Container.Name, Container.Value, htmlAttributes);
        element.WriteTo(writer, encoder);
    }

}
