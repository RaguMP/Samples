using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LocalizationSampleSingleResxFile.Localize;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LocalizationSampleSingleResxFile.Controllers
{
    [Route("{culture:culture}/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        private readonly IStringLocalizer<Resource> localizer;
        public AboutController(IStringLocalizer<Resource> localizer)
        {
            this.localizer = localizer;
        }
        public string Get()
        {
            return localizer["About"];
        }
    }
}