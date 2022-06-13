using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Web.Models;
using Abp.Web.Mvc.Models;
using HarmfulContentDetection.Controllers;
using HarmfulContentDetection.DetectVideos;
using HarmfulContentDetection.Videos;
using HarmfulContentDetection.Web.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class VideoInsertPlatformController : HarmfulContentDetectionControllerBase
    {
        private readonly IRepository<DetectVideo> _repositoryDetectVideo;
        private readonly IRepository<Video> _repositoryOrijinalVideo;
        private readonly IErrorInfoBuilder _errorInfoBuilder;

        public VideoInsertPlatformController(IRepository<DetectVideo> repositoryDetectVideo, IRepository<Video> repositoryOrijinalVideo, IErrorInfoBuilder errorInfoBuilder)
        {
            _repositoryDetectVideo = repositoryDetectVideo;
            _repositoryOrijinalVideo = repositoryOrijinalVideo;
            _errorInfoBuilder = errorInfoBuilder;
        }
        public ActionResult VideoInsertPlatform()
        {
            ObjectDetectionVideoInsertDto videoInsert = new ObjectDetectionVideoInsertDto();
            videoInsert.Alcohol = false;
            videoInsert.Cigarette = false;
            videoInsert.Violence = false;
            videoInsert.VideoData = null;
            return View(videoInsert);
        }
        public class FileUploadVideoViewModel
        {
            public bool Cigarette { get; set; }
            public bool Alcohol { get; set; }
            public bool Violence { get; set; }
            public IFormFile VideoData { get; set; }
        }

        [HttpPost]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public async Task<IActionResult> Create(FileUploadVideoViewModel model)
        {
            var lenghtMB = (model.VideoData.Length) / 1048576;
            if (lenghtMB > 500)
            {
                var exception = new Exception("You cannot attach files larger than 500 MB!");
                return View(
                "Error",
                new ErrorViewModel(
                    _errorInfoBuilder.BuildForException(exception),
                    exception
                )
            );
            }
            using (var dataStream = new MemoryStream())
            {
                await model.VideoData.CopyToAsync(dataStream);
                if (model.Alcohol == false && model.Violence == false && model.Cigarette == false)
                {
                    Video orijinalVideo = new Video
                    {
                        VideoData = dataStream.ToArray(),
                        VideoContent = model.VideoData.FileName,
                        VideoName = model.VideoData.FileName
                    };
                    _repositoryOrijinalVideo.Insert(orijinalVideo);
                }
                else
                {
                    //var orijinalVideoId = _repositoryOrijinalVideo.FirstOrDefault(k => k.VideoName == model.VideoData.FileName).Id;
                    DetectVideo detectVideo = new DetectVideo
                    {
                        VideoData = dataStream.ToArray(),
                        Alcohol = model.Alcohol,
                        Cigarette = model.Cigarette,
                        OrijinalVideoId = 1,
                        VideoContent = System.Text.Encoding.UTF8.GetString(dataStream.ToArray(), 0, (int)dataStream.Length),
                        VideoName = model.VideoData.FileName,
                        Violence = model.Violence
                    };
                    _repositoryDetectVideo.Insert(detectVideo);
                }

                CurrentUnitOfWork.SaveChanges();
                return RedirectToAction(nameof(VideoInsertPlatform));
            }
        }

    }
}
