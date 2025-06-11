namespace API.v1.api.Account.AddRoleToAccount.Exceptions;

public class NoAppropriateRoleFoundException(string message) : Exception(message);