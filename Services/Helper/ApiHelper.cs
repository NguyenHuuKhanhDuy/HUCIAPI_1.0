namespace Services.Helper;

public static class ApiHelper
{
    public async static Task<string> GetRespones(this HttpResponseMessage responseMessage)
    {
        return await responseMessage.Content.ReadAsStringAsync();
    }
}