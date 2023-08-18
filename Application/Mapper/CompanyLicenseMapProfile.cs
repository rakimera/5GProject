using Application.Models.CompanyLicense;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper;

public class CompanyLicenseMapProfile : Profile
{
    public CompanyLicenseMapProfile()
    {
        CreateMap<CompanyLicense, CompanyLicenseDto>()
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
            .ForMember(dest => dest.Number, opt =>
                opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.DateOfIssue, opt =>
                opt.MapFrom(src => src.DateOfIssue))
            .ReverseMap();

        CreateMap<CompanyLicenseDto, UpdateCompanyLicenseDto>()
            .ForMember(dest => dest.Id, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Number, opt =>
                opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.DateOfIssue, opt =>
                opt.MapFrom(src => src.DateOfIssue))
            .ReverseMap();


        CreateMap<CompanyLicenseDto, CreateCompanyLicenseDto>()
            .ForMember(dest => dest.Number, opt =>
                opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.DateOfIssue, opt =>
                opt.MapFrom(src => src.DateOfIssue))
            .ReverseMap();
    }
}