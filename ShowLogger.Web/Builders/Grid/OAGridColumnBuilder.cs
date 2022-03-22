using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NonFactors.Mvc.Grid;
using System.Text.Encodings.Web;

namespace ShowLogger.Web.Builders.Grid;
public class OAGridColumnBuilder<T> : IHtmlContent
    where T : class
{
    public string Column;
    public string Script;
    public IGridColumnsOf<T> Parent;


    public OAGridColumnBuilder(IGridColumnsOf<T> parent)
    {
        Parent = parent;
    }

    public virtual void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        TagBuilder container = new TagBuilder("span");
        container.InnerHtml.AppendHtml(Column);
        container.InnerHtml.AppendHtml(Script);

        container.WriteTo(writer, encoder);
    }
}