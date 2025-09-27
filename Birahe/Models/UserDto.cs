using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;

public class UserDto {
    [Username]
    public string UserName { get; set; }
    public ICollection<StudentDto> Students { get; set; }
    public int Coin { get; set; }
}