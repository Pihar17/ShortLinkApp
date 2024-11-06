namespace ShortLinkApp.Data
{
    // Класс для представления записи о сокращенном URL
    public class UrlEntry
    {
        public int Id { get; set; }
        public required string originalUrl { get; set; }
        public required string shortCode { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public int Clicks { get; set; }
    }
}
