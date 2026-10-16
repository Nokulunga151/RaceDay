namespace RaceDay.Api.Models
{
    public class Event
    {
        public string EventID { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public decimal Distance { get; set; }

        public string EventType { get; set; } = string.Empty;
        public int OrganiserID { get; set; }
        public Organiser Organiser { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
