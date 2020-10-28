using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdateResxFile.Models
{
    public class LanguageComponentResourcesModel
    {
        public string Language { get; set; }
        public string Component { get; set; }
        public List<ResourceModel> ResourceModels { get; set; }
        public int PageNumber { get; set; }

        public LanguageComponentResourcesModel()
        {
            this.PageNumber = 1;
        }
    }
}
