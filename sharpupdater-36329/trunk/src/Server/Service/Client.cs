using System.Xml.Serialization;

namespace SharpUpdater.Server
{
    public class Client
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
         [XmlAttribute("userKey")]
        public string UserKey { get; set; }
         [XmlAttribute("publisherKey")]
        public string PublisherKey { get; set; }
        //[XmlAttribute("version")]
        //public string Version { get; set; }
    }
}