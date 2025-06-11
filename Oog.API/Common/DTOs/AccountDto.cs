namespace API.Common.DTOs;

public class AccountDto
{
   public required int Id { get; set; }
   public required string Username { get; set; }
   public required DateTime CreatedAt { get; set; }
   public required DateTime UpdatedAt { get; set; }
}