using AutoMapper;
using Microservices.Services.DoctorAPI.Models;
using Microservices.Services.DoctorAPI.Models.Dto;

namespace Microservices.Services.DoctorAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Doctor, DoctorDto>().ReverseMap();
                config.CreateMap<Doctor, CreateDoctorDto>().ReverseMap();
                config.CreateMap<Doctor, UpdateDoctorDto>().ReverseMap();

                config.CreateMap<UpdateDoctorDto, Doctor>()
                    .ForMember(dest => dest.DoctorId, opt => opt.Condition(src => src.DoctorId.HasValue))
                    .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                    .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.PhoneNumber != null))
                    .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
                    .ForMember(dest => dest.SpecialtyId, opt => opt.Condition(src => src.SpecialtyId != null));

                config.CreateMap<Specialty, SpecialtyDto>().ReverseMap();
                config.CreateMap<Specialty, CreateSpecialtyDto>().ReverseMap();
                config.CreateMap<Specialty, UpdateSpecialtyDto>().ReverseMap();

                config.CreateMap<UpdateSpecialtyDto, Specialty>()
                    .ForMember(dest => dest.SpecialtyId, opt => opt.Condition(src => src.SpecialtyId.HasValue))
                    .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null));
            });
            return mappingConfig;
        }
    }
}
