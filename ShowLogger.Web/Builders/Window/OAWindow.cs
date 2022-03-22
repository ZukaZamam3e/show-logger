using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders.Window;

public class OAWindow
{
    public string Name { get; set; }

    public string Title { get; set; }

    public string Partial { get; set; }

    public object HtmlAttributes { get; set; }

    public int Width { get; set; }

    public Func<object, IHtmlContent> Content { get; set; }
}

public class OAWindowBuilder<T> : OAControlBuilder<T>
        where T : class
{
    private OAWindow Container;

    public OAWindowBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OAWindow();
    }

    public OAWindowBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OAWindowBuilder<T> Title(string title)
    {
        Container.Title = title;
        return this;
    }

    public OAWindowBuilder<T> HtmlAttributes(object htmlAttributes)
    {
        Container.HtmlAttributes = htmlAttributes;
        return this;
    }

    public OAWindowBuilder<T> Partial(string partial)
    {
        Container.Content = null;
        Container.Partial = partial;
        return this;
    }

    public OAWindowBuilder<T> Content(Func<object, IHtmlContent> content)
    {
        Container.Partial = null;
        Container.Content = content;
        return this;
    }

    public OAWindowBuilder<T> Width(int width)
    {
        Container.Width = width;
        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();

        StringBuilder cssClasses = new StringBuilder();
        cssClasses.Append("modal fade");

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

        TagBuilder modal = new TagBuilder("div");
        modal.AddCssClass(cssClasses.ToString());
        modal.Attributes.Add("role", "dialog");
        modal.Attributes.Add("id", $"{Container.Name}");
        modal.MergeAttributes(htmlAttributes);

        TagBuilder modalDialog = new TagBuilder("div");
        modalDialog.AddCssClass("modal-dialog");


        TagBuilder modalContent = new TagBuilder("div");
        modalContent.AddCssClass("modal-content");
        if (Container.Width > 0)
        {
            modalContent.Attributes.Add("style", $"width: {Container.Width.ToString()}px");
        }

        TagBuilder modalHeader = new TagBuilder("div");
        modalHeader.AddCssClass("modal-header");

        modalContent.InnerHtml.AppendHtml(modalHeader);


        TagBuilder modalTitle = new TagBuilder("h4");
        modalTitle.AddCssClass("modal-title");
        modalTitle.Attributes.Add("id", $"{Container.Name}_Title");
        modalTitle.InnerHtml.AppendHtml(Container.Title);

        modalHeader.InnerHtml.AppendHtml(modalTitle);


        TagBuilder modalClose = new TagBuilder("button");
        modalClose.AddCssClass("close");
        modalClose.Attributes.Add("type", "button");
        modalClose.Attributes.Add("data-dismiss", "modal");
        modalClose.InnerHtml.AppendHtml("&times;");

        modalHeader.InnerHtml.AppendHtml(modalClose);

        TagBuilder modalBody = new TagBuilder("div");
        modalBody.AddCssClass("modal-body");
        modalBody.Attributes.Add("id", $"{Container.Name}_Body");



        if (!string.IsNullOrEmpty(Container.Partial))
        {
            modalBody.InnerHtml.AppendHtml(GetHtmlContent(_htmlHelper.PartialAsync(Container.Partial).Result));
        }
        else if (Container.Content != null)
        {
            modalBody.InnerHtml.AppendHtml(GetHtmlContent(Container.Content.Invoke(null)));
        }

        modalContent.InnerHtml.AppendHtml(modalBody);
        modalDialog.InnerHtml.AppendHtml(modalContent);
        modal.InnerHtml.AppendHtml(modalDialog);

        modal.WriteTo(writer, encoder);
    }
}
