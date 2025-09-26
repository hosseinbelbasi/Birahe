using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;

public class UserDto {
    [Required]
    [NotWhiteSpace]
    [MaxLength(70)]
    [MinLength(4)]
    public string UserName { get; set; }
    public ICollection<StudentDto> Students { get; set; }
    public int Coin { get; set; }
}