using API.v1.api.Log.CreateLog;
using API.v1.api.Log.CreateLog.Exceptions;
using API.v1.api.Log.CreateLog.Interfaces;
using API.v1.api.Log.CreateLog.Requests;
using NSubstitute;
using NUnit.Framework;
using Oog.Domain;
using Oog.Domain.Enums;

namespace Tests;

public class CreateLogTests
{
    private ICreateLogRepository _repository;
    private CreateLogHandler _handler;
    private CreateLogRequest _validRequest;
    private int _validAppId;
    private App _existingApp;
    
    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<ICreateLogRepository>();
        _handler = new CreateLogHandler(_repository);
        _validAppId = 123;
        
        _validRequest = new CreateLogRequest(
            Severity.Error,
            "Test error message",
            ["test", "error"],
            ["admin"]
        );
        
        _existingApp = new App
        {
            Id = _validAppId,
            EnvId = 456,
            Name = "Test App",
            Passkey = Guid.NewGuid().ToString()
        };
        
        _repository.CheckIfAppExists(_validAppId).Returns(_existingApp);
    }
    
    [Test]
    public void Create_WithNonExistentApp_ThrowsAppDoesNotExistException()
    {
        // Arrange
        _repository.CheckIfAppExists(_validAppId).Returns((App?)null);
        
        // Act & Assert
        Assert.ThrowsAsync<AppDoesNotExistException>(() => _handler.Create(_validRequest, _validAppId));
    }
    
    [Test]
    public async Task Create_SetsCurrentTimeAsTimestamp()
    {
        // Arrange
        Log capturedLog = null;
        _repository.Create(Arg.Do<Log>(log => capturedLog = log)).Returns(Task.FromResult(1));
    
        // Act
        await _handler.Create(_validRequest, _validAppId);
    
        // Assert
        Assert.That(capturedLog, Is.Not.Null);
    
        var now = DateTime.UtcNow;
        var timeDifference = Math.Abs((now - capturedLog.LogDateTime).TotalSeconds);
        Assert.That(timeDifference, Is.LessThan(1));
    }
}