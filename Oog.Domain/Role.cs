namespace Oog.Domain;

public class Role
{
    public int Id { get; set; }
    public int EnvId { get; set; }
    public required string Name { get; set; }
}