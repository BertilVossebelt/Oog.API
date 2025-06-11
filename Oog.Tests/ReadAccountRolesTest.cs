using API.v1.api.Account.ReadAccountRoles;
using API.v1.api.Account.ReadAccountRoles.Exceptions;
using API.v1.api.Account.ReadAccountRoles.Interfaces;
using API.v1.api.Account.ReadAccountRoles.Request;
using NSubstitute;
using Oog.Domain;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ReadAccountRolesTest
{
    private ReadAccountRolesHandler _handler;
    private IReadAccountRolesRepository _repository;
    private ReadAccountRolesRequest _validRequest;
    private int _accountId;
    private int _envId;
    private List<string> _ownerRoles;
    private List<AccountRole> _expectedRoles;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IReadAccountRolesRepository>();
        _handler = new ReadAccountRolesHandler(_repository);

        _accountId = 1;
        _envId = 5;
        _validRequest = new ReadAccountRolesRequest(_accountId, _envId);

        _ownerRoles = ["Owner"];
        _expectedRoles = [
            new AccountRole { AccountId = _accountId, RoleId = 1 },
            new AccountRole { AccountId = _accountId, RoleId = 2 }
        ];
    }

    [Test]
    public async Task Get_WithValidRequest_ReturnsExpectedRoles()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId).Returns(_ownerRoles);
        _repository.Get(_validRequest).Returns(_expectedRoles);

        // Act
        var result = await _handler.Get(_validRequest, _accountId);

        // Assert
        Assert.That(result, Is.EqualTo(_expectedRoles));
    }

    [Test]
    public void Get_WithoutAppropriateRole_ThrowsNoAppropriateRoleFoundException()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId).Returns(new List<string> { "User" });

        // Act & Assert
        var exception = Assert.ThrowsAsync<NoAppropriateRoleFoundException>(
            async () => await _handler.Get(_validRequest, _accountId));

        Assert.That(exception.Message, Does.Contain("Owner"));
        Assert.That(exception.Message, Does.Contain("Maintainer"));
    }
}