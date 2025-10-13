using Birahe.EndPoint.Enums;

namespace Birahe.EndPoint.Entities;

public class User : BaseEntity {
    public string Username { get; set; }
    public string Passwordhashed { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
    public int Coin { get; set; }

    public string SerialNumber { get; set; }

    public Role Role { get; set; } = Role.User;

    public bool IsBanned { get; set; } = false;

    public string? BanReason { get; set; }

    public DateTime? BanDateTime { get; set; }

    public List<ContestItem>? ContestItems { get; set; }

    public User()
    {
        SerialNumber = Guid.NewGuid().ToString("N").Substring(0, 10);
    }


}