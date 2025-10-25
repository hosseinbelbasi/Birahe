namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;

public class ZarinpalRequestResponse {
    public ZarinpalRequestData? Data { get; set; }
    public List<ZarinpalError>? Errors { get; set; }
}