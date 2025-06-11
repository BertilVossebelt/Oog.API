using API.v1.api.Authentication.AuthenticateAccount;
using API.v1.api.Authentication.AuthenticateAccount.Exceptions;
using API.v1.api.Authentication.AuthenticateAccount.Interfaces;
using API.v1.api.Authentication.AuthenticateAccount.Requests;
using NSubstitute;
using Oog.Domain;
using System.IdentityModel.Tokens.Jwt;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class AuthenticateAccountTests
{
    private AuthenticateAccountHandler _handler;
    private IAuthenticateAccountRepository _repository;
    private AuthenticateAccountRequest _validRequest;
    private Account _existingAccount;
    private string _jwtSecret;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<IAuthenticateAccountRepository>();
        _handler = new AuthenticateAccountHandler(_repository);

        _validRequest = new AuthenticateAccountRequest(
            username: "testuser@example.com",
            password: "ValidP@ssw0rd123"
        );

        _existingAccount = new Account
        {
            Id = 1,
            Username = _validRequest.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(_validRequest.Password)
        };

        _jwtSecret = "this_is_a_very_long_secret_key_for_testing_jwt_generation_security";
    }

    [Test]
    public async Task Authenticate_WithValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns(_existingAccount);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
        
        // Verify token can be parsed as jwt
        var handler = new JwtSecurityTokenHandler();
        Assert.That(handler.CanReadToken(token), Is.True);
    }

    [Test]
    public void Authenticate_WithInvalidUsername_ThrowsInvalidCredentialsException()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns((Account?)null);

        // Act & Assert
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await _handler.Authenticate(_validRequest, _jwtSecret));
    }

    [Test]
    public void Authenticate_WithInvalidPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        _existingAccount.Password = BCrypt.Net.BCrypt.HashPassword("DifferentPassword123");
        _repository.Authenticate(_validRequest).Returns(_existingAccount);

        // Act & Assert
        Assert.ThrowsAsync<InvalidCredentialsException>(async () => await _handler.Authenticate(_validRequest, _jwtSecret));
    }

    [Test]
    public async Task Authenticate_TokenContainsCorrectClaims()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns(_existingAccount);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Multiple(() =>
        {
            // Check subject claim (user id)
            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            Assert.That(subClaim, Is.Not.Null);
            Assert.That(subClaim?.Value, Is.EqualTo(_existingAccount.Id.ToString()));

            // Check name claim (username)
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            Assert.That(nameClaim, Is.Not.Null);
            Assert.That(nameClaim?.Value, Is.EqualTo(_existingAccount.Username));

            // Check jwt id claim (unique identifier)
            var jtiClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            Assert.That(jtiClaim, Is.Not.Null);
            Assert.That(Guid.TryParse(jtiClaim?.Value, out _), Is.True);
        });
    }

    [Test]
    public async Task Authenticate_TokenHasCorrectExpiryDate()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns(_existingAccount);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var expectedExpiry = DateTime.UtcNow.AddDays(5);
        var tokenExpiry = jwtToken.ValidTo;
        var timeDifference = (tokenExpiry - expectedExpiry).TotalMinutes;

        Assert.That(Math.Abs(timeDifference), Is.LessThan(1));
    }
}