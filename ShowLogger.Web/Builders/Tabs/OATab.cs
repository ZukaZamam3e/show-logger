using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShowLogger.Web.Builders;

namespace ShowLogger.Web.Builders.Tabs;
public class OATab
{
    public string Name { get; set; }

    public List<OATabLink> Tabs { get; set; }

    public OATab()
    {
        Tabs = new List<OATabLink>();
    }
}

public class OATabBuilder<T> : OAControlBuilder<T>
        where T : class
{
    public OATab Container;

    public OATabBuilder(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
        Container = new OATab();
    }

    public OATabBuilder<T> Name(string name)
    {
        Container.Name = name;
        return this;
    }

    public OATabBuilder<T> Items(Action<OATabLinkBuilder<T>> items)
    {
        items(new OATabLinkBuilder<T>(this));

        return this;
    }

    public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        TagBuilder container = new TagBuilder("div");
        container.Attributes.Add("id", Container.Name);


        TagBuilder tabHeader = new TagBuilder("div");
        tabHeader.AddCssClass("tab");

        TagBuilder tabBody = new TagBuilder("div");

        container.InnerHtml.AppendHtml(tabHeader);


        foreach (var tab in Container.Tabs)
        {
            // Tab strip
            TagBuilder buttonTab = new TagBuilder("button");
            buttonTab.AddCssClass("tablinks");
            buttonTab.InnerHtml.AppendHtml(tab.Title);
            buttonTab.Attributes.Add("openTab", $"{Container.Name}-{tab.Title.Replace(" ", "")}");
            buttonTab.Attributes.Add("tabGroup", $"{Container.Name}-tabs");
            buttonTab.Attributes.Add("onclick", "oa_tabs.open_tab(this)");
            buttonTab.Attributes.Add("refreshWhenClicked", $"{tab.RefreshWhenClicked}");

            tabHeader.InnerHtml.AppendHtml(buttonTab);

            // Tab Body
            TagBuilder bodyTab = new TagBuilder("div");
            bodyTab.AddCssClass($"tabcontent {Container.Name}-tabs");
            bodyTab.Attributes.Add("tab", $"{Container.Name}-{tab.Title.Replace(" ", "")}");

            if (tab.Selected)
            {
                bodyTab.AddCssClass($"active-tab");
                buttonTab.AddCssClass($"active");

                IHtmlContent tabPartial = _htmlHelper.PartialAsync(tab.Partial).Result;

                tab.Partial = "";

                bodyTab.InnerHtml.AppendHtml(tabPartial);
            }
            else
            {
                bodyTab.AddCssClass($"non-active-tab");
            }

            if (tab.Content != null)
            {
                bodyTab.InnerHtml.AppendHtml(GetHtmlContent(tab.Content.Invoke(null)));
            }
            else if (!string.IsNullOrEmpty(tab.Partial))
            {
                buttonTab.Attributes.Add("partial", tab.Partial);
            }

            container.InnerHtml.AppendHtml(bodyTab);
        }

        container.WriteTo(writer, encoder);
    }
}
