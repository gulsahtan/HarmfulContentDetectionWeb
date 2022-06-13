namespace HarmfulContentDetection.Web.Dto
{
    public class ObjectDetectionImageDto
    {
        public byte[] Image { get; set; }
        public bool Cigarette { get; set; }
        public bool Alcohol { get; set; }
        public bool Violence { get; set; }
    }
}
