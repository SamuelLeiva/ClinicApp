using AutoMapper;
using Microservices.Services.PatientAPI.Models;
using Microservices.Services.PatientAPI.Models.Dto;

namespace Microservices.Services.PatientAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Patient, PatientDto>().ReverseMap();
                config.CreateMap<Patient, CreatePatientDto>().ReverseMap();
                config.CreateMap<Patient, UpdatePatientDto>().ReverseMap();

                // Configuración para el mapeo de actualización
                config.CreateMap<UpdatePatientDto, Patient>()
                    .ForMember(dest => dest.PatientId, opt => opt.Condition(src => src.PatientId.HasValue))
                    .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                    .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
                    .ForMember(dest => dest.PhoneNumber, opt => opt.Condition(src => src.PhoneNumber != null))
                    .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
                    .ForMember(dest => dest.DateOfBirth, opt => opt.Condition(src => src.DateOfBirth.HasValue));
            });

            return mappingConfig;
        }
    }
}
