using Application.Models.Projects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class ProjectMapProfile : Profile
{
    public ProjectMapProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ContrAgent, opt =>
                opt.MapFrom(src => src.ContrAgent))
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorId))
            .ForMember(dest => dest.Executor, opt =>
                opt.MapFrom(src => src.Executor))
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusId))
            .ForMember(dest => dest.ExecutiveCompany, opt =>
                opt.MapFrom(src => src.ExecutiveCompany))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src => src.DistrictName))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.TownName))
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.PurposeRto, opt =>
                opt.MapFrom(src => src.PurposeRto))
            .ForMember(dest => dest.PlaceOfInstall, opt =>
                opt.MapFrom(src => src.PlaceOfInstall))
            .ForMember(dest => dest.MaxHeightAdjoinBuild, opt =>
                opt.MapFrom(src => src.MaxHeightAdjoinBuild))
            .ForMember(dest => dest.PurposeBuild, opt =>
                opt.MapFrom(src => src.PurposeBuild))
            .ForMember(dest => dest.TypeOfTopCover, opt =>
                opt.MapFrom(src => src.TypeOfTopCover))
            .ForMember(dest => dest.TypeORoof, opt =>
                opt.MapFrom(src => src.TypeORoof))
            .ForMember(dest => dest.PlaceOfCommunicationCloset, opt =>
                opt.MapFrom(src => src.PlaceOfCommunicationCloset))
            .ForMember(dest => dest.HasTechnicalLevel, opt =>
                opt.MapFrom(src => src.HasTechnicalLevel))
            .ForMember(dest => dest.HasOtherRto, opt =>
                opt.MapFrom(src => src.HasOtherRto));
        
        
        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.ContrAgent, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorId))
            .ForMember(dest => dest.Executor, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusId))
            .ForMember(dest => dest.ProjectStatus, opt =>
                opt.Ignore())
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src=> src.Address))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src=> src.TownName))
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src=> src.DistrictName))
            .ForMember(dest => dest.ProjectAntennae, opt =>
                opt.Ignore())
            .ForMember(dest => dest.TotalFluxDensity, opt =>
                opt.Ignore())
            .ForMember(dest => dest.ExecutiveCompany, opt =>
                opt.Ignore())
            .ForMember(dest => dest.PurposeRto, opt =>
                opt.MapFrom(src => src.PurposeRto))
            .ForMember(dest => dest.PlaceOfInstall, opt =>
                opt.MapFrom(src => src.PlaceOfInstall))
            .ForMember(dest => dest.MaxHeightAdjoinBuild, opt =>
                opt.MapFrom(src => src.MaxHeightAdjoinBuild))
            .ForMember(dest => dest.PurposeBuild, opt =>
                opt.MapFrom(src => src.PurposeBuild))
            .ForMember(dest => dest.TypeOfTopCover, opt =>
                opt.MapFrom(src => src.TypeOfTopCover))
            .ForMember(dest => dest.TypeORoof, opt =>
                opt.MapFrom(src => src.TypeORoof))
            .ForMember(dest => dest.PlaceOfCommunicationCloset, opt =>
                opt.MapFrom(src => src.PlaceOfCommunicationCloset))
            .ForMember(dest => dest.HasTechnicalLevel, opt =>
                opt.MapFrom(src => src.HasTechnicalLevel))
            .ForMember(dest => dest.HasOtherRto, opt =>
                opt.MapFrom(src => src.HasOtherRto));

        CreateMap<UpdateProjectDto, Project>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.ExecutorId, opt =>
                opt.MapFrom(src => src.ExecutorId))
            .ForMember(dest => dest.ProjectStatusId, opt =>
                opt.MapFrom(src => src.ProjectStatusId))
            .ForMember(dest => dest.ExecutiveCompanyId, opt =>
                opt.MapFrom(src => src.ExecutiveCompanyId))
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src=>src.Address))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src=>src.TownName))
            .ForMember(dest => dest.DistrictName, opt =>
                opt.MapFrom(src=>src.DistrictName))
            .ForMember(dest => dest.PurposeRto, opt =>
                opt.MapFrom(src => src.PurposeRto))
            .ForMember(dest => dest.PlaceOfInstall, opt =>
                opt.MapFrom(src => src.PlaceOfInstall))
            .ForMember(dest => dest.MaxHeightAdjoinBuild, opt =>
                opt.MapFrom(src => src.MaxHeightAdjoinBuild))
            .ForMember(dest => dest.PurposeBuild, opt =>
                opt.MapFrom(src => src.PurposeBuild))
            .ForMember(dest => dest.TypeOfTopCover, opt =>
                opt.MapFrom(src => src.TypeOfTopCover))
            .ForMember(dest => dest.TypeORoof, opt =>
                opt.MapFrom(src => src.TypeORoof))
            .ForMember(dest => dest.PlaceOfCommunicationCloset, opt =>
                opt.MapFrom(src => src.PlaceOfCommunicationCloset))
            .ForMember(dest => dest.HasTechnicalLevel, opt =>
                opt.MapFrom(src => src.HasTechnicalLevel))
            .ForMember(dest => dest.HasOtherRto, opt =>
                opt.MapFrom(src => src.HasOtherRto));
        
        CreateMap<CreateProjectDto, ProjectDto>()
            .ForMember(dest => dest.ContrAgentId, opt =>
                opt.MapFrom(src => src.ContrAgentId))
            .ForMember(dest => dest.TownName, opt =>
                opt.MapFrom(src => src.TownName))
            .ForMember(dest => dest.Address, opt =>
                opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.ProjectNumber, opt =>
                opt.MapFrom(src => src.ProjectNumber))
            .ForMember(dest => dest.PurposeRto, opt =>
                opt.MapFrom(src => src.PurposeRto))
            .ForMember(dest => dest.PlaceOfInstall, opt =>
                opt.MapFrom(src => src.PlaceOfInstall))
            .ForMember(dest => dest.MaxHeightAdjoinBuild, opt =>
                opt.MapFrom(src => src.MaxHeightAdjoinBuild))
            .ForMember(dest => dest.PurposeBuild, opt =>
                opt.MapFrom(src => src.PurposeBuild))
            .ForMember(dest => dest.TypeOfTopCover, opt =>
                opt.MapFrom(src => src.TypeOfTopCover))
            .ForMember(dest => dest.TypeORoof, opt =>
                opt.MapFrom(src => src.TypeORoof))
            .ForMember(dest => dest.PlaceOfCommunicationCloset, opt =>
                opt.MapFrom(src => src.PlaceOfCommunicationCloset))
            .ForMember(dest => dest.HasTechnicalLevel, opt =>
                opt.MapFrom(src => src.HasTechnicalLevel))
            .ForMember(dest => dest.HasOtherRto, opt =>
                opt.MapFrom(src => src.HasOtherRto));

    }
}