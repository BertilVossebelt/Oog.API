using API.Common.DTOs;
using API.v1.api.Application.CreateApplication;
using API.v1.api.Application.CreateApplication.Interfaces;
using API.v1.api.Application.CreateApplication.Requests;
using AutoMapper;
using NSubstitute;
using Oog.Domain;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class CreateAppTests
{
    private CreateAppHandler _handler;
    private ICreateAppRepository _repository;
    private IMapper _mapper;
    private CreateAppRequest _validRequest;
    private App _createdApp;
    private AppDto _appDto;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<ICreateAppRepository>();
        _mapper = Substitute.For<IMapper>();
        
        _handler = new CreateAppHandler(_repository);

        _validRequest = new CreateAppRequest(
            name: "TestApp",
            envId: 5
        );

        _createdApp = new App
        {
            Name = "TestApp",
            EnvId = 5,
            Passkey = "hashed_passkey"
        };

        _appDto = new AppDto
        {
            Id = 1,
            Name = "TestApp",
            EnvId = 5,
            Passkey = "unhashed_passkey"
        };

        _mapper.Map<AppDto>(Arg.Any<App>()).Returns(_appDto);
    }

    [Test]
    public async Task Create_WithValidRequest_ReturnsAppDto()
    {
        // Arrange
        _repository.Create(Arg.Any<App>()).Returns(_createdApp);

        // Act
        var result = await _handler.Create(_validRequest, _mapper);

        // Assert
        Assert.That(result, Is.EqualTo(_appDto));
    }

    [Test]
    public async Task Create_HashesPasskey()
    {
        // Arrange
        App? capturedApp = null;
        _repository.Create(Arg.Do<App>(app => capturedApp = app)).Returns(_createdApp);

        // Act
        await _handler.Create(_validRequest, _mapper);

        // Assert
        Assert.That(capturedApp, Is.Not.Null);
        Assert.That(capturedApp.Passkey, Is.Not.Null);
        Assert.That(capturedApp.Passkey, Does.StartWith("$2a$")); // BCrypt hash prefix
    }
    
    [Test]
    public async Task Create_CreatesAppWithCorrectProperties()
    {
        // Arrange
        App? capturedApp = null;
        _repository.Create(Arg.Do<App>(app => capturedApp = app)).Returns(_createdApp);

        // Act
        await _handler.Create(_validRequest, _mapper);

        // Assert
        Assert.That(capturedApp, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(capturedApp.Name, Is.EqualTo(_validRequest.Name));
            Assert.That(capturedApp.EnvId, Is.EqualTo(_validRequest.EnvId));
        });
    }
}