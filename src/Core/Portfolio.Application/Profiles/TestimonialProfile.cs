using AutoMapper;
using Portfolio.Application.Dtos.Testimonial;
using Portfolio.Domain.Entities;

namespace Portfolio.Application.Profiles;

public class TestimonialProfile : Profile
{
    public TestimonialProfile()
    {
        CreateMap<TestimonialCreateDto, Testimonial>()
            .ForMember(dest => dest.ProfileImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        CreateMap<Testimonial, TestimonialGetDto>();
    }
}