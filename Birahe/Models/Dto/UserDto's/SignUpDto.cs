namespace Birahe.EndPoint.Models.Dto.UserDto_s;

public class SignUpDto {
    public ICollection<StudentDto> Students { get; set; } = new List<StudentDto>();

    public string Username { get; set; }
    public string Password { get; set; }
}