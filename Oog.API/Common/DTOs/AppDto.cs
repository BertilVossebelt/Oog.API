namespace API.Common.DTOs;

public class AppDto
{
   public required long Id { get; set; }
   public required long EnvId { get; set; }
   public required string Name { get; set; }
   public required string Passkey { get; set; }
}