namespace Oog.Domain;

public class Tag
{
    public uint Id { get; set; }
    public uint EnvironmentId { get; set; }
    public string Name { get; set; }

    List<Role>? Roles { get; set; }
}