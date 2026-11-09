namespace RaceDay.Mvc.ViewModels
{
    public class EventDetailsViewModel
    {
        public EventViewModel Event { get; set; } = new();

        public List<RaceDay.Contracts.CategoryDto> Categories { get; set; } = new();
    }
}

