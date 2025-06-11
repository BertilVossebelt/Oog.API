using API.v1.api.Authentication.AuthenticateAccount.Exceptions;
using API.v1.api.Authentication.AuthenticateApplication;
using API.v1.api.Authentication.AuthenticateApplication.Interfaces;
using API.v1.api.Authentication.AuthenticateApplication.Requests;
using NSubstitute;
using Oog.Domain;
using System.IdentityModel.Tokens.Jwt;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class AuthenticateAppTests
{
    private AuthenticateAppHandler _handler;
    private IAuthenticateAppRepository _repository;
    private AuthenticateAppRequest _validRequest;
    private App _existingApp;
    private string _jwtSecret;

    [SetUp]
    public void Setup()
    {
        // Arrange
        _repository = Substitute.For<IAuthenticateAppRepository>();
        _handler = new AuthenticateAppHandler(_repository);

        _validRequest = new AuthenticateAppRequest(
            name: "test-app",
            passKey: "app-secret-passkey-123"
        );

        _existingApp = new App
        {
            Id = 5,
            Name = _validRequest.Name,
            Passkey = BCrypt.Net.BCrypt.HashPassword(_validRequest.PassKey),
            EnvId = 10
        };

        _jwtSecret = "this_is_a_very_long_secret_key_for_testing_jwt_generation_security";
    }

    [Test]
    public async Task Authenticate_WithValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns(_existingApp);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
        
        // Verify token can be parsed as JWT
        var handler = new JwtSecurityTokenHandler();
        Assert.That(handler.CanReadToken(token), Is.True);
    }

    [Test]
    public void Authenticate_WithInvalidAppName_ThrowsInvalidCredentialsException()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns((App?)null);

        // Act & Assert
        Assert.ThrowsAsync<InvalidCredentialsException>(
            async () => await _handler.Authenticate(_validRequest, _jwtSecret));
    }

    [Test]
    public void Authenticate_WithInvalidPasskey_ThrowsInvalidCredentialsException()
    {
        // Arrange
        _existingApp.Passkey = BCrypt.Net.BCrypt.HashPassword("DifferentPassword123");
        _repository.Authenticate(_validRequest).Returns(_existingApp);

        // Act & Assert
        Assert.ThrowsAsync<InvalidCredentialsException>(
            async () => await _handler.Authenticate(_validRequest, _jwtSecret));
    }

    [Test]
    public async Task Authenticate_TokenContainsCorrectClaims()
    {
        // Arrange
        _repository.Authenticate(_validRequest).Returns(_existingApp);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Multiple(() =>
        {
            // Check subject claim (app id)
            var subClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            Assert.That(subClaim, Is.Not.Null);
            Assert.That(subClaim?.Value, Is.EqualTo(_existingApp.Id.ToString()));

            // Check name claim (app name)
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name);
            Assert.That(nameClaim, Is.Not.Null);
            Assert.That(nameClaim?.Value, Is.EqualTo(_existingApp.Name));

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
        _repository.Authenticate(_validRequest).Returns(_existingApp);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var expectedExpiry = DateTime.UtcNow.AddDays(1);
        var tokenExpiry = jwtToken.ValidTo;
        var timeDifference = (tokenExpiry - expectedExpiry).TotalMinutes;
        
        Assert.That(Math.Abs(timeDifference), Is.LessThan(1));
    }


    [Test]
    public async Task Authenticate_VerifyPasskeyComparison()
    {
        // Arrange
        var freshlyHashedPasskey = BCrypt.Net.BCrypt.HashPassword(_validRequest.PassKey);
        var appWithFreshHash = new App
        {
            Id = _existingApp.Id,
            Name = _existingApp.Name,
            Passkey = freshlyHashedPasskey,
            EnvId = _existingApp.EnvId
        };
        
        _repository.Authenticate(_validRequest).Returns(appWithFreshHash);

        // Act
        var token = await _handler.Authenticate(_validRequest, _jwtSecret);

        // Assert
        Assert.That(token, Is.Not.Null);
        Assert.That(token, Is.Not.Empty);
    }
}