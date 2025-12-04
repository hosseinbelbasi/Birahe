namespace Birahe.EndPoint.Models.Dto.ContestDto_s;

public class AllRiddlesWithStatusDto {
    public ICollection<RiddleWithStatusDto>? riddles { get; set; }
    public int Solved { get; set; }
}