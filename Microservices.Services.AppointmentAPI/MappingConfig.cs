using AutoMapper;
using Microservices.Services.AppointmentAPI.Models;
using Microservices.Services.AppointmentAPI.Models.Dto;

namespace Microservices.Services.AppointmentAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Appointment, AppointmentDto>().ReverseMap();
                config.CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
