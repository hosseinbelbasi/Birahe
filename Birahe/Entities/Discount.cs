namespace Birahe.EndPoint.Entities;

public class Discount : BaseEntity {
    public string Key { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Percent { get; set;}
    public string Title { get; set; } = "";
}