namespace Microservices.Services.DoctorAPI
{
    public class MappingConfig
    {
        public static AutoMapper.MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new AutoMapper.MapperConfiguration(config =>
            {
                config.CreateMap<Models.Doctor, Models.Dto.DoctorDto>().ReverseMap();
                config.CreateMap<Models.Specialty, Models.Dto.SpecialtyDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
