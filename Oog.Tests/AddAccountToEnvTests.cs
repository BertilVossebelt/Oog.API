using API.v1.api.Environment.AddAccountToEnvironment;
using API.v1.api.Environment.AddAccountToEnvironment.Exceptions;
using API.v1.api.Environment.AddAccountToEnvironment.Interfaces;
using API.v1.api.Environment.AddAccountToEnvironment.Requests;
using NSubstitute;
using NUnit.Framework;
using Oog.Domain;
using System.Threading.Tasks;

namespace Tests;

[TestFixture]
public class AddAccountToEnvTests
{
    private AddAccountToEnvHandler _handler;
    private IAddAccountToEnvRepository _repository;
    private AddAccountToEnvRequest _validRequest;
    private EnvAccount _createdEnvAccount;
    private int _requestingAccountId;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IAddAccountToEnvRepository>();
        _handler = new AddAccountToEnvHandler(_repository);
        _requestingAccountId = 1;
        
        _validRequest = new AddAccountToEnvRequest(
            username: "testuser@example.com",
            envId: 5
        );

        var accountResponse = new Account
        {
            Id = 2,
            Username = _validRequest.Username,
            Password = "hashed_password"
        };

        var envOwnerAccount = new EnvAccount 
        {
            AccountId = 3,
            EnvId = _validRequest.EnvId,
        };

        _createdEnvAccount = new EnvAccount
        {
            AccountId = accountResponse.Id,
            EnvId = _validRequest.EnvId,
        };

        _repository.GetEnvOwnerId(_validRequest.EnvId).Returns(Task.FromResult<EnvAccount?>(envOwnerAccount));
        _repository.GetAccountIdFromUsername(_validRequest.Username).Returns(Task.FromResult<Account?>(accountResponse));
        _repository.AddAccountToEnv(Arg.Any<EnvAccount>()).Returns(Task.FromResult<EnvAccount?>(_createdEnvAccount));
    }

    [Test]
    public async Task AddAccountToEnv_WithValidRequest_ReturnsEnvAccount()
    {
        // Act
        var result = await _handler.AddAccountToEnv(_validRequest, _requestingAccountId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(_createdEnvAccount));
    }

    [Test]
    public void AddAccountToEnv_WithNonexistentEnvironment_ThrowsEnvNotFoundException()
    {
        // Arrange
        _repository.GetEnvOwnerId(_validRequest.EnvId).Returns(Task.FromResult<EnvAccount?>(null));

        // Act & Assert
        Assert.ThrowsAsync<EnvNotFoundException>(
            async () => await _handler.AddAccountToEnv(_validRequest, _requestingAccountId));
    }

    [Test]
    public void AddAccountToEnv_WithNonexistentUsername_ThrowsIncorrectUsernameException()
    {
        // Arrange
        _repository.GetAccountIdFromUsername(_validRequest.Username).Returns(Task.FromResult<Account?>(null));

        // Act & Assert
        Assert.ThrowsAsync<IncorrectUsernameException>(
            async () => await _handler.AddAccountToEnv(_validRequest, _requestingAccountId));
    }

    [Test]
    public async Task AddAccountToEnv_WithValidData_CreatesCorrectEnvAccount()
    {
        // Arrange
        var accountResponse = new Account
        {
            Id = 2,
            Username = _validRequest.Username,
            Password = "hashed_password"
        };
        _repository.GetAccountIdFromUsername(_validRequest.Username).Returns(Task.FromResult<Account?>(accountResponse));

        // Act
        var result = await _handler.AddAccountToEnv(_validRequest, _requestingAccountId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.AccountId, Is.EqualTo(accountResponse.Id));
            Assert.That(result.EnvId, Is.EqualTo(_validRequest.EnvId));
        });
    }
}