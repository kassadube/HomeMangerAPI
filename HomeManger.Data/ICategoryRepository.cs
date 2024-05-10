using HomeManger.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeManger.Data
{
    public interface ICategoryRepository
    {

        Task<BaseResultInfo> GetCategories(BaseREQ req, bool onlyWithVehicles = false);

        Task<BaseResultInfo> GetSubCategories(BaseREQ req, bool onlyWithVehicles = false);


    }
}
