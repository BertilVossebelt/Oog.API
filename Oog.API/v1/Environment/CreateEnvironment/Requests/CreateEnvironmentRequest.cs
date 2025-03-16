using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.Environment.CreateEnvironment.Requests;

[method: JsonConstructor]
public class CreateEnvironmentRequest(string name)
{
    [Required] public string Name { get; set; } = name;
}