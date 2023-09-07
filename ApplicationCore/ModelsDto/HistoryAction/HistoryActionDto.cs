namespace ApplicationCore.ModelsDto.HistoryAction
{
    public class HistoryActionDto
    {
        public Guid? Id { get; set; }
        public Guid IdAction { get; set; }
        public string Description { get; set; } = null!;
        public Guid UserCreateId { get; set; }
        public string UserCreateName { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public int TypeActionId { get; set; }
        public string TypeActionName { get; set; } = null!;
    }
}
