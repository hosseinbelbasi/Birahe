using Birahe.EndPoint.Enums;

namespace Birahe.EndPoint.Entities;

public class User : BaseEntity {
    public string UserName { get; set; }
    public string Passwordhashed { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public int Coin { get; set; }

    public string SerialNumber { get; set; }

    public Role Role { get; set; } = Role.User;

    public User()
    {
        SerialNumber = Guid.NewGuid().ToString("N").Substring(0, 10);
    }


}