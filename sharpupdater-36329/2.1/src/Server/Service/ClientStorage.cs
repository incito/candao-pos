using System.IO;
using System.Web;
using System.Xml;
using CnSharp.Windows.Updater.Service.API.Authorization;

namespace CnSharp.Windows.Updater.Service.Hosting
{
    public class ClientStorage
    {
        private static readonly string _storePath = HttpContext.Current.Server.MapPath("/App_Data/client.xml");

        public Client Get(string id)
        {
            if(!File.Exists(_storePath))
                return null;
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(_storePath);
            var node = xmlDoc.SelectSingleNode("//Client[@id='" + id+"']");
            if(node == null)
                return null;
            return new Client
                       {
                           Id = node.Attributes["id"].Value,
                           PublisherKey = node.Attributes["publisherKey"].Value,
                           UserKey = node.Attributes["userKey"].Value
                       };
        }
    }
}