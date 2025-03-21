namespace Oog.Domain;

public class Account
{
   public long Id { get; set; }
   public required string Username { get; set; }
   public required string Password { get; set; }
   public DateTime CreatedAt { get; set; }
   public DateTime UpdatedAt { get; set; }
}