using API.Common.DTOs;
using AutoMapper;
using Oog.Domain;

namespace API.Common.Profiles;

using Oog.Domain;
public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<App, AppDto>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(
                dest => dest.EnvId,
                opt => opt.MapFrom(src => src.EnvId))
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(
                dest => dest.Passkey,
                opt => opt.MapFrom(src => src.Passkey));
    }
}

