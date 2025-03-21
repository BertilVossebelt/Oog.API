using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.v1.Application.CreateApplication.Requests;

[method: JsonConstructor]
public class CreateAppRequest(long envId, string name)
{
    [Required]
    public long EnvId { get; set; } = envId;

    [Required]
    public string Name { get; set; } = name;
}