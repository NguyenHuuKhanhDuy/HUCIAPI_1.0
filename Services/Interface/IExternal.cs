using ApplicationCore.ModelsDto.External;

namespace Services.Interface;

public interface IExternal
{
    Task<object> GetCustomer(string phoneNumber);
}