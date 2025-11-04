using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto.AdminDto_s;
using Mapster;

namespace Birahe.EndPoint.Mapster;

public class ContestConfigConfigs {
    public static void AddConfigs() {
        TypeAdapterConfig<SetContestConfigDto, ContestConfig>
            .NewConfig()
            .Ignore(dest => dest.Id);

    }


}