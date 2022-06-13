using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using HarmfulContentDetection.Controllers;
using HarmfulContentDetection.DetectVideos;
using HarmfulContentDetection.Videos;
using HarmfulContentDetection.Web.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class VideoWatchingPlatformController : HarmfulContentDetectionControllerBase
    {
        private static string _dir;
        private readonly IErrorInfoBuilder _errorInfoBuilder;
        private readonly IRepository<Video> _repositoryVideo;
        private readonly IRepository<DetectVideo> _repositoryDetectVideo;

        public VideoWatchingPlatformController(IHostingEnvironment env, IErrorInfoBuilder errorInfoBuilder, IRepository<Video> repositoryVideo, IRepository<DetectVideo> repositoryDetectVideo)
        {
            _dir = env.WebRootPath;
            _errorInfoBuilder = errorInfoBuilder;
            _repositoryVideo = repositoryVideo;
            _repositoryDetectVideo = repositoryDetectVideo;
        }
        //public  ActionResult VideoWatchingPlatform()
        //{
        //    var a = _repositoryVideo.GetAllList();
        //    return View(_repositoryVideo.GetAllList());
        //}

        public async Task<IActionResult> VideoWatchingPlatform()
        {
            return View(_repositoryVideo.GetAll().Select(p => new ObjectDetectionVideoDetectDto { Id = p.Id, VideoContent = p.VideoContent, VideoName = p.VideoName, Alcohol = false, Violence = false, Cigarette = false }).ToList());
        }
        public class DataModel
        {
            public bool Cigarette { get; set; }
            public bool Alcohol { get; set; }
            public bool Violence { get; set; }
            public int Id { get; set; }
        }
        public ActionResult Show(DataModel model)
        {
            var Id = 0;
            bool detect = false;
            if (model.Alcohol == false && model.Cigarette == false && model.Violence == false)
            {
                 Id = model.Id;
            }
            else
            {
                detect = true;
                var allList=_repositoryDetectVideo.GetAll().Where(k => k.OrijinalVideoId == model.Id && (k.Alcohol == model.Alcohol && k.Cigarette == model.Cigarette && k.Violence == model.Violence)).Select(p => new ObjectDetectionVideoDetectDto { Id = p.Id }).ToList();
                foreach (var ids in allList)
                {
                    Id = ids.Id;
                }
            }
            ObjectDetectionVideoDetectDto result = new ObjectDetectionVideoDetectDto
            {
                Alcohol = model.Alcohol,
                Violence = model.Violence,
                Cigarette = model.Cigarette,
                Id = Id,
                Detect = detect,
                VideoModel = Id + ".mp4"
            };
            return PartialView("~/Views/VideoWatchingPlatform/ShowImage.cshtml", result);
        }
        public async Task<IActionResult> OrijinalVideoStream(int id, bool alcohol, bool violence, bool cigarette)
        {
            if (alcohol == false && cigarette == false && violence == false)
            {
                //var file = _repositoryVideo.GetAllListAsync().GetAwaiter().GetResult().Where(x => x.Id == id).FirstOrDefault();
                string dosya_yolu = @"D:\\datayol.txt";

                //StreamWriter sw = new StreamWriter(dosya_yolu);
                //string someString = Encoding.ASCII.GetString(file.VideoData);
                //sw.WriteLine(someString);
                //sw.Close();
                StreamReader sr = new StreamReader(dosya_yolu);

                //using var writer = new BinaryWriter(sr);
                //writer.Write(data);
                string Veri = sr.ReadToEnd();
                sr.Close();

                byte[] data = Encoding.ASCII.GetBytes(Veri);

                return File(data, "video/mp4");

            }
            else
            {
                var file = _repositoryDetectVideo.GetAllListAsync().GetAwaiter().GetResult().Where(x => x.OrijinalVideoId == id && x.Alcohol == alcohol && x.Violence == violence && x.Cigarette == cigarette).FirstOrDefault();
                return File(file.VideoData, "video/mp4");

            }

        }
    }
}
