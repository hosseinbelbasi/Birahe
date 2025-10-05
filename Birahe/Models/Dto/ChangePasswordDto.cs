namespace Birahe.EndPoint.Models.Dto;

public class ChangePasswordDto {
    public string Username { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }

    public string NewPasswordRepeated { get; set; }
}