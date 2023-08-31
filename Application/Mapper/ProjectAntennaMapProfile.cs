using Application.Models.Projects.ProjectAntennas;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class ProjectAntennaMapProfile : Profile
{
    public ProjectAntennaMapProfile()
    {
        CreateMap<ProjectAntenna, ProjectAntennaDto>()
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
            .ForMember(dest => dest.Azimuth, opt =>
                opt.MapFrom(src => src.Azimuth))
            .ForMember(dest => dest.Height, opt =>
                opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Latitude, opt =>
                opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt =>
                opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Tilt, opt =>
                opt.MapFrom(src => src.Tilt))
            .ForMember(dest => dest.AntennaId, opt =>
                opt.MapFrom(src => src.AntennaId))
            .ForMember(dest => dest.Antenna, opt =>
                opt.MapFrom(src => src.Antenna))
            .ForMember(dest => dest.ProjectId, opt =>
                opt.MapFrom(src => src.ProjectId));
        
        CreateMap<ProjectAntennaDto ,ProjectAntenna>()
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
            .ForMember(dest => dest.Azimuth, opt =>
                opt.MapFrom(src => src.Azimuth))
            .ForMember(dest => dest.Height, opt =>
                opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Latitude, opt =>
                opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt =>
                opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Tilt, opt =>
                opt.MapFrom(src => src.Tilt))
            .ForMember(dest => dest.AntennaId, opt =>
                opt.MapFrom(src => src.AntennaId))
            .ForMember(dest => dest.Antenna, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ProjectId, opt =>
                opt.MapFrom(src => src.ProjectId));
        
        CreateMap<CreateProjectAntennaDto, ProjectAntennaDto>()
            .ForMember(dest => dest.Azimuth, opt =>
                opt.MapFrom(src => src.Azimuth))
            .ForMember(dest => dest.Height, opt =>
                opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Latitude, opt =>
                opt.MapFrom(src => src.Latitude))
            .ForMember(dest => dest.Longitude, opt =>
                opt.MapFrom(src => src.Longitude))
            .ForMember(dest => dest.Tilt, opt =>
                opt.MapFrom(src => src.Tilt))
            .ForMember(dest => dest.AntennaId, opt =>
                opt.MapFrom(src => src.AntennaId))
            .ForMember(dest => dest.ProjectId, opt =>
                opt.MapFrom(src => src.ProjectId));
    }
}