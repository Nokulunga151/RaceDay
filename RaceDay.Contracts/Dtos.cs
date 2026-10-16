using System;
using System.Collections.Generic;
using System.Text;

namespace RaceDay.Contracts
{
    
    
        public class RegisterRequest
        {
            public string Role { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;

            // Organiser details
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;

            // Participant details
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public int? Age { get; set; }
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class AuthResponse
        {
            public string Message { get; set; } = string.Empty;
            public int UserId { get; set; }
            public string Role { get; set; } = string.Empty;
        }

        // PROFILE DTOs

        public class ProfileDto
        {
            public int UserId { get; set; }
            public string Role { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public int? Age { get; set; }
        }

        public class UpdateProfileRequest
        {
            public string Name { get; set; } = string.Empty;
            public string Surname { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public int? Age { get; set; }
        }


        // EVENT DTOs

        public class CreateEventRequest
        {
            public string EventID { get; set; } = string.Empty;
            public string EventName { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public string Location { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
        }

        public class UpdateEventRequest
        {
            public string EventName { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public string Location { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
        }

        public class EventDto
        {
            public string EventID { get; set; } = string.Empty;
            public string EventName { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public string Location { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
            public int OrganiserID { get; set; }
        }

        // CATEGORY DTOs

        public class CreateCategoryRequest
        {
            public string CategoryID { get; set; } = string.Empty;
            public string CategoryName { get; set; } = string.Empty;
            public decimal Distance { get; set; }
            public decimal EntryFee { get; set; }
        }

        public class UpdateCategoryRequest
        {
            public string CategoryName { get; set; } = string.Empty;
            public decimal Distance { get; set; }
            public decimal EntryFee { get; set; }
        }

        public class CategoryDto
        {
            public string CategoryID { get; set; } = string.Empty;
            public string CategoryName { get; set; } = string.Empty;
            public decimal Distance { get; set; }
            public decimal EntryFee { get; set; }
            public string EventID { get; set; } = string.Empty;
        }


        // ENROLMENT DTOs

        public class CreateEnrolmentRequest
        {
            public string EventID { get; set; } = string.Empty;
            public string CategoryID { get; set; } = string.Empty;
        }

        public class UpdateEnrolmentStatusRequest
        {
            public string EnrolmentStatus { get; set; } = string.Empty;
        }


        public class EnrolmentDto
        {
            public string EnrolmentID { get; set; } = string.Empty;
            public int ParticipantID { get; set; }
            public string EventID { get; set; } = string.Empty;
            public string CategoryID { get; set; } = string.Empty;
            public DateTime EnrolmentDate { get; set; }
            public string EnrolmentStatus { get; set; } = string.Empty;
        }

        // RESULT DTOs

        public class CreateResultRequest
        {
            public string ResultID { get; set; } = string.Empty;
            public string EnrolmentID { get; set; } = string.Empty;
            public TimeSpan? FinishTime { get; set; }
            public int? Position { get; set; }
            public string ResultStatus { get; set; } = "Pending";
        }

        public class UpdateResultRequest
        {
            public TimeSpan? FinishTime { get; set; }
            public int? Position { get; set; }
            public string ResultStatus { get; set; } = "Pending";
        }

        public class ResultDto
        {
            public string ResultID { get; set; } = string.Empty;
            public string EnrolmentID { get; set; } = string.Empty;
            public TimeSpan? FinishTime { get; set; }
            public int? Position { get; set; }
            public string ResultStatus { get; set; } = string.Empty;
        }


    
}
