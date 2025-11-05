using Birahe.EndPoint.Constants.Enums;

namespace Birahe.EndPoint.Entities;

public class Riddle : BaseEntity {
    public string Department { get; set; }
    public int Level { get; set; }
    public int No { get; set; }
    public string Content { get; set; }
    public string? HintFileName { get; set; }

    public MediaType? HintMediaType { get; set; }
    public string? RewardFileName { get; set; }

    public MediaType? RewardMediaType { get; set; }
    public int OpeningCost { get; set; }
    public int HintCost { get; set; }
    public int Reward { get; set; }
    public string RiddleUId { get; set; }
    public string Answer { get; set; }

    public string Format { get; set; }

    public List<ContestItem>? ContestItems { get; set; }
}