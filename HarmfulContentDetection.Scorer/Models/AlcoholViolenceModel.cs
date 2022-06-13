using System.Collections.Generic;
using HarmfulContentDetection.Scorer.Models.Abstract;

namespace HarmfulContentDetection.Scorer.Models
{
    public class AlcoholViolenceModel : DetectModel
    {
        public override int Width { get; set; } = 640;
        public override int Height { get; set; } = 640;
        public override int Depth { get; set; } = 3;

        public override int Dimensions { get; set; } = 15;

        public override float[] Strides { get; set; } = new float[] { 8, 16, 32 };

        public override float[][][] Anchors { get; set; } = new float[][][]
        {
            new float[][] { new float[] { 010, 13 }, new float[] { 016, 030 }, new float[] { 033, 023 } },
            new float[][] { new float[] { 030, 61 }, new float[] { 062, 045 }, new float[] { 059, 119 } },
            new float[][] { new float[] { 116, 90 }, new float[] { 156, 198 }, new float[] { 373, 326 } }
        };

        public override int[] Shapes { get; set; } = new int[] { 80, 40, 20 };

        public override float Confidence { get; set; } = 0.20f;
        public override float MulConfidence { get; set; } = 0.25f;
        public override float Overlap { get; set; } = 0.45f;

        public override string[] Outputs { get; set; } = new[] { "output" };
        public override List<Label> Labels { get; set; } = new List<Label>()
        {
            new Label { Id = 1, Name = "beer" },
            new Label { Id = 2, Name = "bootle" },
            new Label { Id = 3, Name = "cocktail" },
            new Label { Id = 4, Name = "whisky" },
            new Label { Id = 5, Name = "winechampagne" },
            new Label { Id = 6, Name = "pistol" },
            new Label { Id = 7, Name = "smartphone" },
            new Label { Id = 8, Name = "knife" },
            new Label { Id = 9, Name = "siliconegun" },
            new Label { Id = 10, Name = "rifle" }
        };

        public override bool UseDetect { get; set; } = true;

        public AlcoholViolenceModel()
        {

        }
    }
}
