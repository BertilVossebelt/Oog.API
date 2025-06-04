using API.v1.api.Role.CreateRole;
using API.v1.api.Role.CreateRole.Exceptions;
using API.v1.api.Role.CreateRole.Interfaces;
using API.v1.api.Role.CreateRole.Requests;
using NSubstitute;
using NUnit.Framework;
using Oog.Domain;

namespace Tests;

public class CreateRoleTests
{
    private ICreateRoleRepository _repository;
    private CreateRoleHandler _handler;
    private CreateRoleRequest _validRequest;
    private int _accountId;
    private Role _createdRole;
    
    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<ICreateRoleRepository>();
        _handler = new CreateRoleHandler(_repository);
        _accountId = 1;
        
        _validRequest = new CreateRoleRequest(1, "TestRole");
        
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId)
            .Returns(Task.FromResult<IEnumerable<string>>(new List<string> { "Owner" }));
            
        _createdRole = new Role
        {
            Id = 789,
            EnvId = _validRequest.EnvId,
            Name = _validRequest.Name
        };
        
        _repository.Create(Arg.Any<Role>()).Returns(Task.FromResult(_createdRole));
    }
    
    [Test]
    public async Task Create_WithOwnerRole_CreatesAndReturnsRole()
    {
        // Act
        var result = await _handler.Create(_validRequest, _accountId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(_validRequest.Name));
            Assert.That(result.EnvId, Is.EqualTo(_validRequest.EnvId));
            Assert.That(result.Id, Is.EqualTo(_createdRole.Id));
        });
    }

    [Test]
    public async Task Create_WithMaintainerRole_CreatesAndReturnsRole()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId)
            .Returns(Task.FromResult<IEnumerable<string>>(new List<string> { "Maintainer" }));
        
        // Act
        var result = await _handler.Create(_validRequest, _accountId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(_validRequest.Name));
            Assert.That(result.EnvId, Is.EqualTo(_validRequest.EnvId));
        });
    }

    [Test]
    public void Create_WithInsufficientRole_ThrowsNoAppropriateRoleFoundException()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId)
            .Returns(Task.FromResult<IEnumerable<string>>(new List<string> { "User" }));
        
        // Act & Assert
        Assert.ThrowsAsync<NoAppropriateRoleFoundException>(() => _handler.Create(_validRequest, _accountId));
    }
    
    [Test]
    public void Create_WithNoRoles_ThrowsNoAppropriateRoleFoundException()
    {
        // Arrange
        _repository.GetAccountRoles(_accountId, _validRequest.EnvId)
            .Returns(Task.FromResult<IEnumerable<string>>(new List<string>()));
        
        // Act & Assert
        Assert.ThrowsAsync<NoAppropriateRoleFoundException>(() => _handler.Create(_validRequest, _accountId));
    }
    
    [Test]
    public void Create_WhenRoleCreationFails_ThrowsRoleWasNotCreatedException()
    {
        // Arrange
        _repository.Create(Arg.Any<Role>()).Returns(Task.FromResult<Role>(null));
        
        // Act & Assert
        Assert.ThrowsAsync<RoleWasNotCreatedException>(() => _handler.Create(_validRequest, _accountId));
    }
}