namespace HarmfulContentDetection.Web.Dto
{
    public class ObjectDetectionVideoDto
    {
        public byte[] Image { get; set; }
        public bool Cigarette { get; set; }
        public bool Alcohol { get; set; }
        public bool Violence { get; set; }
        public bool IsLoading { get; set; } = true;
    }
}
