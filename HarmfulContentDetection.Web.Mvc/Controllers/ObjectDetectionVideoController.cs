using Abp.AspNetCore.Mvc.Authorization;
using Abp.Web.Models;
using Abp.Web.Mvc.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Frapper;
using HarmfulContentDetection.Controllers;
using HarmfulContentDetection.Scorer;
using HarmfulContentDetection.Scorer.Models;
using HarmfulContentDetection.Web.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ObjectDetectionVideoController : HarmfulContentDetectionControllerBase
    {
        private static string _dir;
        private readonly IErrorInfoBuilder _errorInfoBuilder;

        public ObjectDetectionVideoController(IHostingEnvironment env, IErrorInfoBuilder errorInfoBuilder)
        {
            _dir = env.WebRootPath;
            _errorInfoBuilder = errorInfoBuilder;
        }

        public ActionResult ObjectDetectionOnVideo()
        {
            ObjectDetectionVideoDto imageDetect = new ObjectDetectionVideoDto();
            imageDetect.Alcohol = false;
            imageDetect.Cigarette = false;
            imageDetect.Violence = false;
            imageDetect.Image = null;
            imageDetect.IsLoading = true;
            return View(imageDetect);
        }
        public class FileUploadVideoViewModel
        {
            public bool Cigarette { get; set; }
            public bool Alcohol { get; set; }
            public bool Violence { get; set; }
            public IFormFile Image { get; set; }
        }

        [HttpPost]
        [RequestSizeLimit(500 * 1024 * 1024)]       //unit is bytes => 500Mb
        [RequestFormLimits(MultipartBodyLengthLimit = 500 * 1024 * 1024)]
        public async Task<IActionResult> ObjectDetectionOnVideo(FileUploadVideoViewModel model)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var lenghtMB = (model.Image.Length) / 1048576;
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
            if (!System.IO.File.Exists(Path.Combine(_dir, "file1.mp4")))
            {
                using (var fileStream =
                    new FileStream(Path.Combine(_dir, "file1.mp4"), FileMode.Create, FileAccess.Write))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
            }
            else
            {
                using (var fileStream =
          new FileStream(Path.Combine(_dir, "file1.mp4"), FileMode.Open, FileAccess.Write))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
            }


            if (!System.IO.File.Exists(Path.Combine(_dir, "convert.mp4")))
            {

                using (var fileStream =
                    new FileStream(Path.Combine(_dir, "convert.mp4"), FileMode.Create, FileAccess.Write))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
            }
            else
            {
                using (var fileStream =
          new FileStream(Path.Combine(_dir, "convert.mp4"), FileMode.Open, FileAccess.Write))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
            }

            VideoCapture capture = new VideoCapture(Path.Combine(_dir, "file1.mp4"));
            Mat img2 = new Mat();
            capture.Read(img2);
            var image1 = (Image)img2.ToBitmap();
            int fps = (int)capture.Get(CapProp.Fps);
            int frameCount = (int)capture.Get(CapProp.FrameCount);
            using (VideoCapture capture1 = new VideoCapture(Path.Combine(_dir, "file1.mp4")))
            using (VideoWriter writer = new VideoWriter(Path.Combine(_dir, "convert.mp4"), fps, new Size(capture1.Width, capture1.Height), true))
            {
                Mat img = new Mat();               

                while (frameCount > 0)
                {
                    capture1.Read(img);
                    List<Prediction> predictions = new List<Prediction>();
                    using var image = (Image)img.ToBitmap();

                    if (model.Alcohol == true || model.Cigarette == true || model.Violence == true)
                    {
                        if (model.Cigarette == true && model.Alcohol == false && model.Violence == false)
                        {
                            using var cigaretteScorer = new Scorer<CigaretteModel>("Assets/cigaretteweight/best.onnx");
                            List<Prediction> cigarettePred = cigaretteScorer.Predict(image);
                            predictions.AddRange(cigarettePred);
                        }
                        if (model.Cigarette == false && model.Alcohol == true && model.Violence == false)
                        {
                            using var alcoholScorer = new Scorer<AlcoholModel>("Assets/alcoholweight/best.onnx");
                            List<Prediction> alcoholPred = alcoholScorer.Predict(image);
                            predictions.AddRange(alcoholPred);
                            using var yolosScorer = new Scorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
                            List<Prediction> yolosPred = yolosScorer.Predict(image);
                            Parallel.ForEach(yolosPred, pred =>
                            {

                                if (pred.Label.Id == 40 || pred.Label.Id == 41)
                                {
                                    predictions.Add(pred);
                                }
                            });
                        }

                        if (model.Cigarette == false && model.Alcohol == false && model.Violence == true)
                        {
                            using var violenceScorer = new Scorer<ViolenceModel>("Assets/violenceweight/best.onnx");
                            List<Prediction> violencePred = violenceScorer.Predict(image);
                            predictions.AddRange(violencePred);
                            using var yolosScorer = new Scorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
                            List<Prediction> yolosPred = yolosScorer.Predict(image);
                            Parallel.ForEach(yolosPred, pred =>
                            {
                                if (pred.Label.Id == 44)
                                {
                                    predictions.Add(pred);
                                }
                            });
                        }
                        if (model.Cigarette == false && model.Alcohol == true && model.Violence == true)
                        {
                            using var alcoholviolenceScorer = new Scorer<AlcoholViolenceModel>("Assets/alcoholviolenceweight/best.onnx");
                            List<Prediction> alcoholviolencePred = alcoholviolenceScorer.Predict(image);
                            predictions.AddRange(alcoholviolencePred);
                            using var yolosScorer = new Scorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
                            List<Prediction> yolosPred = yolosScorer.Predict(image);
                            Parallel.ForEach(yolosPred, pred =>
                            {

                                if (pred.Label.Id == 40 || pred.Label.Id == 41 || pred.Label.Id == 44)
                                {
                                    predictions.Add(pred);
                                }
                            });
                        }
                        if (model.Cigarette == true && model.Alcohol == false && model.Violence == true)
                        {
                            using var cigaretteviolenceScorer = new Scorer<CigaretteViolenceModel>("Assets/cigaretteviolenceweight/best.onnx");
                            List<Prediction> cigaretteviolencePred = cigaretteviolenceScorer.Predict(image);
                            predictions.AddRange(cigaretteviolencePred);
                            using var yolosScorer = new Scorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
                            List<Prediction> yolosPred = yolosScorer.Predict(image);
                            Parallel.ForEach(yolosPred, pred =>
                            {
                                if (pred.Label.Id == 44)
                                {
                                    predictions.Add(pred);
                                }
                            });
                        }
                        if (model.Cigarette == true && model.Alcohol == true && model.Violence == false)
                        {
                            using var alcoholcigaretteScorer = new Scorer<AlcoholCigaretteModel>("Assets/alcoholcigaretteweight/best.onnx");
                            List<Prediction> alcoholcigarette = alcoholcigaretteScorer.Predict(image);
                            predictions.AddRange(alcoholcigarette);
                            using var yolosScorer = new Scorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
                            List<Prediction> yolosPred = yolosScorer.Predict(image);
                            Parallel.ForEach(yolosPred, pred =>
                            {

                                if (pred.Label.Id == 40 || pred.Label.Id == 41)
                                {
                                    predictions.Add(pred);
                                }
                            });
                        }
                        if (model.Cigarette == true && model.Alcohol == true && model.Violence == true)
                        {
                            using var allScorer = new Scorer<AlcoholCigaretteViolenceModel>("Assets/alcoholcigaretteviolenceweight/best.onnx");
                            List<Prediction> allPred = allScorer.Predict(image);
                            predictions.AddRange(allPred);
                            using var yolosScorer = new Scorer<YoloCocoP5Model>("Assets/Weights/yolov5s.onnx");
                            List<Prediction> yolosPred = yolosScorer.Predict(image);
                            Parallel.ForEach(yolosPred, pred =>
                            {

                                if (pred.Label.Id == 40 || pred.Label.Id == 41 || pred.Label.Id == 44)
                                {
                                    predictions.Add(pred);
                                }
                            });
                        }

                        using var graphics = Graphics.FromImage(image);
                        Bitmap img1 = new Bitmap(image);

                        foreach (var prediction in predictions)
                        {
                            if (prediction != null)
                            {
                                double score = Math.Round(prediction.Score, 2);


                                for (Int32 xx = (int)prediction.Rectangle.Left; xx < prediction.Rectangle.Right; xx += 12)
                                {
                                    for (Int32 yy = (int)prediction.Rectangle.Top; yy < prediction.Rectangle.Bottom; yy += 12)
                                    {
                                        Int32 avgR = 0, avgG = 0, avgB = 0;
                                        Int32 blurPixelCount = 0;
                                        Rectangle currentRect = new Rectangle(xx, yy, 12, 12);

                                        for (Int32 a = currentRect.Left; (a < currentRect.Right && a < image.Width); a++)
                                        {
                                            for (Int32 b = currentRect.Top; (b < currentRect.Bottom && b < image.Height); b++)
                                            {
                                                Color pixel = img1.GetPixel(a, b);

                                                avgR += pixel.R;
                                                avgG += pixel.G;
                                                avgB += pixel.B;

                                                blurPixelCount++;
                                            }
                                        }

                                        avgR = avgR / blurPixelCount;
                                        avgG = avgG / blurPixelCount;
                                        avgB = avgB / blurPixelCount;

                                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(avgR, avgG, avgB)), currentRect);
                                    }
                                }
                            }
                        }
                    }
                    Bitmap bitmapImage = new Bitmap(image);

                    Rectangle rectangle = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);//System.Drawing
                    BitmapData bmpData = bitmapImage.LockBits(rectangle, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//System.Drawing.Imaging

                    Image<Bgr, byte> outputImage = new Image<Bgr, byte>(bitmapImage.Width, bitmapImage.Height, bmpData.Stride, bmpData.Scan0);//(IntPtr)
                    writer.Write(outputImage);
                    Thread.Sleep((int)(1000.0 / fps));

                    frameCount--;
                }

            }
            watch.Stop();
            var total = watch.Elapsed.Seconds;
            string path = Path.Combine(_dir, "file1.mp4");
            string convert = Path.Combine(_dir, "convert.mp4");
            //string videosound = Path.Combine(_dir, "videosound.mp4");

            //Mp4ToWav(path, convert, 35);
            ObjectDetectionVideoDto result = new ObjectDetectionVideoDto();
            using (MemoryStream mStream = new MemoryStream())
            {
                image1.Save(mStream, ImageFormat.Png);
                result.Image = mStream.ToArray();
                result.IsLoading = false;
                mStream.Close();
                mStream.Dispose();
            }
            image1.Dispose();
            return View(result);
        }
        public static string Mp4ToWav(string path, string convert, int seconds = 35)
        {
            string result;
            return Mp4ToWav(path, convert, seconds, out result);
        }

        private static string Mp4ToWav(string path, string convert, int seconds, out string result)
        {
            FFMPEG ffmpeg = new FFMPEG();
            string detectedvideopath = convert;
            // Trim mp4.
            string outputPath = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(path) + "_trim.mp4";
            string result1 = ffmpeg.RunCommand("-ss 00:00:00.0 -t 00:00:" + seconds.ToString().PadLeft(2, '0') + ".0 -i \"" + path + "\" -c copy \"" + outputPath + "\"");
            // Convert to wav.
            string wavPath = outputPath.Replace(".mp4", ".wav");
            string result2 = ffmpeg.RunCommand("-i \"" + outputPath + "\" -acodec pcm_u8 -ar 22050 \"" + wavPath + "\"");
            mergeFile(wavPath, detectedvideopath);
            result = result1 + "\n\n\n" + result2;
            return wavPath;
        }

        private static void mergeFile(string wavefilepath, string videofilepath)
        {
            FFMPEG Path_FFMPEG = new FFMPEG();
            string Wavefile = wavefilepath;
            string video1 = videofilepath;
            string strResult;
            if (System.IO.File.Exists(Path.Combine(_dir, "videosound.mp4")))
            {
                System.IO.File.Delete(Path.Combine(_dir, "videosound.mp4"));
                strResult = Path.Combine(_dir) + "\\videosound.mp4";
            }
            else
            {
                strResult = Path.Combine(_dir) + "\\videosound.mp4";
            }
            System.Diagnostics.Process proc = new System.Diagnostics.Process();

            try
            {
                string result1 = Path_FFMPEG.RunCommand("-i " + video1 + " -i " + Wavefile + " -c:v copy -c:a aac -map 0:v:0 -map 1:a:0 " + strResult);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.FileName = Path_FFMPEG.ffExe;
                proc.Start();
                string StdOutVideo = proc.StandardOutput.ReadToEnd();
                string StdErrVideo = proc.StandardError.ReadToEnd();

                //System.IO.File.Delete(wavefilepath);
                //System.IO.File.Delete(videofilepath);
                //System.IO.File.Delete(Path.Combine(_dir, "file_trim.mp4"));

                //System.IO.File.Delete(detectpath);
            }
            catch { }
            finally
            {
                proc.WaitForExit();
                proc.Close();
            }

        }
    }
}
