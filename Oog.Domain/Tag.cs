namespace Oog.Domain;

public class Tag(string name)
{
    public int Id { get; set; }
    public int EnvId { get; set; }
    public string Name { get; set; } = name;
}