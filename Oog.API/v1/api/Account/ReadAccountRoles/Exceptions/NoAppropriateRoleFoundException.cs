namespace API.v1.api.Account.ReadAccountRoles.Exceptions;

public class NoAppropriateRoleFoundException(string message) : Exception(message);