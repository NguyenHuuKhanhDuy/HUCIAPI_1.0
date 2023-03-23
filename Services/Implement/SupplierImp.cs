using Infrastructure.Models;
using Services.Interface;

namespace Services.Implement
{
    public class SupplierImp : BaseServices, ISupplierServices
    {
        private readonly HucidbContext _dbContext;
        public SupplierImp(HucidbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
