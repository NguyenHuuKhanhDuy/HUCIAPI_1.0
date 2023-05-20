namespace ApplicationCore.ViewModels.Shift
{
    public class ShiftUpdateVM
    {
        public Guid Id { get; set; }
        public string StartTime { get; set; } = null!;

        public string EndTime { get; set; } = null!;
    }
}
