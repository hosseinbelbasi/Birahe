using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;

public class LoginDto {
    [Required]
    [NotWhiteSpace]
    [MaxLength(70)]
    [MinLength(4)]
    public string UserName { get; set; }
    [Required]
    [NotWhiteSpace]
    [MaxLength(50)]
    [MinLength(8)]
    public string Password { get; set; }
}