namespace Meta_xi.Application;
public class MisionsBack
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required int Goal { get; set; }
    public required int Progress { get; set; }
    public required bool Claimed { get; set; }
    public required float Reward { get; set; }
}