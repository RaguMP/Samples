using LocalizationSample.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizationSample.API.Controllers
{
    [Route("{culture:culture}/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        public HomeController(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }
        public string Get()
        {
            return localizer["Home"];
        }
    }
}