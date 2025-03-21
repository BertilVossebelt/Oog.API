namespace Oog.Domain;

public class EnvAccount
{
    public required long EnvId { get; set; }
    public required long AccountId { get; set; }
    public required bool Owner {  get; set; }
}