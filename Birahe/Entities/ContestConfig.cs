namespace Birahe.EndPoint.Entities;

public class ContestConfig : BaseEntity {
    public int Id { get; set; }
    public string Key { get; set; }
    public string context { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}