using API.Common.DTOs;
using AutoMapper;
using Oog.Domain;

namespace API.Common.Profiles;

using Oog.Domain;
public class EnvironmentProfile : Profile
{
    public EnvironmentProfile()
    {
        CreateMap<Environment, EnvironmentDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name));
        
        CreateMap<EnvAccount, EnvironmentDto>()
            .ForMember(
                dest => dest.OwnerId,
                opt => opt.MapFrom(src => src.OwnerId));
    }
}

