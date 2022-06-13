using System.Collections.Generic;
using HarmfulContentDetection.Scorer.Models.Abstract;

namespace HarmfulContentDetection.Scorer.Models
{
    public class YoloCocoP6Model : DetectModel
    {
        public override int Width { get; set; } = 1280;
        public override int Height { get; set; } = 1280;
        public override int Depth { get; set; } = 3;

        public override int Dimensions { get; set; } = 85;

        public override float[] Strides { get; set; } = new float[] { 8, 16, 32, 64 };

        public override float[][][] Anchors { get; set; } = new float[][][]
        {
            new float[][] { new float[] { 019, 027 }, new float[] { 044, 040 }, new float[] { 038, 094 } },
            new float[][] { new float[] { 096, 068 }, new float[] { 086, 152 }, new float[] { 180, 137 } },
            new float[][] { new float[] { 140, 301 }, new float[] { 303, 264 }, new float[] { 238, 542 } },
            new float[][] { new float[] { 436, 615 }, new float[] { 739, 380 }, new float[] { 925, 792 } }
        };

        public override int[] Shapes { get; set; } = new int[] { 160, 80, 40, 20 };

        public override float Confidence { get; set; } = 0.20f;
        public override float MulConfidence { get; set; } = 0.25f;
        public override float Overlap { get; set; } = 0.45f;

        public override string[] Outputs { get; set; } = new[] { "output" };

        public override List<Label> Labels { get; set; } = new List<Label>()
        {
            new Label { Id = 1, Name = "person" },
            new Label { Id = 2, Name = "bicycle" },
            new Label { Id = 3, Name = "car" },
            new Label { Id = 4, Name = "motorcycle" },
            new Label { Id = 5, Name = "airplane" },
            new Label { Id = 6, Name = "bus" },
            new Label { Id = 7, Name = "train" },
            new Label { Id = 8, Name = "truck" },
            new Label { Id = 9, Name = "boat" },
            new Label { Id = 10, Name = "traffic light" },
            new Label { Id = 11, Name = "fire hydrant" },
            new Label { Id = 12, Name = "stop sign" },
            new Label { Id = 13, Name = "parking meter" },
            new Label { Id = 14, Name = "bench" },
            new Label { Id = 15, Name = "bird" },
            new Label { Id = 16, Name = "cat" },
            new Label { Id = 17, Name = "dog" },
            new Label { Id = 18, Name = "horse" },
            new Label { Id = 19, Name = "sheep" },
            new Label { Id = 20, Name = "cow" },
            new Label { Id = 21, Name = "elephant" },
            new Label { Id = 22, Name = "bear" },
            new Label { Id = 23, Name = "zebra" },
            new Label { Id = 24, Name = "giraffe" },
            new Label { Id = 25, Name = "backpack" },
            new Label { Id = 26, Name = "umbrella" },
            new Label { Id = 27, Name = "handbag" },
            new Label { Id = 28, Name = "tie" },
            new Label { Id = 29, Name = "suitcase" },
            new Label { Id = 30, Name = "frisbee" },
            new Label { Id = 31, Name = "skis" },
            new Label { Id = 32, Name = "snowboard" },
            new Label { Id = 33, Name = "sports ball" },
            new Label { Id = 34, Name = "kite" },
            new Label { Id = 35, Name = "baseball bat" },
            new Label { Id = 36, Name = "baseball glove" },
            new Label { Id = 37, Name = "skateboard" },
            new Label { Id = 38, Name = "surfboard" },
            new Label { Id = 39, Name = "tennis racket" },
            new Label { Id = 40, Name = "bottle" },
            new Label { Id = 41, Name = "wine glass" },
            new Label { Id = 42, Name = "cup" },
            new Label { Id = 43, Name = "fork" },
            new Label { Id = 44, Name = "knife" },
            new Label { Id = 45, Name = "spoon" },
            new Label { Id = 46, Name = "bowl" },
            new Label { Id = 47, Name = "banana" },
            new Label { Id = 48, Name = "apple" },
            new Label { Id = 49, Name = "sandwich" },
            new Label { Id = 50, Name = "orange" },
            new Label { Id = 51, Name = "broccoli" },
            new Label { Id = 52, Name = "carrot" },
            new Label { Id = 53, Name = "hot dog" },
            new Label { Id = 54, Name = "pizza" },
            new Label { Id = 55, Name = "donut" },
            new Label { Id = 56, Name = "cake" },
            new Label { Id = 57, Name = "chair" },
            new Label { Id = 58, Name = "couch" },
            new Label { Id = 59, Name = "potted plant" },
            new Label { Id = 60, Name = "bed" },
            new Label { Id = 61, Name = "dining table" },
            new Label { Id = 62, Name = "toilet" },
            new Label { Id = 63, Name = "tv" },
            new Label { Id = 64, Name = "laptop" },
            new Label { Id = 65, Name = "mouse" },
            new Label { Id = 66, Name = "remote" },
            new Label { Id = 67, Name = "keyboard" },
            new Label { Id = 68, Name = "cell phone" },
            new Label { Id = 69, Name = "microwave" },
            new Label { Id = 70, Name = "oven" },
            new Label { Id = 71, Name = "toaster" },
            new Label { Id = 72, Name = "sink" },
            new Label { Id = 73, Name = "refrigerator" },
            new Label { Id = 74, Name = "book" },
            new Label { Id = 75, Name = "clock" },
            new Label { Id = 76, Name = "vase" },
            new Label { Id = 77, Name = "scissors" },
            new Label { Id = 78, Name = "teddy bear" },
            new Label { Id = 79, Name = "hair drier" },
            new Label { Id = 80, Name = "toothbrush" }
        };

        public override bool UseDetect { get; set; } = true;

        public YoloCocoP6Model()
        {

        }
    }
}
