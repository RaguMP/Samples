using LocalizationSample.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizationSample.API.Controllers
{
    [Route("{culture:culture}/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        public AboutController(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }
        public string Get()
        {
            return localizer["About"];
        }
    }
}