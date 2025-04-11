using API.v1.api.Authentication.AuthenticateAccount.Requests;

namespace API.v1.api.Authentication.AuthenticateAccount.Interfaces;

public interface IAuthenticateAccountRepository
{
    public Task<Oog.Domain.Account?> Authenticate(AuthenticateAccountRequest request);
}