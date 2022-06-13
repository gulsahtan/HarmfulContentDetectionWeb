using Abp.AspNetCore.Mvc.Authorization;
using HarmfulContentDetection.Controllers;
using HarmfulContentDetection.Web.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ShowImageController : HarmfulContentDetectionControllerBase
    {
        private string _dir;

        public ShowImageController(IHostingEnvironment env)
        {
            _dir = env.WebRootPath;
        }
        public ActionResult ShowImage()
        {
            ObjectDetectionImageDto imageDetect = new ObjectDetectionImageDto();
            imageDetect.Alcohol = false;
            imageDetect.Cigarette = false;
            imageDetect.Violence = false;
            imageDetect.Image = null;
            return View(imageDetect);
        }
     
    }
}
