using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpdateResxFile.Models
{
    public class LanguageComponentSettingsModel
    {
        public string[] Languages { get; set; }

        public string[] Components { get; set; }

        public int PerPageCount { get; set; }
    }
}
