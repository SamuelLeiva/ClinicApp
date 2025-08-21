using CitasMedicas.Web.Services.IServices;
using CitasMedicas.Web.Utility;

namespace CitasMedicas.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearTokenAsync()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TokenCookie);
        }

        public string? GetTokenAsync()
        {
            var context = _contextAccessor.HttpContext;
            if (context == null)
            {
                var cookie = context?.Request.Cookies[SD.TokenCookie];
                Console.WriteLine($"Token from cookie: {cookie}");
                return cookie;
            }
            Console.WriteLine("HttpContext is null, cannot retrieve token from cookie.");
            return null;
        }

        public void SetTokenAsync(string token)
        {
            _contextAccessor.HttpContext?.Response.Cookies.Append(SD.TokenCookie, token);
        }
    }
}
