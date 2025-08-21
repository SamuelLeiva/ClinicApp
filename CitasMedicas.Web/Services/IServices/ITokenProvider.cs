namespace CitasMedicas.Web.Services.IServices
{
    public interface ITokenProvider
    {
        void SetTokenAsync(string token);
        string? GetTokenAsync();
        void ClearTokenAsync();
    }
}
