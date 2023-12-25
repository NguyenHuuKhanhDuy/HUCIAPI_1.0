using ApplicationCore.Exceptions;
using ApplicationCore.ModelsDto.IP;
using ApplicationCore.ViewModels.IP;
using Common.Constants;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Services.Helper;
using Services.Interface;

namespace Services.Implement
{
    public class IPImp : BaseServices, IIPServices
    {
        private readonly HucidbContext _dbContext;
        public IPImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<IPDto> CreateIPAsync(IPVM vm)
        {
            var ip = new Ip
            {
                Id = Guid.NewGuid(),
                Ipv4 = vm.Ipv4,
                CreateDate = GetDateTimeNow(),
                IsDeleted = BaseConstants.IsDeletedDefault,
                Notes = !string.IsNullOrEmpty(vm.Notes) ? vm.Notes : string.Empty
            };

            await _dbContext.AddAsync(ip);
            await _dbContext.SaveChangesAsync();

            var dto = MapFIPTIPDto(ip);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IPId"></param>
        /// <returns></returns>
        public async Task DeleteIPAsync(Guid IPId)
        {
            var ip = await FindIpAsync(IPId);
            ip.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<IPDto>> GetAllIPsAsync()
        {
            var ips = await _dbContext.Ips.Where(x => !x.IsDeleted).ToListAsync();
            var dtos = new List<IPDto>();
            dtos = DataMapper.MapList<Ip, IPDto>(ips);

            return dtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IPDto> UpdateIPAsync(IPUpdateVM vm)
        {
            var ip = await FindIpAsync(vm.Id);
            ip.Ipv4 = vm.Ipv4;
            ip.Notes = !string.IsNullOrEmpty(vm.Notes) ? vm.Notes : string.Empty;
            await _dbContext.SaveChangesAsync();

            var dto = MapFIPTIPDto(ip);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IPId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task<Ip> FindIpAsync(Guid IPId)
        {
            var ip = await _dbContext.Ips.FirstOrDefaultAsync(x => x.Id == IPId && !x.IsDeleted);

            if (ip == null)
            {
                throw new BusinessException(IPConstants.IPNotExist);
            }

            return ip;
        }
    }
}
