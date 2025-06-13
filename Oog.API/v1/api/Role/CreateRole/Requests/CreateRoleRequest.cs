using System.Text.Json.Serialization;

namespace API.v1.api.Role.CreateRole.Requests;

[method: JsonConstructor]
public class CreateRoleRequest(int envId, string name)
{
    public int EnvId { get; set; } = envId;
    public string Name { get; set; } = name;
}