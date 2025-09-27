using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;


public class StudentDto {
    [Name]
    public string FirstName { get; set; }
    [Name]
    public string LastName { get; set; }
    [StudentNo]
    public string StudentNo { get; set; }
    [Field]
    public string Field { get; set; }
    [Required]
    public bool IsMale { get; set; }
}