namespace Birahe.EndPoint.Models.Dto.UserDto_s;

public class ChangePasswordDto {

    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public string NewPasswordRepeated { get; set; }
}