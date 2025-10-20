namespace Birahe.EndPoint.Entities;

public class Riddle : BaseEntity {
    public string Department { get; set; }
    public int Level { get; set; }
    public int No { get; set; }
    public string Content { get; set; }
    public string? HintImageFileName { get; set; }
    public string? RewardImageFileName { get; set; }
    public int OpeningCost { get; set; }
    public int HintCost { get; set; }
    public int Reward { get; set; }
    public string RiddleUId { get; set; }
    public string Asnwer { get; set; }

    public List<ContestItem>? ContestItems { get; set; }
}