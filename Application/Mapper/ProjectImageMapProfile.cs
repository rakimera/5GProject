using Application.Models.Projects.ProjectImages;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class ProjectImageMapProfile : Profile
{
    public ProjectImageMapProfile()
    {
        CreateMap<ProjectImage, ProjectImageDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Created, opt =>
                opt.MapFrom(src => src.Created))
            .ForMember(dest => dest.LastModified, opt =>
                opt.MapFrom(src => src.LastModified))
            .ForMember(dest => dest.LastModifiedBy, opt =>
                opt.MapFrom(src => src.LastModifiedBy))
            .ForMember(dest => dest.CreatedBy, opt =>
                opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.Route, opt =>
                opt.MapFrom(src => src.Route))
            .ForMember(dest => dest.ProjectId, opt =>
                opt.MapFrom(src => src.ProjectId))
            .ReverseMap();
    }
}