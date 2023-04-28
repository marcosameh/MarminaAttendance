using Microsoft.AspNetCore.Razor.TagHelpers;

namespace App.UI.TagHelpers
{
    [HtmlTargetElement("if")]
    public class IfTagHelper : TagHelper
    {
        [HtmlAttributeName("is-true")]
        public bool IsTrue { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            if (IsTrue)
            {
                return;
            }

            output.SuppressOutput();

        }
    }


}

