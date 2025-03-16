namespace Oog.Domain;

public class Role
{
    public uint Id { get; set; }
    public uint EnvironmentId { get; set; }
    public string Name { get; set; }
    
    List<Tag>? Tags { get; set; }
    List<Account>? Accounts { get; set; }
}