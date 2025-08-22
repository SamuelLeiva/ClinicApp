using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using CitasMedicas.Web.Utility;

namespace CitasMedicas.Web.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IBaseService _baseService;
        public SpecialtyService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public Task<ResponseDto> CreateSpecialtyAsync(CreateSpecialtyDto specialtyDto)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = specialtyDto,
                Url = SD.SpecialtyAPIBase + "/api/SpecialtyAPI"
            });
        }

        public Task<ResponseDto> GetSpecialtiesAsync()
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.SpecialtyAPIBase + "/api/SpecialtyAPI"
            });
        }

        public Task<ResponseDto> GetSpecialtyByIdAsync(int specialtyId)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.SpecialtyAPIBase + "/api/SpecialtyAPI/" + specialtyId
            });
        }

        public Task<ResponseDto> UpdateSpecialtyAsync(UpdateSpecialtyDto specialtyDto)
        {
            return _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = specialtyDto,
                Url = SD.SpecialtyAPIBase + "/api/SpecialtyAPI/" + specialtyDto.SpecialtyId
            });
        }
    }
}
