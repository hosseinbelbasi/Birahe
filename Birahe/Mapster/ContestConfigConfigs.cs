using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using Mapster;

namespace Birahe.EndPoint.Mapster;

public class ContestConfigConfigs {
    public static void AddConfigs() {
        TypeAdapterConfig<SetContestConfigDto, ContestConfig>
            .NewConfig()
            .Ignore(dest => dest.Id);
        TypeAdapterConfig<SetContestConfigDto, ContestConfig>
            .NewConfig()
            .Map(dest => dest.context, src => MapKeyToContext(src.Key));
    }

    private static String MapKeyToContext(string key) {
        return key switch {
            "Contest" => "مرحله ابتدایی",
            "FinalContest" => "مرحله نهایی",
            "Signup" => "ثبت نام",
            _ => key
        };
    }
}