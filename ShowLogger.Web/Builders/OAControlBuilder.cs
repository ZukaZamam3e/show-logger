using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders;
public class OAControlBuilder<T> : IHtmlContent
{
    protected IHtmlHelper<T> _htmlHelper;

    public OAControlBuilder(IHtmlHelper<T> htmlHelper)
    {
        _htmlHelper = htmlHelper;
    }

    public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {

    }

    public string GetHtmlContent(IHtmlContent htmlContent)
    {
        string htmlString = "";

        using (var writer = new StringWriter())
        {
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            htmlString = writer.ToString();
        }

        return htmlString;
    }

}