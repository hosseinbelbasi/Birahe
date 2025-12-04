namespace Birahe.EndPoint.Entities;

public class Discount : BaseEntity {
    public string Key { get; set; } = "";
    public int Percent { get; set; }
    public string Title { get; set; } = "";

    public DateTime ExpiresAt { get; set; }
}