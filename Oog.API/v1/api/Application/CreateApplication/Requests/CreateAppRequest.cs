using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.api.Application.CreateApplication.Requests;

[method: JsonConstructor]
public class CreateAppRequest(int envId, string name)
{
    [Required]
    public int EnvId { get; set; } = envId;

    [Required]
    public string Name { get; set; } = name;
}