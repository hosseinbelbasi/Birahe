namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;

public class ZarinpalVerifyResponse {
    public ZarinpalVerifyData? Data { get; set; }
    public List<ZarinpalError>? Errors { get; set; }
}