namespace Birahe.EndPoint.Models.Dto.AdminDto_s;

public class AdminContestItemDto {
    public string Department { get; set; }

    public int Level { get; set; }

    public int No { get; set; }

    public DateTime OpeningDateTime { get; set; }

    public bool IsSolved { get; set; } = false;
    public DateTime? SolvingDateTime { get; set; }

    public int Tries { get; set; } = 0;
    public DateTime? LastTryDateTime { get; set; }
    public string? LastAnswer { get; set; }

    public bool HasOpenedHint { get; set; } = false;
    public DateTime? OpeningHintDateTime { get; set; }
}