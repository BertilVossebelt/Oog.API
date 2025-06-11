using API.Common.DTOs;
using API.v1.api.Account.CreateAccount.Requests;

namespace API.v1.api.Account.CreateAccount.Interfaces;

public interface ICreateAccountHandler
{
    public Task<AccountDto?> Create(CreateAccountRequest request);
}