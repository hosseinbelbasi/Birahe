using System.Text.Json;
using System.Text.Json.Serialization;

namespace Birahe.EndPoint.Models.Dto.PaymentDto_s.ResponseModels;


public class ZarinpalVerifyData {
    public int code { get; set; }


    public string? card_hash { get; set; }
    
    public string? card_pan { get; set; }
    public Int128? ref_id { get; set; }


    public string? fee_type { get; set; }

    public long fee { get; set; }
}


