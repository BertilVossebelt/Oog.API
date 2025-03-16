using API.v1.Account.AuthenticateAccount.Requests;

namespace API.v1.Account.AuthenticateAccount.Interfaces;

public interface IAuthenticateAccountRepository
{
    public Task<Oog.Domain.Account?> Authenticate(AuthenticateAccountRequest request);
}