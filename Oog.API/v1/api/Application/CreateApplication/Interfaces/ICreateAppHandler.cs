using API.Common.DTOs;
using API.v1.api.Application.CreateApplication.Requests;
using AutoMapper;

namespace API.v1.api.Application.CreateApplication.Interfaces;

public interface ICreateAppHandler
{
    public Task<AppDto?> Create(CreateAppRequest request, IMapper mapper);
}