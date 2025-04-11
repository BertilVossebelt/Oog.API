using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Authentication.AuthenticateApplication.Requests;

[method: JsonConstructor]
public class AuthenticateAppRequest(string name, string passKey)
{
    [Required]
    public string Name { get; set; } = name;
    
    [Required]
    [Length(64,64)]
    public string PassKey { get; set; } = passKey;
}