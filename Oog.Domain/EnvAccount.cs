namespace Oog.Domain;

public class EnvAccount
{
    public required long EnvId { get; set; }
    public required long OwnerId { get; set; }
    public required bool Owner { get; set; }
}