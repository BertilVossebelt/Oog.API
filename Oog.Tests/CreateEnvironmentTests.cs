using API.Common.DTOs;
using API.v1.api.Environment.CreateEnvironment;
using API.v1.api.Environment.CreateEnvironment.Interfaces;
using API.v1.api.Environment.CreateEnvironment.Requests;
using AutoMapper;
using NSubstitute;
using NUnit.Framework;
using Oog.Domain;
using Environment = Oog.Domain.Environment;

namespace Tests;

[TestFixture]
public class CreateEnvironmentTests
{
    private CreateEnvironmentHandler _handler;
    private ICreateEnvironmentRepository _repository;
    private IMapper _mapper;
    private CreateEnvironmentRequest _validRequest;
    private int _accountId;
    private Environment _createdEnv;
    private EnvAccount _createdEnvAccount;
    private List<Role> _createdRoles;
    private AccountRole _createdAccountRole;
    private EnvironmentDto _expectedDto;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<ICreateEnvironmentRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateEnvironmentHandler(_repository, _mapper);
        _accountId = 1;
        
        _validRequest = new CreateEnvironmentRequest("Test Environment");

        _createdEnv = new Environment
        {
            Id = 10,
            Name = _validRequest.Name
        };

        _createdEnvAccount = new EnvAccount
        {
            AccountId = _accountId,
            EnvId = _createdEnv.Id,
        };

        _createdRoles =
        [
            new Role { Id = 1, Name = "Owner", EnvId = _createdEnv.Id },
            new Role { Id = 2, Name = "Maintainer", EnvId = _createdEnv.Id }
        ];

        _createdAccountRole = new AccountRole
        {
            AccountId = _accountId,
            RoleId = _createdRoles[0].Id
        };

        _repository.Create(
            Arg.Any<Environment>(),
            Arg.Any<EnvAccount>(),
            Arg.Any<List<Role>>(),
            Arg.Any<int>()
        ).Returns(Task.FromResult<(Environment?, EnvAccount?, IEnumerable<Role>?, AccountRole?)>(
            (_createdEnv, _createdEnvAccount, _createdRoles, _createdAccountRole)));

        _expectedDto = new EnvironmentDto
        {
            Id = _createdEnv.Id,
            Name = _createdEnv.Name,
            OwnerId = _createdEnvAccount.AccountId
        };

        _mapper.Map<EnvironmentDto>(Arg.Any<Environment>()).Returns(_expectedDto);
        _mapper.Map(Arg.Any<EnvAccount>(), Arg.Any<EnvironmentDto>()).Returns(_expectedDto);
    }

    [Test]
    public async Task Create_WithValidRequest_ReturnsEnvironmentDto()
    {
        // Act
        var result = await _handler.Create(_validRequest, _accountId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(_expectedDto));
    }
    
    [Test]
    public void Create_WhenEnvironmentCreationFails_ThrowsException()
    {
        // Arrange
        _repository.Create(Arg.Any<Environment>(), Arg.Any<EnvAccount>(), Arg.Any<List<Role>>(), Arg.Any<int>())
            .Returns(Task.FromResult<(Environment?, EnvAccount?, IEnumerable<Role>?, AccountRole?)>((null, _createdEnvAccount, _createdRoles, _createdAccountRole)));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _handler.Create(_validRequest, _accountId));
    }

    [Test]
    public void Create_WhenEnvAccountCreationFails_ThrowsException()
    {
        // Arrange
        _repository.Create(Arg.Any<Environment>(), Arg.Any<EnvAccount>(), Arg.Any<List<Role>>(), Arg.Any<int>()
        ).Returns(Task.FromResult<(Environment?, EnvAccount?, IEnumerable<Role>?, AccountRole?)>((_createdEnv, null, _createdRoles, _createdAccountRole)));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _handler.Create(_validRequest, _accountId));
    }

    [Test]
    public void Create_WhenRolesCreationFails_ThrowsException()
    {
        // Arrange
        _repository.Create(Arg.Any<Environment>(), Arg.Any<EnvAccount>(), Arg.Any<List<Role>>(), Arg.Any<int>())
            .Returns(Task.FromResult<(Environment?, EnvAccount?, IEnumerable<Role>?, AccountRole?)>((_createdEnv, _createdEnvAccount, null, _createdAccountRole)));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _handler.Create(_validRequest, _accountId));
    }
}