using CitasMedicas.Web.Models.Dto;

namespace CitasMedicas.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
