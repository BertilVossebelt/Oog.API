using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.Environment.AddAccountToEnvironment.Requests;

[method: JsonConstructor]
public class AddAccountToEnvRequest(long envId, string username)
{
    [Required] 
    public long EnvId { get; set; } = envId;
    
    [Required] 
    [EmailAddress]
    public string Username { get; set; } = username;
}