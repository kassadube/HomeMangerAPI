using HomeManger.Data;
using HomeManger.Model;
using Microsoft.AspNetCore.Mvc;

namespace HomeMangerAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryProxy _catProxy;
    private readonly ILogger _logger;

    public CategoryController(CategoryProxy catProxy, ILogger<CategoryProxy> logger)
    {
        this._catProxy = catProxy;
        this._logger = logger;
    }
    [HttpGet(Name = "GetCat")]
    public async Task<IActionResult> Get()
    {
        var catRes = await _catProxy.GetCategories(new BaseREQ() {  AccountId= 123});
       
        return this.StatusCode(200,  catRes.ResultObject);
    }
}
