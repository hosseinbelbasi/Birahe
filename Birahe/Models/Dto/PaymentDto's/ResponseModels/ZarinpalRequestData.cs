namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;

public class ZarinpalRequestData {
    public int code { get; set; }
    public string authority { get; set; } = null!;

    public int fee { get; set; }

    public string fee_type { get; set; }

    public string message { get; set; }
}