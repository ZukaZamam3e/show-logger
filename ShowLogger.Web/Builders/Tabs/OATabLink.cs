using Microsoft.AspNetCore.Html;

namespace ShowLogger.Web.Builders.Tabs;
public class OATabLink
{
    public string Title { get; set; }

    public string Read { get; set; }

    public bool Selected { get; set; }

    public string Partial { get; set; }

    public Func<object, IHtmlContent> Content { get; set; }
}

public class OATabLinkBuilder<T>
        where T : class
{
    private OATabBuilder<T> Parent;

    private OATabLink Container;

    public OATabLinkBuilder(OATabBuilder<T> parent)
    {
        Parent = parent;
    }

    public OATabLinkBuilder<T> Add()
    {
        Container = new OATabLink();
        Parent.Container.Tabs.Add(Container);

        return this;
    }

    public OATabLinkBuilder<T> Title(string title)
    {
        Container.Title = title;
        return this;
    }

    public OATabLinkBuilder<T> Selected(bool selected)
    {
        Parent.Container.Tabs.ForEach(m =>
        {
            m.Selected = false;
        });

        Container.Selected = selected;
        return this;
    }

    public OATabLinkBuilder<T> Content(Func<object, IHtmlContent> content)
    {
        Container.Partial = null;
        Container.Content = content;
        return this;
    }

    public OATabLinkBuilder<T> LoadFromPartial(string partial)
    {
        Container.Content = null;
        Container.Partial = partial;
        return this;
    }
}