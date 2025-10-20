using System.Runtime.InteropServices.JavaScript;

namespace Birahe.EndPoint.Entities;

public class ContestItem : BaseEntity {
    public User User { get; set; }
    public int UserId { get; set; }

    public Riddle Riddle { get; set; }
    public int RiddleId { get; set; }

    public DateTime OpeningDateTime { get; set; }

    public bool IsSolved { get; set; } = false;
    public DateTime? SolvingDateTime { get; set; }

    public int Tries { get; set; } = 0;
    public DateTime? LastTryDateTime { get; set; }
    public string? LastAnswer { get; set; }

    public bool HasOpenedHint { get; set; } = false;
    public DateTime? OpeningHintDateTime { get; set; }
}