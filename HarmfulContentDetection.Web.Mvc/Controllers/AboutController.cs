using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using HarmfulContentDetection.Controllers;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : HarmfulContentDetectionControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
