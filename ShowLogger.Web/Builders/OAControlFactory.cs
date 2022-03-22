using Microsoft.AspNetCore.Mvc.Rendering;
using ShowLogger.Web.Builders.Button;
using ShowLogger.Web.Builders.Date;
using ShowLogger.Web.Builders.DropDownList;
using ShowLogger.Web.Builders.Grid;
using ShowLogger.Web.Builders.NumericTextbox;
using ShowLogger.Web.Builders.Tabs;
using ShowLogger.Web.Builders.Textbox;
using ShowLogger.Web.Builders.Window;
using System.Linq.Expressions;

namespace ShowLogger.Web.Builders;
public static class OAControls
{
    public static OAControlFactory<T> OA<T>(this IHtmlHelper<T> helper) where T : class
    {
        return new OAControlFactory<T>(helper);
    }
}

public class OAControlFactory<T> : OAControlBuilder<T>
    where T : class
{
    public OAControlFactory(IHtmlHelper<T> htmlHelper) : base(htmlHelper)
    {
    }

    public OAGridPageBuilder<T> GridPage()
    {
        OAGridPageBuilder<T> builder = new OAGridPageBuilder<T>(_htmlHelper);

        return builder;
    }

    public OATabBuilder<T> Tab()
    {
        OATabBuilder<T> builder = new OATabBuilder<T>(_htmlHelper);

        return builder;
    }

    public OATextboxBuilder<T> TextBoxFor(Expression<Func<T, object>> column)
    {
        OATextboxBuilder<T> builder = new OATextboxBuilder<T>(_htmlHelper, column);

        return builder;
    }

    public OATextboxBuilder<T> TextBox()
    {
        OATextboxBuilder<T> builder = new OATextboxBuilder<T>(_htmlHelper);

        return builder;
    }

    public OATextAreaBuilder<T> TextAreaFor(Expression<Func<T, object>> column)
    {
        OATextAreaBuilder<T> builder = new OATextAreaBuilder<T>(_htmlHelper, column);

        return builder;
    }

    public OATextAreaBuilder<T> TextArea()
    {
        OATextAreaBuilder<T> builder = new OATextAreaBuilder<T>(_htmlHelper);

        return builder;
    }

    public OALabelBuilder<T, TValue> Label<TValue>()
    {
        OALabelBuilder<T, TValue> builder = new OALabelBuilder<T, TValue>(_htmlHelper);

        return builder;
    }

    public OALabelBuilder<T, TValue> LabelFor<TValue>(Expression<Func<T, TValue>> column)
    {
        OALabelBuilder<T, TValue> builder = new OALabelBuilder<T, TValue>(_htmlHelper, column);

        return builder;
    }

    public OADateBuilder<T, TValue> Date<TValue>()
    {
        OADateBuilder<T, TValue> builder = new OADateBuilder<T, TValue>(_htmlHelper);

        return builder;
    }

    public OADateBuilder<T, TValue> DateFor<TValue>(Expression<Func<T, TValue>> column)
    {
        OADateBuilder<T, TValue> builder = new OADateBuilder<T, TValue>(_htmlHelper, column);

        return builder;
    }

    public OANumericTextboxBuilder<T> NumericTextBoxFor(Expression<Func<T, object>> column)
    {
        OANumericTextboxBuilder<T> builder = new OANumericTextboxBuilder<T>(_htmlHelper, column);

        return builder;
    }

    public OANumericTextboxBuilder<T> NumericTextBox()
    {
        OANumericTextboxBuilder<T> builder = new OANumericTextboxBuilder<T>(_htmlHelper);

        return builder;
    }

    public OAWindowBuilder<T> Window()
    {
        OAWindowBuilder<T> builder = new OAWindowBuilder<T>(_htmlHelper);

        return builder;
    }

    public OAButtonBuilder<T> Button()
    {
        OAButtonBuilder<T> builder = new OAButtonBuilder<T>(_htmlHelper);

        return builder;
    }

    public OADropDownListBuilder<T, TValue> DropDownList<TValue>()
    {
        OADropDownListBuilder<T, TValue> builder = new OADropDownListBuilder<T, TValue>(_htmlHelper);

        return builder;
    }

    public OADropDownListBuilder<T, TValue> DropDownListFor<TValue>(Expression<Func<T, TValue>> column)
    {
        OADropDownListBuilder<T, TValue> builder = new OADropDownListBuilder<T, TValue>(_htmlHelper, column);

        return builder;
    }
}