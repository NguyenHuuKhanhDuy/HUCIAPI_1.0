using ApplicationCore.ModelsDto.Location;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interface;

namespace Services.Implement
{
    public class LocationImp : BaseServices, ILocationServices
    {
        private readonly HucidbContext _dbContext;

        public LocationImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LocationDto>> GetLocationByIdParentAsync(int parentId)
        {
            var locations = await _dbContext.Locations.AsNoTracking().Where(x => x.ParentId == parentId).ToListAsync();
            var locationDtos = new List<LocationDto>();

            foreach (var location in locations)
            {
                locationDtos.Add(new LocationDto
                {
                    Id = location.Id,
                    ParentId = location.ParentId.HasValue ? location.ParentId.Value : 0,
                    Name = location.Name
                });
            }

            return locationDtos;
        }
    }
}
