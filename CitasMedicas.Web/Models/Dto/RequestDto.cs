using System.Security.AccessControl;
using static CitasMedicas.Web.Utility.SD;

namespace CitasMedicas.Web.Models.Dto
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
