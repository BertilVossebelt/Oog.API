namespace API.v1.api.Account.CreateAccount.Interfaces;

public interface ICreateAccountRepository
{
    public Task<Oog.Domain.Account?> Create(Oog.Domain.Account request);

    public Task<Oog.Domain.Account?> CheckIfAccountExists(Oog.Domain.Account account);
}