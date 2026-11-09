namespace RaceDay.Mvc.ViewModels
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }

        public int EnrolmentID { get; set; }

        public string? FinishTime { get; set; }

        public int? Position { get; set; }

        public string ResultStatus { get; set; } = "Pending";
    }
}
