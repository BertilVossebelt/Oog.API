namespace API.Common.DTOs;

public class AppDto
{
   public required int Id { get; set; }
   public required int EnvId { get; set; }
   public required string Name { get; set; }
   public required string Passkey { get; set; }
}