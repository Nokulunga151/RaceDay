using System.ComponentModel.DataAnnotations;

namespace RaceDay.Mvc.ViewModels
{
    public class EventFormViewModel
    {
        [Required(ErrorMessage = "Event ID is required.")]
        public string EventID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event name is required.")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required.")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 1000)]
        public decimal Distance { get; set; }

        [Required(ErrorMessage = "Event type is required.")]
        public string EventType { get; set; } = string.Empty;

        public IFormFile? EventFile { get; set; }
    }
}
