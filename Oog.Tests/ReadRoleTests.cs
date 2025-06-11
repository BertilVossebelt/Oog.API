using API.v1.api.Role.ReadRole;
using API.v1.api.Role.ReadRole.Exceptions;
using API.v1.api.Role.ReadRole.Interface;
using NSubstitute;
using NUnit.Framework;
using Oog.Domain;

namespace Tests;

public class ReadRoleTests
{
    private IReadRoleRepository _repository;
    private ReadRoleHandler _handler;
    private int _envId;
    private int _accountId;
    private IEnumerable<Role> _expectedRoles;
    
    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IReadRoleRepository>();
        _handler = new ReadRoleHandler(_repository);
        _envId = 456;
        _accountId = 123;
        
        _expectedRoles =
        [
            new Role { Id = 1, Name = "Admin", EnvId = _envId },
            new Role { Id = 2, Name = "User", EnvId = _envId }
        ];
        
        _repository.GetAccountRoles(_accountId, _envId)
            .Returns(Task.FromResult<IEnumerable<string>>(["Owner"]));
        _repository.Get(_envId).Returns(Task.FromResult(_expectedRoles));
    }
    
    [Test]
    public async Task Get_WithOwnerRole_ReturnsRoles()
    {
        // Act
        var result = await _handler.Get(_envId, _accountId);
        
        // Assert
        var roles = result as Role[] ?? result.ToArray();
        Assert.That(roles, Is.Not.Null);
        Assert.That(roles, Is.EquivalentTo(_expectedRoles));
    }
    
    [Test]
    public async Task Get_WithMaintainerRole_ReturnsRoles()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _envId)
            .Returns(Task.FromResult<IEnumerable<string>>(["Maintainer"]));
        
        // Act
        var result = await _handler.Get(_envId, _accountId);
        
        // Assert
        var roles = result as Role[] ?? result.ToArray();
        Assert.That(roles, Is.Not.Null);
        Assert.That(roles, Is.EquivalentTo(_expectedRoles));
    }
    
    [Test]
    public void Get_WithInsufficientRole_ThrowsNoAppropriateRoleFoundException()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _envId)
            .Returns(Task.FromResult<IEnumerable<string>>(["User"]));
        
        // Act & Assert
        Assert.ThrowsAsync<NoAppropriateRoleFoundException>(() => _handler.Get(_envId, _accountId));
    }
    
    [Test]
    public void Get_WithNoRoles_ThrowsNoAppropriateRoleFoundException()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _envId)
            .Returns(Task.FromResult<IEnumerable<string>>([]));
        
        // Act & Assert
        Assert.ThrowsAsync<NoAppropriateRoleFoundException>(() => _handler.Get(_envId, _accountId));
    }
    
    [Test]
    public async Task Get_WhenRepositoryReturnsEmptyList_ReturnsEmptyList()
    {
        // Arrange
        _repository.Get(_envId)
            .Returns(Task.FromResult<IEnumerable<Role>>([]));
        
        // Act
        var result = await _handler.Get(_envId, _accountId);
        
        // Assert
        Assert.That(result, Is.Empty);
    }
    
    [TestCase("Owner")]
    [TestCase("Maintainer")]
    public async Task Get_WithSufficientRoleInMixedRoleList_ReturnsRoles(string sufficientRole)
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _envId)
            .Returns(Task.FromResult<IEnumerable<string>>(["User", sufficientRole, "Viewer"]));
        
        // Act
        var result = await _handler.Get(_envId, _accountId);
        
        // Assert
        var roles = result as Role[] ?? result.ToArray();
        Assert.That(roles, Is.Not.Null);
        Assert.That(roles, Is.EquivalentTo(_expectedRoles));
    }
}