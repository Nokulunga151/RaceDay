namespace RaceDay.Api.Models
{
    public class Participants
    {
        public int ParticipantID {  get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; } 
        public string Phone { get; set; }  
        public int Age { get; set; }    

        public string PasswordHash { get; set; }

        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>(); 
    }
}
