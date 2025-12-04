namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;

public class ZarinpalVerifyResponse {
    public ZarinpalVerifyData? data { get; set; }
    public List<ZarinpalError>? errors { get; set; }
}