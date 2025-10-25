namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;

public class ZarinpalRequestData {
    public int Code { get; set; }
    public string Authority { get; set; } = null!;

    public int Fee { get; set; }

    public string FeeType { get; set; }

    public string Mmessage { get; set; }

}