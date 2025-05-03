using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Account.CreateAccount.Requests;

[method: JsonConstructor]
public class CreateAccountRequest(string username, string password, string confirmation)
{
    [Required]
    [EmailAddress]
    public string Username { get; set; } = username;

    [Required]
    [MinLength(12)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = password;

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string Confirmation { get; set; } = confirmation;
}