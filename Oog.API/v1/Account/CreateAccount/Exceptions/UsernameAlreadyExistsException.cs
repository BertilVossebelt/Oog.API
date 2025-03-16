namespace API.v1.Account.CreateAccount.Exceptions;

public class UsernameAlreadyExistsException(string message) : Exception(message);