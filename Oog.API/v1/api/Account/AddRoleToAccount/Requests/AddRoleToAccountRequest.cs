using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Account.AddRoleToAccount.Requests;

[method: JsonConstructor]
public class AddRoleToAccountRequest(int envId, int accountId, List<int> roleIds)
{
    [Required]
    public int EnvId { get; set; } = envId;

    [Required] public int AccountId { get; set; } = accountId;
    
    [Required]
    public List<int> RoleIds { get; set; } = roleIds;}
