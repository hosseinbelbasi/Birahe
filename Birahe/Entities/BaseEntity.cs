namespace Birahe.EndPoint.Entities;

public class BaseEntity {
    public BaseEntity() {
        CreationDateTime = DateTime.UtcNow;
    }

    public int Id { get; set; }

    public DateTime CreationDateTime { get; set; }
    public DateTime? ModificationDateTime { get; set; }

    public DateTime? RemoveTime { get; set; }
}