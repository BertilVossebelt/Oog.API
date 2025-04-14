namespace API.v1.api.Role.CreateRole.Requests;

public class CreateRoleRequest
{
    public int Id { get; set; }
    public int EnvId { get; set; }
    public string Name { get; set; }
}