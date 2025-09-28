namespace Birahe.EndPoint.Models.Dto;

public class UserDto {
    public string Username { get; set; }
    public ICollection<StudentDto> Students { get; set; }
    public int Coin { get; set; }

}