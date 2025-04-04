namespace api_vod.Models
{
    public class Verse
    {
        public int Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string VerseText { get; set; } = string.Empty;
        public int Chapter { get; set; }
        public int VerseNumber { get; set; }
        public string Book { get; set;} = string.Empty;
    }
}
