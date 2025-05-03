using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Account.ReadAccountRoles.Request;

[method: JsonConstructor]
public class ReadAccountRolesRequest(int accountId, int envId)
{
    [Required]
    public int AccountId { get; set; } = accountId;

    [Required]
    public int EnvId { get; set; } = envId;
}