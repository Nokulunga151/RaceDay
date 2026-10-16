using Microsoft.EntityFrameworkCore.Update.Internal;

namespace RaceDay.Api.Models
{
    public class Enrolment
    {
        public string EnrolmentID { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; }
        public string EnrolmentStatus { get; set; } = "Active";

        public int ParticipantID { get; set; }
        public Participants Participant { get; set; } = null;

        public string EventID { get; set; } = String.Empty;
        public Event Event { get; set; } = null;

        public string CategoryID { get; set; } = string.Empty;
        public Category Category { get; set; } = null;

        public Results? Result {  get; set; }
    }
}
