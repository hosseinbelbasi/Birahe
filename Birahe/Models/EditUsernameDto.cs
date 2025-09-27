using System.ComponentModel.DataAnnotations;
using Birahe.EndPoint.Attribute;

namespace Birahe.EndPoint.Models;

public class EditUsernameDto {
    [Username]
    public string OldUsername { get; set; }
    [Username]
    public string NewUsername { get; set; }
    [Password]
    public string Password { get; set; }
}