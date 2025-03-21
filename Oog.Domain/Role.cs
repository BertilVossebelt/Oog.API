namespace Oog.Domain;

public class Role(string name)
{
    public uint Id { get; set; }
    public uint EnvironmentId { get; set; }
    public string Name { get; set; } = name;
}