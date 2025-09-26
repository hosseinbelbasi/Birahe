using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;

public class SignUpDto {
    [Required]
    public ICollection<StudentDto> Students { get; set; } = new List<StudentDto>();
    [Required]
    [MaxLength(70)]
    [NotWhiteSpace]
    [MinLength(8)]
    public string Password { get; set; }
}