using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;

public class SignUpDto {
    [Required]
    public ICollection<StudentDto> Students { get; set; } = new List<StudentDto>();
    [Username]

    public string Username { get; set; }
    [Password]
    public string Password { get; set; }
}