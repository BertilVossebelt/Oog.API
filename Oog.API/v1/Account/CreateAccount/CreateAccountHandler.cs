using API.Common.DTOs;
using API.v1.Account.CreateAccount.Exceptions;
using API.v1.Account.CreateAccount.Interfaces;
using API.v1.Account.CreateAccount.Requests;
using AutoMapper;

namespace API.v1.Account.CreateAccount;

using Oog.Domain;
public class CreateAccountHandler(ICreateAccountRepository createAccountRepository, IMapper mapper) : ICreateAccountHandler
{
    public async Task<AccountDto?> Create(CreateAccountRequest request)
    {
        // Prepare account.
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var account = new Account { Username = request.Username, Password = passwordHash };

        // Check if username already exists.
        var existingAccount = await createAccountRepository.CheckIfAccountExists(account);
        if (existingAccount != null) throw new UsernameAlreadyExistsException($"Username '{account.Username}' already exists.");

        // Create account.
        var createdAccount = await createAccountRepository.Create(account);
        
        return mapper.Map<AccountDto>(createdAccount);
    }
}