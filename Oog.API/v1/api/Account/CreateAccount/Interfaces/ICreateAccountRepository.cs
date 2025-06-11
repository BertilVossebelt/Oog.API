namespace API.v1.api.Account.CreateAccount.Interfaces;

using Oog.Domain;
public interface ICreateAccountRepository
{
    public Task<Account?> Create(Account request);

    public Task<Account?> CheckIfAccountExists(Account account);
}