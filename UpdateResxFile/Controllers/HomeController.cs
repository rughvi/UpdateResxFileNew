using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UpdateResxFile.Models;

namespace UpdateResxFile.Controllers
{
    public class HomeController : Controller
    {
        private readonly int _MaxResouresCountPerPage = 100;
        private readonly IOptions<LanguageComponentSettingsModel> _languageComponentSettings;
        private readonly LanguageComponentSettingsModel _languageComponentSettingsModel;
        private Managers.ResourceManager _resourceManager;

        public HomeController(IOptions<LanguageComponentSettingsModel> languageComponentSettings)
        {
            _resourceManager = new Managers.ResourceManager();
            _languageComponentSettings = languageComponentSettings;
            _languageComponentSettingsModel = _languageComponentSettings.Value;
            _MaxResouresCountPerPage = _languageComponentSettingsModel.PerPageCount;
        }

        public IActionResult Index()
        {
            LanguageComponentResourcesModel languageComponentResourcesModel = new LanguageComponentResourcesModel();
            ViewData["LanguageComponentSettingsModel"] = _languageComponentSettingsModel;
            return View(languageComponentResourcesModel);
        }

        //[HttpPost]
        //public IActionResult Fetch(LanguageComponentModel languageComponentModel, int pageNumber = 1)
        //{
        //    List<ResourceModel> sourceResourceModels = _resourceManager.GetResources(languageComponentModel.Language, languageComponentModel.Component);
        //    LanguageComponentResourcesModel languageComponentResourcesModel = new LanguageComponentResourcesModel()
        //    {
        //        Language = languageComponentModel.Language,
        //        Component = languageComponentModel.Component,
        //        PageNumber = pageNumber,
        //        ResourceModels = sourceResourceModels.Skip(_MaxResouresCountPerPage * (pageNumber - 1)).Take(_MaxResouresCountPerPage).ToList()
        //    };

        //    return View("Fetch", languageComponentResourcesModel);
        //}

        [HttpPost]
        public IActionResult Fetch1(LanguageComponentResourcesModel languageComponentResourceModels)
        {
            if (languageComponentResourceModels.ResourceModels?.Count > 1)
            {
                _resourceManager.UpdateDestinationResources(languageComponentResourceModels);
            }

            ModelState.Clear();
            List<ResourceModel> sourceResourceModels = _resourceManager.GetResources(languageComponentResourceModels.Language, languageComponentResourceModels.Component);
            var resourceModels = (sourceResourceModels?.Skip(_MaxResouresCountPerPage * (languageComponentResourceModels.PageNumber - 1))?.Take(_MaxResouresCountPerPage)?.ToList() ?? new List<ResourceModel>());
            LanguageComponentResourcesModel languageComponentResourcesModel = new LanguageComponentResourcesModel()
            {
                Language = languageComponentResourceModels.Language,
                Component = languageComponentResourceModels.Component,
                PageNumber = languageComponentResourceModels.PageNumber + 1,
                ResourceModels = resourceModels
            };

            if (languageComponentResourcesModel.ResourceModels.Count > 0)
            {
                return View("Fetch", languageComponentResourcesModel);       
            }
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public IActionResult Update(LanguageComponentResourcesModel languageComponentResourceModels)
        //{
        //    //GenericHelper _genericHelper = new GenericHelper();
        //    _resourceManager.UpdateDestinationResources(languageComponentResourceModels);

        //    List<ResourceModel> sourceResourceModels = _resourceManager.GetResources(languageComponentResourceModels.Language, languageComponentResourceModels.Component);
        //    var nextPageResourceModels = sourceResourceModels.Skip(languageComponentResourceModels.PageNumber * _MaxResouresCountPerPage).Take(_MaxResouresCountPerPage)?.ToList();
        //    if (nextPageResourceModels?.Count > 0)
        //    {
        //        return this.Fetch(new LanguageComponentModel()
        //        {
        //            Language = languageComponentResourceModels.Language,
        //            Component = languageComponentResourceModels.Component
        //        }, languageComponentResourceModels.PageNumber + 1);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
