using HomeManger.Data;
using HomeManger.Model;
using HomeMangerAPI.Controllers;

namespace HomeMangerAPI;

public class CategoryProxy
{
    private readonly ICategoryRepository _catRepository;
    private readonly ILogger _logger;
    public CategoryProxy(ICategoryRepository repository, ILogger<CategoryProxy> logger)
    {
        this._catRepository = repository;
        _logger = logger;
    }
    public async Task<BaseResultInfo> GetCategories(BaseREQ req)
    {
        BaseResultInfo res = await _catRepository.GetCategories(req);
        BaseResultInfo subCatRes = await _catRepository.GetSubCategories(req);
        foreach (var cat in res.GetResult<List<CategoryInfo>>()) 
        {
            cat.SubGategories = (from x in subCatRes.GetResult<List<SubCategoryInfo>>()
                                where x.CatId == cat.Id
                                select x).ToList();
        }
        return res;
    }
}
