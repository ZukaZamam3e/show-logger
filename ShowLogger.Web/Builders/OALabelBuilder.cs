using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShowLogger.Web.Builders.Label;

namespace ShowLogger.Web.Builders;
public class OALabelBuilder<T, TValue> : OAControlBuilder<T>
{

    OALabel<T, TValue> Container;

    public OALabelBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OALabel<T, TValue>();
    }

    public OALabelBuilder(IHtmlHelper<T> htmlHelper, Expression<Func<T, TValue>> column) : base(htmlHelper)
    {
        Container = new OALabel<T, TValue>
        {
            Column = column
        };
    }

    public OALabelBuilder<T, TValue> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OALabelBuilder<T, TValue> Value(string value)
    {
        Container.Value = value;
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        StringBuilder cssClasses = new StringBuilder();
        cssClasses.Append("oa_label");

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

        IHtmlContent element;
        if (Container.Column != null)
        {
            element = _htmlHelper.LabelFor(Container.Column, htmlAttributes);
        }
        else
        {
            element = _htmlHelper.Label(Container.Name, Container.Value, htmlAttributes);
        }
        element.WriteTo(writer, encoder);

    }
}

public static class SSLabelBuilder
{
    public static IHtmlContent SSLabelFor<TModel, TValue>(this IHtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, string cssClasses = "", object htmlAttributes = null)
        where TModel : class
        where TValue : class
    {
        Dictionary<string, object> attributes = new Dictionary<string, object>();
        RouteValueDictionary attr = new RouteValueDictionary(htmlAttributes);

        foreach (var a in attr)
        {
            attributes.Add(a.Key, a.Value);
        }

        attributes.Add("class", $"{cssClasses} oa_label");

        return helper.LabelFor(expression, attributes);
    }
}