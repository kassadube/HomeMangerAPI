using HomeManger.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeManger.Data
{
    public class CategoryRepository: BaseRepository, ICategoryRepository
    {
        private const string SQL_GET_GATEGORIES = "select * from CategoryTbl";
        private const string SQL_GET_SUB_GATEGORIES = "select * from SubCategoryTbl";
        public CategoryRepository(IConfiguration configuration, ILogger<CategoryRepository> logger) : base(configuration, logger)
        {

        }

        public async Task<BaseResultInfo> GetCategories(BaseREQ req, bool onlyWithVehicles = false)
        {
            BaseResultInfo res = new BaseResultInfo();
            try
            {
                res.ResultObject = base.GetTable<CategoryInfo>(SQL_GET_GATEGORIES, new { AccountId = req.AccountId });
            }
            catch (Exception ex)
            {
                base._logger.LogError(ex, "Data {data}", new { sp = SQL_GET_GATEGORIES, m = "GetCategories", req = req, onlyWithVehicles });
                res.Error = new BaseErrorInfo() { Message = ex.Message };
            }
            return res;
        }

        public async Task<BaseResultInfo> GetSubCategories(BaseREQ req, bool onlyWithVehicles = false)
        {
            BaseResultInfo res = new BaseResultInfo();
            try
            {
                res.ResultObject = base.GetTable<SubCategoryInfo>(SQL_GET_SUB_GATEGORIES, new { AccountId = req.AccountId });
            }
            catch (Exception ex)
            {
                base._logger.LogError(ex, "Data {data}", new { sp = SQL_GET_SUB_GATEGORIES, m = "GetSubCategories", req = req, onlyWithVehicles });
                res.Error = new BaseErrorInfo() { Message = ex.Message };
            }
            return res;
        }

    }
}
