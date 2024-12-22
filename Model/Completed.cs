using System.ComponentModel.DataAnnotations;

public class Completed
{
    [Key]
    public int IDFinished { get; set; }
    public required string Name { get; set; }

}