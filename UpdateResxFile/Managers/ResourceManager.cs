using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
using UpdateResxFile.Models;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace UpdateResxFile.Managers
{
    public class ResourceManager
    {
        //private const string _sourceResourceFile = @"ResxFiles\{0}\{1}\AppResourcesSource.resx";
        //private const string _destinationResourceFile = @"ResxFiles\{0}\{1}\AppResourcesDestination.resx";

        public List<ResourceModel> GetResources(string language, string component)
        {
            List<ResourceModel> resourceModels = new List<ResourceModel>();
            var sourceFilePath = Path.Combine("ResxFiles", language.ToLower(), component.ToLower(), "AppResourcesSource.resx");
            //var  = string.Format(_sourceResourceFile, language.ToLower(), component.ToLower());
            using (XmlReader reader = XmlReader.Create(sourceFilePath))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "data")
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {
                                ResourceModel resourceModel = new ResourceModel();
                                resourceModel.Key = el.Attribute("name").Value;
                                resourceModel.SourceStr = el.Element("value").Value;
                                resourceModels.Add(resourceModel);
                            }
                        }
                    }
                }
            }
            var destinationFilePath = Path.Combine("ResxFiles", language.ToLower(), component.ToLower(), "AppResourcesDestination.resx");
            //var destinationFilePath = string.Format(_destinationResourceFile, language.ToLower(), component.ToLower());
            using (XmlReader reader = XmlReader.Create(destinationFilePath))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "data")
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {

                                var key = el.Attribute("name").Value;
                                var value = el.Element("value").Value;

                                ResourceModel existingResourceModel = resourceModels.SingleOrDefault(rm => rm.Key == key);
                                if (existingResourceModel != null)
                                {
                                    existingResourceModel.DestinationStr = value;
                                }
                                else
                                {
                                    ResourceModel resourceModel = new ResourceModel()
                                    {
                                        Key = key,
                                        DestinationStr = value
                                    };
                                    resourceModels.Add(resourceModel);
                                }
                            }
                        }
                    }
                }
            }

            return resourceModels;
        }

        public void UpdateDestinationResources(LanguageComponentResourcesModel languageComponentResourceModels)
        {
            var destinationFilePath = Path.Combine("ResxFiles", languageComponentResourceModels.Language.ToLower(), languageComponentResourceModels.Component.ToLower(), "AppResourcesDestination.resx");
            //var destinationFilePath = string.Format(_destinationResourceFile, languageComponentResourceModels.Language.ToLower(), languageComponentResourceModels.Component.ToLower());
            XDocument xdoc = XDocument.Load(destinationFilePath);
            foreach (ResourceModel resourceModel in languageComponentResourceModels.ResourceModels)
            {
                var el = xdoc.Root.Elements("data").SingleOrDefault(e => e.Attribute("name").Value == resourceModel.Key)?.Element("value");
                if (el == null)
                {
                    el = new XElement("data", new XAttribute("name", resourceModel.Key), new XElement("value", resourceModel.DestinationStr));
                    //el.SetValue(resourceModel.DestinationStr ?? string.Empty);
                    xdoc.Root.Add(el);
                }
                else
                {
                    el.SetValue(resourceModel.DestinationStr ?? string.Empty);
                }
            }
            xdoc.Save(destinationFilePath);
        }
    }
}
