using Oog.Domain;

namespace API.Common.DTOs;

public class EnvironmentDto
{
    public required uint Id { get; set; }
    public required string Name { get; set; }
    public required ulong OwnerId { get; set; }
}