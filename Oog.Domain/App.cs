namespace Oog.Domain;

public class App
{
    public int Id { get; set; }
    public required int EnvId { get; set; }
    public required string Name { get; set; }
    public required string Passkey { get; set; }
}