using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManger.Data
{
    public class CategoryRepository: BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration configuration, ILogger<CategoryRepository> logger) : base(configuration, logger)
        {

        }

    }
}
