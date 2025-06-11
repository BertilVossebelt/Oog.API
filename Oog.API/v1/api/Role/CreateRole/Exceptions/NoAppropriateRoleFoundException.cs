namespace API.v1.api.Role.CreateRole.Exceptions;

public class NoAppropriateRoleFoundException(string message) : Exception(message);