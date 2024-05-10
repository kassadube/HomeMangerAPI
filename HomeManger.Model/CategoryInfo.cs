using System;
using System.Collections.Generic;
using System.Text;

namespace HomeManger.Model
{
    public class CategoryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryInfo> SubGategories { get; set; }
    }
}
