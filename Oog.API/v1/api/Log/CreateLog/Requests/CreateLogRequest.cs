using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Oog.Domain.Enums;

namespace API.v1.api.Log.CreateLog.Requests;

[method: JsonConstructor]
public class CreateLogRequest(Severity severity, string message, List<string> tags, List<string> roles)
{
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Severity Severity { get; set; } = severity;

    [Required] 
    public string Message { get; set; } = message;

    public List<string> Tags { get; set; } = tags;
    
    public List<string> Roles { get; set; } = roles;
}