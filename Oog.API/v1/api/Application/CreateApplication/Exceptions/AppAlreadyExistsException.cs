namespace API.v1.api.Application.CreateApplication.Exceptions;

public class AppAlreadyExistsException(string message) : Exception(message);