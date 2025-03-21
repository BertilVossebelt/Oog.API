using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.Environment.GetAccountsFromEnvironment.Requests;

[method: JsonConstructor]
public class GetAccountsFromEnvRequest(long envId)
{
    [Required] 
    public long EnvId { get; set; } = envId;
}