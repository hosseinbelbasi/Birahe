using Birahe.EndPoint.Constants.Enums;

namespace Birahe.EndPoint.Models.Dto.AdminDto_s;

public class AdminRiddleDto {
    public int Id { get; set; }

    public string Department { get; set; }
    public int Level { get; set; }

    public int No { get; set; }
    public string Content { get; set; }
    public int OpeningCost { get; set; }
    public int HintCost { get; set; }

    public bool HintFile { get; set; }
    public MediaType HintMediaType { get; set; }

    public bool RewardFile { get; set; }

    public MediaType RewardMediaType { get; set; }

    public int Reward { get; set; }

    public string Format { get; set; }


    public string Answer { get; set; }
}