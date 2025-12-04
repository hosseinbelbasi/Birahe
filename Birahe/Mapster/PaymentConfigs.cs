using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto.PaymentDto_s.Dto;
using Mapster;

namespace Birahe.EndPoint.Mapster;

public class PaymentConfigs {
    public static void AddConfigs() {
        TypeAdapterConfig<Payment, PaymentVerifiedDto>
            .NewConfig()
            .Map(dest => dest.TeamName, src => src.User!.TeamName)
            .Map(dest => dest.Username, src => src.User!.Username)
            .Map(dest => dest.RefId, src => src.RefId)
            .Map(dest => dest.Amount, src => src.Amount);
    }
}