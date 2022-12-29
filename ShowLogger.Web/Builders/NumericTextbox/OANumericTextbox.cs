using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShowLogger.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ShowLogger.Web.Builders.NumericTextbox;
    public class OANumericTextbox<T>
        where T : class
    {
        public Expression<Func<T, object>> Column { get; set; }

        public bool ReadOnly { get; set; }

        public object HtmlAttributes { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public OANumericTextbox()
        {
            HtmlAttributes = new Dictionary<string, object>();
        }
    }

public class OANumericTextboxBuilder<T> : OAControlBuilder<T>
        where T : class
{
    OANumericTextbox<T> Container;

    public OANumericTextboxBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OANumericTextbox<T>();
    }

    public OANumericTextboxBuilder(IHtmlHelper<T> htmlHelper, Expression<Func<T, object>> column) : base(htmlHelper)
    {
        Container = new OANumericTextbox<T>
        {
            Column = column
        };
    }

    public OANumericTextboxBuilder<T> Readonly(bool readOnly)
    {
        Container.ReadOnly = readOnly;
        return this;
    }

    public OANumericTextboxBuilder<T> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OANumericTextboxBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OANumericTextboxBuilder<T> Value(string value)
    {
        Container.Value = value;
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        htmlAttributes.Add("type", "tel");

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

        IHtmlContent element;
        if (Container.Column != null)
        {
            //Type t = Container.Column.GetObjectType();

            PropertyInfo info = Container.Column.GetPropertyInfo(_htmlHelper.ViewData.Model);
            string value = "";

            object model = Container.Column.Compile()(_htmlHelper.ViewData.Model);

            if (model != null)
            {
                value = Container.Column.Compile()(_htmlHelper.ViewData.Model).ToString();
            }

            element = _htmlHelper.TextBox(info.Name, value, htmlAttributes);
        }
        else
        {
            element = _htmlHelper.TextBox(Container.Name, Container.Value, htmlAttributes);
        }

        element.WriteTo(writer, encoder);
    }
}
