namespace HarmfulContentDetection.Web.Dto
{
    public class ObjectDetectionVideoDetectDto
    {
        public int Id  { get; set; }
        public bool Cigarette { get; set; }
        public bool Alcohol { get; set; }
        public bool Violence { get; set; }
        public string VideoName { get; set; }
        public string VideoContent { get; set; }
        public string VideoModel { get; set; }
        public bool Detect { get; set; }

    }
}
