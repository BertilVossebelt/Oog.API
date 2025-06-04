using API.Common.DTOs;
using API.v1.api.Account.CreateAccount;
using API.v1.api.Account.CreateAccount.Exceptions;
using API.v1.api.Account.CreateAccount.Interfaces;
using API.v1.api.Account.CreateAccount.Requests;
using AutoMapper;
using NSubstitute;
using Oog.Domain;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class CreateAccountTests
{
    private CreateAccount _handler;
    private ICreateAccountRepository _repository;
    private IMapper _mapper;
    private CreateAccountRequest _validRequest;
    private Account _createdAccount;
    private AccountDto _accountDto;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<ICreateAccountRepository>();
        _mapper = Substitute.For<IMapper>();
        
        _handler = new CreateAccount(_repository, _mapper);

        _validRequest = new CreateAccountRequest(
            username: "testuser@example.com",
            password: "ValidP@ssw0rd123",
            confirmation: "ValidP@ssw0rd123"
        );

        _createdAccount = new Account
        {
            Username = _validRequest.Username,
            Password = "hashed_password" 
        };

        _accountDto = new AccountDto
        {
            Id = 1,
            Username = _validRequest.Username,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        // Setup mapper to return the account DTO
        _mapper.Map<AccountDto>(Arg.Any<Account>()).Returns(_accountDto);
    }

    [Test]
    public async Task Create_WithValidRequest_ReturnsExpectedAccountDto()
    {
        // Arrange
        _repository.CheckIfAccountExists(Arg.Any<Account>()).Returns((Account?)null);
        _repository.Create(Arg.Any<Account>()).Returns(_createdAccount);

        // Act
        var result = await _handler.Create(_validRequest);

        // Assert
        Assert.That(result, Is.EqualTo(_accountDto));
    }

    [Test]
    public void Create_WithExistingUsername_ThrowsUsernameAlreadyExistsException()
    {
        // Arrange
        var existingAccount = new Account { 
            Username = _validRequest.Username,
            Password = "existing_password"
        };
        _repository.CheckIfAccountExists(Arg.Any<Account>()).Returns(existingAccount);

        // Act & Assert
        var exception = Assert.ThrowsAsync<UsernameAlreadyExistsException>(
            async () => await _handler.Create(_validRequest));
        
        // Verify exception message contains relevant information
        Assert.That(exception.Message, Does.Contain(_validRequest.Username));
        Assert.That(exception.Message, Does.Contain("already exists"));
    }

    [Test]
    public async Task Create_PasswordIsHashedWithBCrypt()
    {
        // Arrange
        _repository.CheckIfAccountExists(Arg.Any<Account>()).Returns((Account?)null);
        
        Account? capturedAccount = null;
        _repository.Create(Arg.Do<Account>(acc => capturedAccount = acc)).Returns(_createdAccount);

        // Act
        await _handler.Create(_validRequest);

        // Assert
        Assert.That(capturedAccount, Is.Not.Null);
        Assert.That(BCrypt.Net.BCrypt.Verify(_validRequest.Password, capturedAccount.Password), Is.True);
        Assert.That(capturedAccount.Password, Is.Not.EqualTo(_validRequest.Password));
    }
}