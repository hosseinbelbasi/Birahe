namespace Birahe.EndPoint.Entities;

public class Riddle : BaseEntity {
    public string Department { get; set; }
    public int Level { get; set; }
    public string Content { get; set; }
    public string Hint { get; set; }
    public int OpeningCost { get; set; }
    public int HintCost { get; set; }
    public int Reward { get; set; }
}