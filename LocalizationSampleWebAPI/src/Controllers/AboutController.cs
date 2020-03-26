using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizationSampleWebAPI.Controllers
{
    [Route("{culture:culture}/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IStringLocalizer<AboutController> localizer;
        public AboutController(IStringLocalizer<AboutController> localizer)
        {
            this.localizer = localizer;
        }
        public string Get()
        {
            return localizer["About"];
        }
    }
}