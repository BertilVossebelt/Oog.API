using API.v1.api.Authentication.AuthenticateAccount.Requests;

namespace API.v1.api.Authentication.AuthenticateAccount.Interfaces;

public interface IAuthenticateAccountHandler
{
    public Task<string> Authenticate(AuthenticateAccountRequest request, string jwtSecret);

}