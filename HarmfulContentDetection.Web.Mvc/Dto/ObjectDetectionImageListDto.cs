using System.Collections.Generic;

namespace HarmfulContentDetection.Web.Dto
{
    public class ObjectDetectionImageListDto
    {
        public byte[] Image { get; set; }
        public bool Cigarette { get; set; }
        public bool Alcohol { get; set; }
        public bool Violence { get; set; }
        public int FrameNo { get; set; }
        public bool Cigarettev { get; set; }
        public bool Alcoholv { get; set; }
        public bool Violencev { get; set; }
    }
}
