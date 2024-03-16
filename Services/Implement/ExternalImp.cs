using System.Net.Http.Headers;
using System.Text;
using ApplicationCore.ModelsDto.External;
using Microsoft.Extensions.Configuration;
using Services.Helper;
using Services.Interface;

namespace Services.Implement;

public class ExternalImp : IExternal
{
    private IConfiguration _configuration;

    public ExternalImp(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<object> GetCustomer(string phoneNumber)
    {
        var response = new CustomerInfor();

        if (string.IsNullOrEmpty(phoneNumber))
        {
            return response;
        }

        var token = await GetToken();
        var endPoint = $"https://public.kiotapi.com/customers?contactNumber=${phoneNumber}";

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
            client.DefaultRequestHeaders.Add("Retailer", "tigris");
            var responseFromKiot = await client.GetAsync(endPoint);
            var content = responseFromKiot.GetRespones().Result;
            return content;
        }
    }
    
    private async Task<ConnectToken> GetToken()
    {
        var boxingSaigonSection = _configuration.GetSection("BoxingSaigon");
        var clientId = boxingSaigonSection["ClientId"];
        var clientSecret = boxingSaigonSection["ClientSecret"];
        var endPoint = "https://id.kiotviet.vn/connect/token";
        using (var client = new HttpClient())
        {
            var body = new StringContent(
                $"scopes=PublicApi.Access&grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}",
                Encoding.UTF8, "application/x-www-form-urlencoded");
            
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var responseFromKiot = await client.PostAsync(endPoint, body);
            var response = Newtonsoft.Json.JsonConvert.DeserializeObject<ConnectToken>(responseFromKiot.GetRespones().Result);
            return response;
        }
    }

    
}