namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;

public class ZarinpalRequestResponse {
    public ZarinpalRequestData? data { get; set; }
    public List<ZarinpalError>? errors { get; set; }
}