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
            });

            return mappingConfig;
        }
    }
}
