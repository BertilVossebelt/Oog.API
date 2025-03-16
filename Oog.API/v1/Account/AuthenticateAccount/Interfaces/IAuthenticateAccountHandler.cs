using API.v1.Account.AuthenticateAccount.Requests;

namespace API.v1.Account.AuthenticateAccount.Interfaces;

public interface IAuthenticateAccountHandler
{
    public Task<string> Authenticate(AuthenticateAccountRequest request, string jwtSecret);

}