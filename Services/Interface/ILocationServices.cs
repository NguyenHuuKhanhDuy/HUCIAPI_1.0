using ApplicationCore.ModelsDto.Location;

namespace Services.Interface
{
    public interface ILocationServices
    {
        Task<List<LocationDto>> GetLocationByIdParentAsync(int parentId);
    }
}
