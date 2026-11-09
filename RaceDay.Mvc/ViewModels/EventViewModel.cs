namespace RaceDay.Mvc.ViewModels
{
    public class EventViewModel
    {
        public string EventID { get; set; } = string.Empty;

        public string EventName { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Distance { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string? FileUrl { get; set; }
    }
}
