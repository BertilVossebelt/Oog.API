namespace Oog.Domain;

public class App
{
    public long Id { get; set; }
    public required long EnvId { get; set; }
    public required string Name { get; set; }
    public required string Passkey { get; set; }
}