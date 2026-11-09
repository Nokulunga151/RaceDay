namespace RaceDay.Mvc.ViewModels
{
    public class EnrolmentManagementViewModel
    {
        public string EnrolmentID { get; set; } = string.Empty;
        public string EventID { get; set; } = string.Empty;
        public string CategoryID { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; }
        public string EnrolmentStatus { get; set; } = string.Empty;

        public int ParticipantID { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string ParticipantSurname { get; set; } = string.Empty;
        public string ParticipantEmail { get; set; } = string.Empty;
        public string ParticipantPhone { get; set; } = string.Empty;
    }
}
