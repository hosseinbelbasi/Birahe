using Birahe.EndPoint.Constants.Enums;

namespace Birahe.EndPoint.Entities;

public class Payment : BaseEntity {
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public string Authority { get; set; } = null!;
    public int Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending; // Pending | Success | Failed
    public string? RefId { get; set; }
}