namespace Birahe.EndPoint.Models.Dto.AdminDto_s;

public class AddRiddleDto {
    public string Department { get; set; }
    public int Level { get; set; }

    public int No { get; set; }
    public string Content { get; set; }

    public int OpeningCost { get; set; }
    public int HintCost { get; set; }
    public int Reward { get; set; }

    public string Format { get; set; }

    public string Answer { get; set; }
}