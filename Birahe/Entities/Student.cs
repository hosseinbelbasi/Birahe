namespace Birahe.EndPoint.Entities;

public class Student : BaseEntity {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string StudentNo { get; set; }
    public string Field { get; set; }
    public bool IsMale { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
}