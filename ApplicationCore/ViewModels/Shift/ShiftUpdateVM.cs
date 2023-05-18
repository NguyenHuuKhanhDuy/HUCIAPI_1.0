namespace ApplicationCore.ViewModels.Shift
{
    public class ShiftUpdateVM
    {
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
