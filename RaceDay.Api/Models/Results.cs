namespace RaceDay.Api.Models
{
    public class Results
    {
        public string ResultID { get; set; } = string.Empty;
        public TimeSpan? FinishTime {  get; set; }

        public int? Position { get; set; }

        public string ResultStatus { get; set; } = "Pending";

        public string EnrolmentID { get; set; } = string.Empty;
        public Enrolment Enrolment { get; set; } = null;
    }
}
