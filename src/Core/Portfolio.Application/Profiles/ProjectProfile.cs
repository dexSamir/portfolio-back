using AutoMapper;
using Portfolio.Application.Dtos.Project;
using Portfolio.Application.Dtos.Technology;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Profiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectGetDto>()
            .ForMember(dest => dest.Technologies,
                opt => opt.MapFrom(src =>
                    src.ProjectTechnologies.Select(pt => pt.Technology)));
        
        CreateMap<Technology, TechnologyNestedGetDto>();

        CreateMap<ProjectCreateDto, Project>()
            .ForMember(
                dest => dest.ProjectTechnologies,
                opt => opt.MapFrom(src =>
                    src.TechnologyIds.Select(id => new ProjectTechnology
                    {
                        TechnologyId = id
                    })
                )
            )
            .ForMember(dest => dest.CreatedTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedTime, opt => opt.Ignore())
            .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

        CreateMap<ProjectUpdateDto, Project>()

            .ForMember(dest => dest.ProjectTechnologies, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedTime, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null)
            ); 
    }
}