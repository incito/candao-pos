using System.Xml.Serialization;

namespace CnSharp.Windows.Updater.Service.API.Authorization
{
    public class Client
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("userKey")]
        public string UserKey { get; set; }

        [XmlAttribute("publisherKey")]
        public string PublisherKey { get; set; }
    }
}