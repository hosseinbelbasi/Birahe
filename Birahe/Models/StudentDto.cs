using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;


public class StudentDto {
    [Required]
    [NotWhiteSpace]
    [MinLength(3)]
    [MaxLength(70)]
    public string FirstName { get; set; }
    [Required]
    [NotWhiteSpace]
    [MinLength(3)]
    [MaxLength(70)]
    public string LastName { get; set; }
    [Required]
    [NotWhiteSpace]
    [MinLength(10)]
    [MaxLength(11)]

    public string StudentNo { get; set; }
    [Required]
    [NotWhiteSpace]
    [MinLength(4)]
    [MaxLength(100)]
    public string Field { get; set; }
    [Required]
    public bool IsMale { get; set; }
}