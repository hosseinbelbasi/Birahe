using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models.Dto;
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



    }
}