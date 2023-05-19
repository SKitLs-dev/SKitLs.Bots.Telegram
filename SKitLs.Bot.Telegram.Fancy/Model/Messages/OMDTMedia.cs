namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages
{
    public enum MediaType
    {
        Text, Photo, Location
    }

    public class OMDTMedia : OutputMessageDecorText
    {
        public MediaType Type { get; set; }

        public string? MediaTempFile { get; set; }
        public double Longtitude { get; set; }
        public double Latitude { get; set; }

        //public OMDTMedia() { }
        public OMDTMedia(string? message) : base(message) { }

        public bool IsValid => Type switch
        {
            //MediaType.Text => Sections.Count != 0,
            MediaType.Photo => MediaTempFile != null,
            _ => true,
        };
    }
}
