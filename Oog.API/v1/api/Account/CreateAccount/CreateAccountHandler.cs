using API.Common.DTOs;
using API.v1.api.Account.CreateAccount.Exceptions;
using API.v1.api.Account.CreateAccount.Interfaces;
using API.v1.api.Account.CreateAccount.Requests;
using AutoMapper;

namespace API.v1.api.Account.CreateAccount;

public class CreateAccountHandler(ICreateAccountRepository repository, IMapper mapper) : ICreateAccountHandler
{
    public async Task<AccountDto?> Create(CreateAccountRequest request)
    {
        // Prepare account.
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var account = new Oog.Domain.Account { Username = request.Username, Password = passwordHash };

        // Check if username already exists.
        var existingAccount = await repository.CheckIfAccountExists(account);
        if (existingAccount != null) throw new UsernameAlreadyExistsException($"Username '{account.Username}' already exists.");

        // Create account.
        var createdAccount = await repository.Create(account);
        
        return mapper.Map<AccountDto>(createdAccount);
    }
}