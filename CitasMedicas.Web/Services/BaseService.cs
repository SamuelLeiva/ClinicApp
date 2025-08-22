using CitasMedicas.Web.Models.Dto;
using CitasMedicas.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Text;
using static CitasMedicas.Web.Utility.SD;

namespace CitasMedicas.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory clientFactory, ITokenProvider tokenProvider)
        {
            _clientFactory = clientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _clientFactory.CreateClient("MicroservicesAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                if (requestDto != null)
                {
                    if (requestDto?.ContentType == Utility.SD.ContentType.MultiPartFormData)
                    {
                        message.Headers.Add("Accept", "*/*");
                        Console.WriteLine("Using Content-Type: multipart/form-data");
                    }
                    else
                    {
                        message.Headers.Add("Accept", "application/json");
                        Console.WriteLine("Using Content-Type: application/json");
                    }
                }

                //TOKEN
                if (withBearer)
                {
                    var token = _tokenProvider.GetTokenAsync();
                    if (!string.IsNullOrEmpty(token))
                    {
                        message.Headers.Add("Authorization", $"Bearer {token}");
                        Console.WriteLine("Using Bearer Token: " + token);
                    }
                    else
                    {
                        Console.WriteLine("No Bearer Token found.");
                    }
                }

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto?.ContentType == Utility.SD.ContentType.MultiPartFormData)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if (value is FormFile)
                        {
                            var file = value as FormFile;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(value?.ToString() ?? string.Empty), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (requestDto?.Data != null)
                    {
                        var jsonData = JsonConvert.SerializeObject(requestDto.Data);
                        Console.WriteLine("Request JSON Data: " + jsonData);
                        message.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    }
                }

                message.Method = requestDto?.ApiType switch
                {
                    ApiType.GET => HttpMethod.Get,
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };


                Console.WriteLine($"Sending request to: {message.RequestUri} with method: {message.Method}");

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    var errorContent = await apiResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error response from API: {errorContent} with status code: {apiResponse.StatusCode}");
                }

                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "Bad Request" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        ResponseDto? apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }

            }
            catch (Exception ex)
            {
                return new ResponseDto()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
