using System.ComponentModel.DataAnnotations;
using Oog.Domain.Enums;

namespace Oog.Domain;

public class Log
{
    public int Id { get; set; }
    public required Severity Severity { get; set; }
    public required string Message { get; set; }
    public List<string> Tags { get; set; } = [];
    public List<string> Roles { get; set; } = [];
    public required DateTime LogDateTime { get; set; }
    public required int EnvId { get; set; }
    public required int AppId { get; set; }
}