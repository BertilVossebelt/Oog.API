namespace API.Common.DTOs;

public class EnvironmentDto
{
    public required long Id { get; set; }
    public required string Name { get; set; }
    public required long OwnerId { get; set; }
}