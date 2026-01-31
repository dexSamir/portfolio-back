using AutoMapper;
using Portfolio.Application.Dtos.Technology;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Profiles;

public class TechnologyProfile : Profile
{
    public TechnologyProfile()
    {
        CreateMap<Technology, TechnologyGetDto>();

        CreateMap<TechnologyCreateDto, Technology>()
            .ForMember(dest => dest.CreatedTime,
                opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<TechnologyUpdateDto, Technology>()
            .ForMember(dest => dest.UpdatedTime,
                opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}