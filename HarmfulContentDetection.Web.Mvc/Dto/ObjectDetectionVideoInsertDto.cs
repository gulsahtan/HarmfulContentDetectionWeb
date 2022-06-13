namespace HarmfulContentDetection.Web.Dto
{
    public class ObjectDetectionVideoInsertDto
    {
        public byte[] VideoData { get; set; }
        public bool Cigarette { get; set; }
        public bool Alcohol { get; set; }
        public bool Violence { get; set; }
    }
}
