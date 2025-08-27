namespace CandleUp.Models;

public abstract class Birthday
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime DateOfBirth { get; set; }
    public string? Notes { get; set; }
}