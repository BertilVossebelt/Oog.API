namespace Oog.Domain;

public class Application(string name, string passKey)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string PassKey { get; set; } = passKey;
}
