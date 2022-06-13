using Abp.AspNetCore.Mvc.Authorization;
using HarmfulContentDetection.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class DataSetController : HarmfulContentDetectionControllerBase
    {
        private readonly IHostingEnvironment _dir;

        public DataSetController(IHostingEnvironment env)
        {
            _dir = env;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> VideoZip()
        {
            var path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot", "samplevideos.zip");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/zip", Path.GetFileName(path));

        }
        public async Task<IActionResult> FormAppZip()
        {
            var path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot", "harmfulcontentdetection.zip");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/zip", Path.GetFileName(path));

        }
        public async Task<IActionResult> AlcoholZip()
        {
            var path = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot", "alcohol.zip");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/zip", Path.GetFileName(path));
           
        }

        public async Task<IActionResult> ViolenceZip()
        {
            var path = Path.Combine(
                      Directory.GetCurrentDirectory(),
                      "wwwroot", "violence.zip");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/zip", Path.GetFileName(path));
        }

       
        public async Task<IActionResult> CigaretteZip()
        {
            var path = Path.Combine(
                                 Directory.GetCurrentDirectory(),
                                 "wwwroot", "cigarette.zip");

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/zip", Path.GetFileName(path));
        }

    }
}
