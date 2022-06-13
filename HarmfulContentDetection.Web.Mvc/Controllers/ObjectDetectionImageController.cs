using Abp.AspNetCore.Mvc.Authorization;
using HarmfulContentDetection.Controllers;
using HarmfulContentDetection.Scorer;
using HarmfulContentDetection.Scorer.Models;
using HarmfulContentDetection.Web.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace HarmfulContentDetection.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ObjectDetectionImageController : HarmfulContentDetectionControllerBase
    {      
      
        public ActionResult ObjectDetectionOnImage()
        {
            ObjectDetectionImageDto imageDetect = new ObjectDetectionImageDto();
            imageDetect.Alcohol = false;
            imageDetect.Cigarette = false;
            imageDetect.Violence = false;
            imageDetect.Image = null;
            return View(imageDetect);
        }
        public class FileUploadImageViewModel
        {
            public bool Cigarette { get; set; }
            public bool Alcohol { get; set; }
            public bool Violence { get; set; }
            public IFormFile Image { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> ObjectDetectionOnImage(FileUploadImageViewModel model)
        {
            try
            {

                if (model.Image == null || model.Image.Length == 0)
                {
                    return BadRequest();
                }

                using (var memoryStream = new MemoryStream())
                {
                    await model.Image.CopyToAsync(memoryStream);
                    using (var image = Image.FromStream(memoryStream))
                    {
                        if (model.Alcohol == true || model.Cigarette == true ||  model.Violence == true)
                        {
                            List<Prediction> predictions = new List<Prediction>();
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
                                double score = Math.Round(prediction.Score, 2);


                                for (Int32 xx = (int)prediction.Rectangle.Left; xx < prediction.Rectangle.Right; xx += 25)
                                {
                                    for (Int32 yy = (int)prediction.Rectangle.Top; yy < prediction.Rectangle.Bottom; yy += 25)
                                    {
                                        Int32 avgR = 0, avgG = 0, avgB = 0;
                                        Int32 blurPixelCount = 0;
                                        Rectangle currentRect = new Rectangle(xx, yy, 25, 25);

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
                        ObjectDetectionImageDto result = new ObjectDetectionImageDto();
                        using (MemoryStream mStream = new MemoryStream())
                        {
                            image.Save(mStream, image.RawFormat);
                            result.Image = mStream.ToArray();
                            mStream.Close();
                            mStream.Dispose();
                        }
                        image.Dispose();

                        return View(result);
                    }
                }
            }

            catch (System.OutOfMemoryException)
            {
                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
            }
            return View();
        }

    }
}
