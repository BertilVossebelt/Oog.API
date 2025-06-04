using API.v1.api.Account.AddRoleToAccount;
using API.v1.api.Account.AddRoleToAccount.Exceptions;
using API.v1.api.Account.AddRoleToAccount.Interfaces;
using API.v1.api.Account.AddRoleToAccount.Requests;
using AutoMapper;
using NSubstitute;
using Oog.Domain;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class AddRoleToAccountTests
{
    private AddRoleToAccountHandler _handler;
    private IAddRoleToAccountRepository _repository;
    private IMapper _mapper;
    private AddRoleToAccountRequest _validRequest;
    private int _accountId;
    private List<string> _ownerRoles;
    private AccountRole _createdAccountRole;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IAddRoleToAccountRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new AddRoleToAccountHandler(_repository, _mapper);

        _accountId = 1;
        _validRequest = new AddRoleToAccountRequest(
            accountId: 2,
            envId: 5,
            roleIds: [10, 11]
        );

        _ownerRoles = ["Owner"];
        _createdAccountRole = new AccountRole
        {
            AccountId = _validRequest.AccountId,
            RoleId = _validRequest.RoleIds.First()
        };
    }

    [Test]
    public async Task Add_WithValidRequest_ReturnsCreatedRoles()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId).Returns(_ownerRoles);
        _repository.Add(Arg.Any<AccountRole>()).Returns(_createdAccountRole);

        // Act
        var result = await _handler.Add(_validRequest, _accountId);

        // Assert
        var accountRoles = result as AccountRole[] ?? result.ToArray();
        Assert.That(accountRoles, Is.Not.Null);
        Assert.That(accountRoles, Has.Length.EqualTo(_validRequest.RoleIds.Count));
    }

    [Test]
    public void Add_WithoutAppropriateRole_ThrowsNoAppropriateRoleFoundException()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId).Returns(new List<string> { "User" });

        // Act & Assert
        var exception = Assert.ThrowsAsync<NoAppropriateRoleFoundException>(
            async () => await _handler.Add(_validRequest, _accountId));

        Assert.That(exception.Message, Does.Contain("Owner"));
        Assert.That(exception.Message, Does.Contain("Maintainer"));
    }

    [Test]
    public async Task Add_WithSomeRolesFailingToAdd_ReturnsOnlySuccessfullyAddedRoles()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId).Returns(_ownerRoles);
        
        // Should succeed
        _repository.Add(Arg.Is<AccountRole>(ar => ar.RoleId == _validRequest.RoleIds.First()))
            .Returns(_createdAccountRole);
        
        // Should fail
        _repository.Add(Arg.Is<AccountRole>(ar => ar.RoleId == _validRequest.RoleIds.Last()))
            .Returns((AccountRole?)null);

        // Act
        var result = await _handler.Add(_validRequest, _accountId);

        // Assert
        var accountRoles = result as AccountRole[] ?? result.ToArray();
        Assert.That(accountRoles, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(accountRoles.Count, Is.EqualTo(1));
            Assert.That(accountRoles.First().RoleId, Is.EqualTo(_validRequest.RoleIds.First()));
        });
    }
}