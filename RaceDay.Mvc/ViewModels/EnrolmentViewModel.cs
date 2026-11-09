namespace RaceDay.Mvc.ViewModels
{
    public class EnrolmentViewModel
    {
        public string EnrolmentID { get; set; } = string.Empty;

        public int ParticipantID { get; set; }

        public string EventID { get; set; } = string.Empty;

        public string CategoryID { get; set; } = string.Empty;

        public DateTime EnrolmentDate { get; set; }

        public string EnrolmentStatus { get; set; } = string.Empty;
    }
}
