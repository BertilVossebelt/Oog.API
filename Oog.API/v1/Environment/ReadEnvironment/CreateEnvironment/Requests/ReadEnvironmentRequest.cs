using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.Environment.ReadEnvironment.CreateEnvironment.Requests;

[method: JsonConstructor]
public class ReadEnvironmentRequest(string name)
{
    [Required] public string Name { get; set; } = name;
}