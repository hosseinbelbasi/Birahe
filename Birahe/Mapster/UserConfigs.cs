using Birahe.EndPoint.Entities;
using Birahe.EndPoint.Models;
using Birahe.EndPoint.Models.Dto;
using Mapster;

namespace Birahe.EndPoint.Mapster;

public class UserConfigs  {
    public static void AddConfigs() {

        TypeAdapterConfig<UserDto, User>
            .NewConfig()
            .Ignore(dest => dest.Coin);
        TypeAdapterConfig<SignUpDto, User>
            .NewConfig()
            .Map(dest => dest.Passwordhashed, src => src.Password.Hash());
        TypeAdapterConfig<User, UserDto>
            .NewConfig()
            .Map(dest => dest.Students, src => src.Students);

    }
}