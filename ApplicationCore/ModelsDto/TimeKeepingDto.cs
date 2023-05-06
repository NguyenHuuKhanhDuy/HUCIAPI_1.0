namespace ApplicationCore.ModelsDto
{
    public class TimeKeepingDto
    {
        public Guid Id { get; set; }

        public Guid UserCreateId { get; set; }

        public string UserCreateName { get; set; } = null!;

        public Guid UserTimeKeepingId { get; set; }

        public string UserTimeKeepingName { get; set; } = null!;

        public DateTime CreateDate { get; set; }
    }
}
