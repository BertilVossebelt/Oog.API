using API.Common.DTOs;
using API.v1.api.Environment.GetAccountsFromEnvironment;
using API.v1.api.Environment.GetAccountsFromEnvironment.Exceptions;
using API.v1.api.Environment.GetAccountsFromEnvironment.Interfaces;
using API.v1.api.Environment.GetAccountsFromEnvironment.Requests;
using NSubstitute;
using NUnit.Framework;
using Oog.Domain;
namespace Tests;

[TestFixture]
public class GetAccountsFromEnvTests
{
    private GetAccountsFromEnvHandler _handler;
    private IGetAccountsFromEnvRepository _repository;
    private GetAccountsFromEnvRequest _validRequest;
    private int _accountId;
    private EnvAccount _envOwner;
    private List<AccountDto> _accountList;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IGetAccountsFromEnvRepository>();
        _handler = new GetAccountsFromEnvHandler(_repository);
        _accountId = 1;
        _validRequest = new GetAccountsFromEnvRequest(5);

        _envOwner = new EnvAccount
        {
            AccountId = _accountId,
            EnvId = _validRequest.EnvId,
            Owner = true
        };

        _accountList =
        [
            new AccountDto
            {
                Id = 1,
                Username = "user1@example.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },

            new AccountDto
            {
                Id = 2,
                Username = "user2@example.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        ];

        _repository.GetEnvOwnerId(_accountId).Returns(_envOwner);

        _repository.GetAccountsFromEnv(_accountId, _validRequest.EnvId)
            .Returns(Task.FromResult<IEnumerable<AccountDto>>(_accountList));
    }

    [Test]
    public async Task GetAccountsFromEnv_WithValidRequest_ReturnsAccounts()
    {
        // Act
        var result = await _handler.GetAccountsFromEnv(_validRequest, _accountId);

        // Assert
        var accountDtos = result as AccountDto[] ?? result.ToArray();
        Assert.That(accountDtos, Is.Not.Null);

        foreach (var expectedAccount in _accountList)
        {
            Assert.That(accountDtos, Has.Some.Matches<AccountDto>(a =>
                a.Id == expectedAccount.Id &&
                a.Username == expectedAccount.Username
            ));
        }
    }
}