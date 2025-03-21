using API.Common.DTOs;
using API.v1.Account.CreateAccount.Requests;
using API.v1.Application.CreateApplication.Requests;

namespace API.v1.Application.CreateApplication.Interfaces;

public interface ICreateAppHandler
{
    public Task<AppDto?> Create(CreateAppRequest request);
}