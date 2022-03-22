using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders.DropDownList;
public class OADropDownList<T, TValue>
{
    public Expression<Func<T, TValue>> Property { get; set; }

    public object HtmlAttributes { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }

    public string? ValueList { get; set; }

    public string ParentName { get; set; }

    public string ParentFunc { get; set; }

    public string DataTextName { get; set; }

    public string DataTextValue { get; set; }

    public string OnChange { get; set; }

    public string EmptyOption { get; set; }
}

public class OADropDownListBuilder<T, TValue> : OAControlBuilder<T>
{
    OADropDownList<T, TValue> Container;

    public OADropDownListBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OADropDownList<T, TValue>();
    }

    public OADropDownListBuilder<T, TValue> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OADropDownListBuilder(IHtmlHelper<T> htmlHelper, Expression<Func<T, TValue>> prop) : base(htmlHelper)
    {
        Container = new OADropDownList<T, TValue>
        {
            Property = prop
        };
    }

    public OADropDownListBuilder<T, TValue> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OADropDownListBuilder<T, TValue> Value(string value)
    {
        Container.Value = value;
        return this;
    }

    public OADropDownListBuilder<T, TValue> ValueList(string? value)
    {
        Container.ValueList = value;
        return this;
    }

    public OADropDownListBuilder<T, TValue> Parent(string name, string valFunc)
    {
        Container.ParentName = name;
        Container.ParentFunc = valFunc;
        return this;
    }

    public OADropDownListBuilder<T, TValue> DataTextName(string dataTextName)
    {
        Container.DataTextName = dataTextName;
        return this;
    }

    public OADropDownListBuilder<T, TValue> DataTextValue(string dataTextValue)
    {
        Container.DataTextValue = dataTextValue;
        return this;
    }

    public OADropDownListBuilder<T, TValue> OnChange(string onChange)
    {
        if (!onChange.EndsWith("()"))
        {
            onChange += "()";
        }

        Container.OnChange = onChange;
        return this;
    }

    public OADropDownListBuilder<T, TValue> EmptyOption(string emptyOption)
    {
        Container.EmptyOption = emptyOption;
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        StringBuilder cssClasses = new StringBuilder();
        cssClasses.Append("oa_dropdownlist");

        RouteValueDictionary attr = new RouteValueDictionary(Container.HtmlAttributes);

        string haClasses = string.Join(" ", attr.Where(m => m.Key == "class").Select(m => m.Value));
        if (!string.IsNullOrEmpty(haClasses))
        {
            cssClasses.Append($" {haClasses}");
            attr.Remove("class");
        }

        TagBuilder container = new TagBuilder("select");


        string propName = "";

        if (Container.Property != null)
        {
            propName = ((MemberExpression)Container.Property.Body).Member.Name;
            container.Attributes.Add("name", propName);
            container.Attributes.Add("id", propName);

            string option = Container.Property.Compile()(_htmlHelper.ViewData.Model).ToString();
            if (!string.IsNullOrEmpty(option))
            {
                container.Attributes.Add("defaultValue", option);
            }
        }
        else
        {
            propName = Container.Name;
            container.Attributes.Add("id", propName);
        }

        container.AddCssClass($"{cssClasses.ToString()}");

        htmlAttributes.Add("value_list_url", Container.ValueList);
        htmlAttributes.Add("parent_name", Container.ParentName);
        htmlAttributes.Add("parent_func", Container.ParentFunc);

        if (string.IsNullOrEmpty(Container.DataTextName))
        {
            Container.DataTextName = "DecodeTxt";
        }

        if (string.IsNullOrEmpty(Container.DataTextValue))
        {
            Container.DataTextValue = "CodeValue";
        }

        

        htmlAttributes.Add("data_text_name", char.ToLowerInvariant(Container.DataTextName[0]) + Container.DataTextName.Substring(1));
        htmlAttributes.Add("data_text_value", char.ToLowerInvariant(Container.DataTextValue[0]) + Container.DataTextValue.Substring(1));
        htmlAttributes.Add("onChange", Container.OnChange);
        htmlAttributes.Add("emptyOption", Container.EmptyOption);

        foreach (var a in attr)
        {
            htmlAttributes.Add(a.Key, a.Value);
        }

        container.MergeAttributes(htmlAttributes);

        container.WriteTo(writer, encoder);
    }
}