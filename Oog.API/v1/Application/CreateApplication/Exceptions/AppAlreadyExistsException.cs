namespace API.v1.Account.CreateAccount.Exceptions;

public class AppAlreadyExistsException(string message) : Exception(message);