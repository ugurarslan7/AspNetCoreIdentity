using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentity.Web.TagHelpers
{
    public class UserPictureTumbnailTagHelper:TagHelper
    {
        public string? PictureUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            if (String.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/UserPhoto/defaultUserPhoto.jpg");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"{PictureUrl}");

            }
        }
    }
}
