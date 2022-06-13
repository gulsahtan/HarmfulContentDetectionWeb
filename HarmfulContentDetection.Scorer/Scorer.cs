﻿using HarmfulContentDetection.Scorer.Models.Abstract;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HarmfulContentDetection.Scorer
{
    /// <summary>
    /// Yolov5 scorer.
    /// </summary>
    public class Scorer<T> : IDisposable where T : DetectModel
    {
        private readonly T _model;

        private readonly InferenceSession _inferenceSession;

        /// <summary>
        /// Outputs value between 0 and 1.
        /// </summary>
        private float Sigmoid(float value)
        {
            return 1 / (1 + (float)Math.Exp(-value));
        }

        /// <summary>
        /// Converts xywh bbox format to xyxy.
        /// </summary>
        private float[] Xywh2xyxy(float[] source)
        {
            var result = new float[4];

            result[0] = source[0] - source[2] / 2f;
            result[1] = source[1] - source[3] / 2f;
            result[2] = source[0] + source[2] / 2f;
            result[3] = source[1] + source[3] / 2f;

            return result;
        }

        /// <summary>
        /// Returns value clamped to the inclusive range of min and max.
        /// </summary>
        public float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        /// <summary>
        /// Resizes image keeping ratio to fit model input size.
        /// </summary>
        private Bitmap ResizeImage(Image image)
        {
            PixelFormat format = image.PixelFormat;

            var output = new Bitmap(_model.Width, _model.Height, format);

            var (w, h) = (image.Width, image.Height); // image width and height
            var (xRatio, yRatio) = (_model.Width / (float)w, _model.Height / (float)h); // x, y ratios
            var ratio = Math.Min(xRatio, yRatio); // ratio = resized / original
            var (width, height) = ((int)(w * ratio), (int)(h * ratio)); // roi width and height
            var (x, y) = ((_model.Width / 2) - (width / 2), (_model.Height / 2) - (height / 2)); // roi x and y coordinates
            var roi = new Rectangle(x, y, width, height); // region of interest

            using (var graphics = Graphics.FromImage(output))
            {
                graphics.Clear(Color.FromArgb(0, 0, 0, 0)); // clear canvas

                graphics.SmoothingMode = SmoothingMode.None; // no smoothing
                graphics.InterpolationMode = InterpolationMode.Bilinear; // bilinear interpolation
                graphics.PixelOffsetMode = PixelOffsetMode.Half; // half pixel offset

                graphics.DrawImage(image, roi); // draw scaled
            }

            return output;
        }

        /// <summary>
        /// Extracts pixels into tensor for net input.
        /// </summary>
        private Tensor<float> ExtractPixels(Image image)
        {
            var bitmap = (Bitmap)image;

            var rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            var tensor = new DenseTensor<float>(new[] { 1, 3, _model.Height, _model.Width });

            unsafe // speed up conversion by direct work with memory
            {
                Parallel.For(0, bitmapData.Height, (y) =>
                {
                    byte* row = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);

                    Parallel.For(0, bitmapData.Width, (x) =>
                    {
                        tensor[0, 0, y, x] = row[x * bytesPerPixel + 2] / 255.0F; // r
                        tensor[0, 1, y, x] = row[x * bytesPerPixel + 1] / 255.0F; // g
                        tensor[0, 2, y, x] = row[x * bytesPerPixel + 0] / 255.0F; // b
                    });
                });

                bitmap.UnlockBits(bitmapData);
            }

            return tensor;
        }

        /// <summary>
        /// Runs inference session.
        /// </summary>
        private DenseTensor<float>[] Inference(Image image)
        {
            Bitmap resized = null;

            if (image.Width != _model.Width || image.Height != _model.Height)
            {
                resized = ResizeImage(image); // fit image size to specified input size
            }

            var inputs = new List<NamedOnnxValue> // add image as onnx input
            {
                NamedOnnxValue.CreateFromTensor("images", ExtractPixels(resized ?? image))
            };

            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> result = _inferenceSession.Run(inputs); // run inference

            var output = new List<DenseTensor<float>>();
            Parallel.ForEach(_model.Outputs, item => {
                output.Add(result.First(x => x.Name == item).Value as DenseTensor<float>);

            });
            //foreach (var item in _model.Outputs) // add outputs for processing
            //{
                
            //};

            return output.ToArray();
        }

        /// <summary>
        /// Parses net output (detect) to predictions.
        /// </summary>
        private List<Prediction> ParseDetect(DenseTensor<float> output, Image image)
        {
            var result = new ConcurrentBag<Prediction>();

            var (w, h) = (image.Width, image.Height); // image w and h
            var (xGain, yGain) = (_model.Width / (float)w, _model.Height / (float)h); // x, y gains
            var gain = Math.Min(xGain, yGain); // gain = resized / original

            var (xPad, yPad) = ((_model.Width - w * gain) / 2, (_model.Height - h * gain) / 2); // left, right pads

            Parallel.For(0, (int)output.Length / _model.Dimensions, (i) =>
            {
                if (output[0, i, 4] <= _model.Confidence) return; // skip low obj_conf results

                Parallel.For(5, _model.Dimensions, (j) =>
                {
                    output[0, i, j] = output[0, i, j] * output[0, i, 4]; // mul_conf = obj_conf * cls_conf
                });

                Parallel.For(5, _model.Dimensions, (k) =>
                {
                    if (output[0, i, k] <= _model.MulConfidence) return; // skip low mul_conf results

                    float xMin = ((output[0, i, 0] - output[0, i, 2] / 2) - xPad) / gain; // unpad bbox tlx to original
                    float yMin = ((output[0, i, 1] - output[0, i, 3] / 2) - yPad) / gain; // unpad bbox tly to original
                    float xMax = ((output[0, i, 0] + output[0, i, 2] / 2) - xPad) / gain; // unpad bbox brx to original
                    float yMax = ((output[0, i, 1] + output[0, i, 3] / 2) - yPad) / gain; // unpad bbox bry to original

                    xMin = Clamp(xMin, 0, w); // clip bbox tlx to boundaries
                    yMin = Clamp(yMin, 0, h); // clip bbox tly to boundaries
                    xMax = Clamp(xMax, 0, w); // clip bbox brx to boundaries
                    yMax = Clamp(yMax, 0, h); // clip bbox bry to boundaries

                    Label label = _model.Labels[k - 5];

                    var prediction = new Prediction(label, output[0, i, k])
                    {
                        Rectangle = new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin)
                    };

                    result.Add(prediction);
                });
            });

            return result.ToList();
        }

        /// <summary>
        /// Parses net outputs (sigmoid) to predictions.
        /// </summary>
        private List<Prediction> ParseSigmoid(DenseTensor<float>[] output, Image image)
        {
            var result = new ConcurrentBag<Prediction>();

            var (w, h) = (image.Width, image.Height); // image w and h
            var (xGain, yGain) = (_model.Width / (float)w, _model.Height / (float)h); // x, y gains
            var gain = Math.Min(xGain, yGain); // gain = resized / original

            var (xPad, yPad) = ((_model.Width - w * gain) / 2, (_model.Height - h * gain) / 2); // left, right pads

            Parallel.For(0, output.Length, (i) => // iterate model outputs
            {
                int shapes = _model.Shapes[i]; // shapes per output

                Parallel.For(0, _model.Anchors.Length, (a) => // iterate model configuration anchors
                {
                    Parallel.For(0, shapes, (y) => // iterate shapes (rows)
                    {
                        Parallel.For(0, shapes, (x) => // iterate shapes (columns)
                        {
                            int offset = (shapes * shapes * a + shapes * y + x) * _model.Dimensions;

                            float[] buffer = output[i].Skip(offset).Take(_model.Dimensions).Select(Sigmoid).ToArray();

                            if (buffer[4] <= _model.Confidence) return; // skip low obj_conf results

                            List<float> scores = buffer.Skip(5).Select(b => b * buffer[4]).ToList(); // mul_conf = obj_conf * cls_conf

                            float mulConfidence = scores.Max(); // max confidence score

                            if (mulConfidence <= _model.MulConfidence) return; // skip low mul_conf results

                            float rawX = (buffer[0] * 2 - 0.5f + x) * _model.Strides[i]; // predicted bbox x (center)
                            float rawY = (buffer[1] * 2 - 0.5f + y) * _model.Strides[i]; // predicted bbox y (center)

                            float rawW = (float)Math.Pow(buffer[2] * 2, 2) * _model.Anchors[i][a][0]; // predicted bbox w
                            float rawH = (float)Math.Pow(buffer[3] * 2, 2) * _model.Anchors[i][a][1]; // predicted bbox h

                            float[] xyxy = Xywh2xyxy(new float[] { rawX, rawY, rawW, rawH });

                            float xMin = Clamp((xyxy[0] - xPad) / gain, 0, w); // unpad, clip tlx
                            float yMin = Clamp((xyxy[1] - yPad) / gain, 0, h); // unpad, clip tly
                            float xMax = Clamp((xyxy[2] - xPad) / gain, 0, w); // unpad, clip brx
                            float yMax = Clamp((xyxy[3] - yPad) / gain, 0, h); // unpad, clip bry

                            Label label = _model.Labels[scores.IndexOf(mulConfidence)];

                            var prediction = new Prediction(label, mulConfidence)
                            {
                                Rectangle = new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin)
                            };

                            result.Add(prediction);
                        });
                    });
                });
            });

            return result.ToList();
        }

        /// <summary>
        /// Parses net outputs (sigmoid or detect layer) to predictions.
        /// </summary>
        private List<Prediction> ParseOutput(DenseTensor<float>[] output, Image image)
        {
            return _model.UseDetect ? ParseDetect(output[0], image) : ParseSigmoid(output, image);
        }

        /// <summary>
        /// Removes overlaped duplicates (nms).
        /// </summary>
        private List<Prediction> Supress(List<Prediction> items)
        {
            var result = new List<Prediction>(items);


            //foreach (var item in items) // iterate every prediction
            //{
            //    foreach (var current in result.ToList()) // make a copy for each iteration
            //    {
            //        if (current == item) continue;

            //        var (rect1, rect2) = (item.Rectangle, current.Rectangle);

            //        RectangleF intersection = RectangleF.Intersect(rect1, rect2);

            //        float intArea = intersection.Area(); // intersection area
            //        float unionArea = rect1.Area() + rect2.Area() - intArea; // union area
            //        float overlap = intArea / unionArea; // overlap ratio

            //        if (overlap >= _model.Overlap)
            //        {
            //            if (item.Score >= current.Score)
            //            {
            //                result.Remove(current);
            //            }
            //        }
            //    }
            //}

            return result;
        }

        /// <summary>
        /// Runs object detection.
        /// </summary>
        public List<Prediction> Predict(Image image)
        {
            return ParseOutput(Inference(image), image);
        }

        /// <summary>
        /// Creates new instance of Scorer.
        /// </summary>
        public Scorer()
        {
            _model = Activator.CreateInstance<T>();
        }

        /// <summary>
        /// Creates new instance of Scorer with weights path and options.
        /// </summary>
        public Scorer(string weights, SessionOptions opts = null) : this()
        {
            _inferenceSession = new InferenceSession(File.ReadAllBytes(weights), opts ?? new SessionOptions());
        }

        /// <summary>
        /// Creates new instance of Scorer with weights stream and options.
        /// </summary>
        public Scorer(Stream weights, SessionOptions opts = null) : this()
        {
            using (var reader = new BinaryReader(weights))
            {
                _inferenceSession = new InferenceSession(reader.ReadBytes((int)weights.Length), opts ?? new SessionOptions());
            }
        }

        /// <summary>
        /// Creates new instance of Scorer with weights bytes and options.
        /// </summary>
        public Scorer(byte[] weights, SessionOptions opts = null) : this()
        {
            _inferenceSession = new InferenceSession(weights, opts ?? new SessionOptions());
        }

        /// <summary>
        /// Disposes Scorer instance.
        /// </summary>
        public void Dispose()
        {
            _inferenceSession.Dispose();
        }
    }
}
