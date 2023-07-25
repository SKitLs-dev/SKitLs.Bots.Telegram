using SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Text;

namespace SKitLs.Bots.Telegram.AdvancedMessages.Model.Messages.Attaches
{
    public enum MediaType
    {
        Text, Photo, Location
    }

    [Obsolete("Will be rebuilt.")]
    public class OMDMedia : OMDText
    {
        public MediaType Type { get; set; }

        public string? MediaTempFile { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        //public OMDTMedia() { }
        public OMDMedia(string message) : base(message) { }

        public bool IsValid => Type switch
        {
            //MediaType.Text => Sections.Count != 0,
            MediaType.Photo => MediaTempFile != null,
            _ => true,
        };
    }
}
