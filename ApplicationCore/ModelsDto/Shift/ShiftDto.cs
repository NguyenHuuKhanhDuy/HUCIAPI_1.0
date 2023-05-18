namespace ApplicationCore.ModelsDto.Shift
{
    public class ShiftDto
    {
        public Guid Id { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
