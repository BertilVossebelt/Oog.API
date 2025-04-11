namespace API.v1.api.Account.CreateAccount.Exceptions;

public class UsernameAlreadyExistsException(string message) : Exception(message);