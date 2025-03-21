using Oog.Domain;
using Environment = Oog.Domain.Environment;

namespace API.Common.DTOs;

public class AccountDto
{
   public required long Id { get; set; }
   public required string Username { get; set; }
   public required DateTime CreatedAt { get; set; }
   public required DateTime UpdatedAt { get; set; }
}