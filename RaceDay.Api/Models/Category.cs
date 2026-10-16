namespace RaceDay.Api.Models
{
    public class Category
    {
        public string CategoryID { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Distance { get; set; }

        public decimal EntryFee { get; set; }
        public string EventID { get; set; } = string.Empty;
        public Event Event { get; set; } = null;


        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
       
    }
}