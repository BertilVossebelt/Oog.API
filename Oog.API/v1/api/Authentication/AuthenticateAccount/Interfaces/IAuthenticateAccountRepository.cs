using API.v1.api.Authentication.AuthenticateAccount.Requests;

namespace API.v1.api.Authentication.AuthenticateAccount.Interfaces;

using Oog.Domain;
public interface IAuthenticateAccountRepository
{
    public Task<Account?> Authenticate(AuthenticateAccountRequest request);
}