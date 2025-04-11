namespace API.v1.api.Log.CreateLog.Exceptions;

public class AppDoesNotExistException(string message) : Exception(message);