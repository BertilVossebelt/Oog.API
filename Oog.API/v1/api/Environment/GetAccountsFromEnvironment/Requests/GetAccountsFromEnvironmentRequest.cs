using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Environment.GetAccountsFromEnvironment.Requests;

[method: JsonConstructor]
public class GetAccountsFromEnvRequest(int envId)
{
    [Required] 
    public int EnvId { get; set; } = envId;
}