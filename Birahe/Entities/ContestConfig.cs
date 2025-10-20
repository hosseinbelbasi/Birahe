namespace Birahe.EndPoint.Entities;

public class ContestConfig : BaseEntity {
    public int Id { get; set; }
    public string Key { get; set; } = "Contest";
    public DateTime StartTime { get; set; }
}