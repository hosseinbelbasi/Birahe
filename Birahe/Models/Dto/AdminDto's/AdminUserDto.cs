using Birahe.EndPoint.Enums;
using Birahe.EndPoint.Models.Dto.UserDto_s;

namespace Birahe.EndPoint.Models.Dto.AdminDto_s;

public class AdminUserDto {
    public string Username { get; set; }
    public string TeamName { get; set; }
    public ICollection<StudentDto> Students { get; set; }
    public int Coin { get; set; }
    public Role Role { get; set; }
}