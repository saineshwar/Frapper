using System.Collections.Generic;
using Frapper.Web.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Frapper.Web.CustomTagHelper
{
    [HtmlTargetElement("trow", Attributes = "isAsc,sortby,pagesize,search")]
    public class SorterTagHelper : TagHelper
    {
        private const int V = 0;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private LocalizationService _localizationService;
        public SorterTagHelper(IUrlHelperFactory helperFactory, LocalizationService localizationService)
        {
            _urlHelperFactory = helperFactory;
            _localizationService = localizationService;
        }

        #region Input Attributes
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("isAsc")]
        public bool isAsc { get; set; }

        [HtmlAttributeName("sortby")]
        public int sortby { get; set; }

        [HtmlAttributeName("pagesize")]
        public int? pagesize { get; set; }

        [HtmlAttributeName("search")]
        public string search { get; set; }
        #endregion

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            List<string> listofheaders = new List<string>()
            {
                _localizationService.GetLocalizedHtmlString("FirstName"),
                _localizationService.GetLocalizedHtmlString("LastName"),
                _localizationService.GetLocalizedHtmlString("EmailId"),
                _localizationService.GetLocalizedHtmlString("MobileNo"),
                _localizationService.GetLocalizedHtmlString("State"),
                _localizationService.GetLocalizedHtmlString("City")
            };

            TagBuilder tr = new TagBuilder("tr");
            int headerid = V;
            for (int row = 1; row <= 6; row++)
            {
                TagBuilder th = new TagBuilder("th");
                TagBuilder tag = new TagBuilder("a");
                var togglesort = (row == sortby ? (!isAsc).ToString() : "true");

                tag.Attributes["href"] = urlHelper.Action("Index", "Customer", new { page = pagesize, sortby = row, isAsc = togglesort, Search = search });
                tag.InnerHtml.Append(listofheaders[headerid]);

                if (sortby != 0)
                {
                    if (row == sortby)
                    {
                        TagBuilder tagspan = new TagBuilder("span");
                        tagspan.AddCssClass($"{(isAsc ? "fas fa-arrow-up" : "fas fa-arrow-down")}");
                        th.InnerHtml.AppendHtml(tagspan);
                    }
                }
                th.InnerHtml.AppendHtml(tag);
                tr.InnerHtml.AppendHtml(th);
                headerid += 1;
            }
            output.Content.AppendHtml(tr.InnerHtml);
        }
    }
}

