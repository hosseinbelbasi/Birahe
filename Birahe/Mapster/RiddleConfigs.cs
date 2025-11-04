using System.Security.Cryptography;
using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using Mapster;

namespace Birahe.EndPoint.Mapster;

public class RiddleConfigs {
    public static void AddConfigs() {
        TypeAdapterConfig<Riddle, Riddle>
            .NewConfig()
            .Ignore(dest => dest.RiddleUId);
        TypeAdapterConfig<Riddle, Riddle>
            .NewConfig()
            .Ignore(dest => dest.Id);


        TypeAdapterConfig<AdminRiddleDto, Riddle>
            .NewConfig()
            .Map(dest => dest.Answer, src => src.Answer);

        TypeAdapterConfig<Riddle, AdminRiddleDto>
            .NewConfig()
            .Map(dest => dest.HintFile, src => !String.IsNullOrEmpty(src.HintFileName))
            .Map(dest => dest.RewardFile, src => !String.IsNullOrEmpty(src.RewardFileName));
    }
}