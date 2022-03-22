using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders.Date;
public class OADate<T, TValue>
{
    public Expression<Func<T, TValue>> Property { get; set; }

    public object HtmlAttributes { get; set; }

    public DateTime Value { get; set; }

    public string Name { get; set; }

    public string Format { get; set; } = "LT";

    public bool ReadOnly { get; set; }
}

public class OADateBuilder<T, TValue> : OAControlBuilder<T>
        where T : class
{
    public OADate<T, TValue> Container;

    public OADateBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OADate<T, TValue>();
    }

    public OADateBuilder(IHtmlHelper<T> htmlHelper, Expression<Func<T, TValue>> prop) : base(htmlHelper)
    {
        Container = new OADate<T, TValue>
        {
            Property = prop
        };
    }

    public OADateBuilder<T, TValue> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OADateBuilder<T, TValue> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OADateBuilder<T, TValue> Value(DateTime value)
    {
        Container.Value = value;
        return this;
    }

    public OADateBuilder<T, TValue> Readonly(bool readOnly)
    {
        Container.ReadOnly = readOnly;
        return this;
    }

    public OADateBuilder<T, TValue> Format(string format)
    {
        Container.Format = format;
        return this;
    }


    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        StringBuilder cssClasses = new StringBuilder();
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

        TagBuilder container = new TagBuilder("div");

        container.AddCssClass($"input-group date oa_date {cssClasses.ToString()}");

        if (Container.ReadOnly)
        {
            htmlAttributes.Add("readonly", "true");
            htmlAttributes.Add("disabled", "");
        }

        string propName = "";
        string defaultDate = "";

        if (Container.Property != null)
        {
            propName = ((MemberExpression)Container.Property.Body).Member.Name;
            container.Attributes.Add("name", propName);
            container.Attributes.Add("id", propName);

            string dateZ = Container.Property.Compile()(_htmlHelper.ViewData.Model).ToString();
            if (!string.IsNullOrEmpty(dateZ))
            {
                DateTime date = DateTime.Parse(dateZ);
                defaultDate = date.ToString("MM/dd/yyyy hh:mm tt");
            }

            //BinaryExpression expression = ((BinaryExpression)Container.Property.Body);
            //DateTime date = )expression.Right.;

            //var prop = (PropertyInfo)((MemberExpression)Container.Property.Body).Member;
            //prop.GetValue(date);
        }
        else
        {
            propName = Container.Name;
        }

        container.Attributes.Add("defaultDate", defaultDate);
        container.Attributes.Add("format", Container.Format);

        container.MergeAttributes(htmlAttributes);

        TagBuilder input = new TagBuilder("input");

        input.Attributes.Add("name", propName);
        input.Attributes.Add("id", propName);

        input.Attributes.Add("type", "text");
        input.Attributes.Add("class", "form-control");

        if (Container.ReadOnly)
        {
            input.Attributes.Add("readonly", "true");
            input.Attributes.Add("disabled", "");
        }

        container.InnerHtml.AppendHtml(input);

        TagBuilder span1 = new TagBuilder("span");
        span1.AddCssClass("input-group-append");

        if (!Container.ReadOnly)
        {
            TagBuilder span2 = new TagBuilder("span");
            span2.AddCssClass("input-group-text fa fa-calendar");
            span1.InnerHtml.AppendHtml(span2);
        }
        container.InnerHtml.AppendHtml(span1);

        container.WriteTo(writer, encoder);

    }
}
