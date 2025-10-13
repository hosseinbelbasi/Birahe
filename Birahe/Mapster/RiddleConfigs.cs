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
            .Map(dest => dest.Asnwer, src => src.Asnswer);

        TypeAdapterConfig<Riddle, AdminRiddleDto>
            .NewConfig()
            .Map(dest => dest.HintImage, src => !String.IsNullOrEmpty(src.HintImageFileName))
            .Map(dest => dest.RewardImage, src => !String.IsNullOrEmpty(src.RewardImageFileName));




    }
}