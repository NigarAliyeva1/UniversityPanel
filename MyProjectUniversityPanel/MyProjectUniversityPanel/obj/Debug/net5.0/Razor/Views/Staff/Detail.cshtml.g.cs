#pragma checksum "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c7e892930f81a46a2376784a67355d03b2181449"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Staff_Detail), @"mvc.1.0.view", @"/Views/Staff/Detail.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\_ViewImports.cshtml"
using MyProjectUniversityPanel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\_ViewImports.cshtml"
using MyProjectUniversityPanel.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\_ViewImports.cshtml"
using MyProjectUniversityPanel.ViewModels;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c7e892930f81a46a2376784a67355d03b2181449", @"/Views/Staff/Detail.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"de6e3a8415bb6becb8b4820e91fd180187ca1c3f", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Staff_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Staff>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("mdi mdi-keyboard-return text-black icon-lg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("height", new global::Microsoft.AspNetCore.Html.HtmlString("100"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("width", new global::Microsoft.AspNetCore.Html.HtmlString("100"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("width", new global::Microsoft.AspNetCore.Html.HtmlString("500px"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("height", new global::Microsoft.AspNetCore.Html.HtmlString("800px"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"
<style>
    * {
        margin: 0;
        padding: 0
    }

    body {
        background-color: white
    }

    .card {
        width: 350px;
        background-color: white;
        border: none;
        cursor: pointer;
        transition: all 0.5s;
    }

    .image img {
        transition: all 0.5s
    }

    .card:hover .image img {
        transform: scale(1.5)
    }

    .btn {
        height: 200px;
        width: 200px;
        border-radius: 50%
    }

    .name {
        font-size: 22px;
        font-weight: bold
    }

    .idd {
        font-size: 14px;
        font-weight: 600
    }

    .idd1 {
        font-size: 12px
    }

    .number {
        font-size: 22px;
        font-weight: bold
    }

    .follow {
        font-size: 12px;
        font-weight: 500;
        color: #444444
    }


    .text p {
        font-size: 13px;
        color: #545454;
        font-weight: 500
    }

    .icons i {
        font-size: 19px
  ");
            WriteLiteral(@"  }

    hr .new1 {
        border: 1px solid
    }

    .join {
        font-size: 14px;
        color: #a0a0a0;
        font-weight: bold
    }

    .date {
        background-color: #ccc
    }
</style>


<div class=""row"">
    <div class=""col-md-12 grid-margin stretch-card"">
        <div class=""card"">
            <div>
                <h1 class=""text-black"">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c7e892930f81a46a2376784a67355d03b21814497190", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("Go Back</h1>\r\n            </div>\r\n            <div class=\" image d-flex flex-column justify-content-center align-items-center\">\r\n                <button class=\"btn btn-secondary\">\r\n                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c7e892930f81a46a2376784a67355d03b21814498596", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1710, "~/assets/images/", 1710, 16, true);
#nullable restore
#line 95 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
AddHtmlAttributeValue("", 1726, Model.Image, 1726, 12, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </button>\r\n\r\n                <p class=\"name mt-3\">");
#nullable restore
#line 98 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
                                Write(Model.FullName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Birthday:</h5>\r\n                </div>\r\n                <h4>");
#nullable restore
#line 102 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Birthday.ToString("MMMM dd, yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Gender:</h5>\r\n                </div>\r\n\r\n                <h4>");
#nullable restore
#line 107 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Gender.Type);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Salary:</h5>\r\n                </div>\r\n\r\n                <h4>");
#nullable restore
#line 112 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Salary);

#line default
#line hidden
#nullable disable
            WriteLiteral("$</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Designation:</h5>\r\n                </div>\r\n\r\n                <h4>");
#nullable restore
#line 117 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Designation.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Education:</h5>\r\n                </div>\r\n                <h4>");
#nullable restore
#line 121 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Education);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Email:</h5>\r\n                </div>\r\n                <h4>");
#nullable restore
#line 125 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Mobile Number:</h5>\r\n                </div>\r\n                <h4>");
#nullable restore
#line 129 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Number);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Address:</h5>\r\n                </div>\r\n                <h4>");
#nullable restore
#line 133 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
               Write(Model.Address);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                <div class=\"d-flex flex-row justify-content-center align-items-center gap-2 text-muted\">\r\n                    <h5>Upload File:</h5>\r\n                </div>\r\n                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("embed", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c7e892930f81a46a2376784a67355d03b218144914781", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 3791, "~/assets/images/", 3791, 16, true);
#nullable restore
#line 137 "C:\Users\nigar\OneDrive\Masaüstü\UniversityPanel\MyProjectUniversityPanel\MyProjectUniversityPanel\Views\Staff\Detail.cshtml"
AddHtmlAttributeValue("", 3807, Model.File, 3807, 11, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                <h4></h4>\r\n\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Staff> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
