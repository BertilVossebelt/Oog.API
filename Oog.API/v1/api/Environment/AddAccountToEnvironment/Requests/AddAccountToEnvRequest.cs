using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Environment.AddAccountToEnvironment.Requests;

[method: JsonConstructor]
public class AddAccountToEnvRequest(int envId, string username)
{
    [Required] 
    public int EnvId { get; set; } = envId;
    
    [Required] 
    [EmailAddress]
    public string Username { get; set; } = username;
}