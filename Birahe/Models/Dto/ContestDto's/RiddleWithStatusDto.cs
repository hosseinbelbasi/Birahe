namespace Birahe.EndPoint.Models.Dto.ContestDto_s;

public class RiddleWithStatusDto {
    public bool IsOpened { get; set; }
    public bool HasOpenedHint { get; set; }
    public bool IsSolved { get; set; }
    public int Id { get; set; }
    public string Department { get; set; }
    public int Level { get; set; }
    public int No { get; set; }
    public string? Content { get; set; }

    public int OpeningCost { get; set; }

    public int HintCost { get; set; }
}