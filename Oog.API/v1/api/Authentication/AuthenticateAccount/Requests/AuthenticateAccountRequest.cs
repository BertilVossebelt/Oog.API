using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Authentication.AuthenticateAccount.Requests;

[method: JsonConstructor]
public class AuthenticateAccountRequest(string username, string password)
{
    [Required]
    [EmailAddress]
    public string Username { get; set; } = username;
    
    [Required]
    [MinLength(12)]
    public string Password { get; set; } = password;
}