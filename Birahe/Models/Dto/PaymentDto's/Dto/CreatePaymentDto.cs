namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;

public class CreatePaymentDto
{
    public int UserId { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; } = "Signup payment";
}