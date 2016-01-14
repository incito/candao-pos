using CnSharp.Windows.Updater.Service.API.Authorization;

namespace CnSharp.Windows.Updater.Service.Hosting
{
    public class XmlAuthorization : IAuthorization
    {
        private static readonly ClientStorage _clientStorage = new ClientStorage();

        public bool ValidateUser(string clientId, string timestamp, string token)
        {
            var client = _clientStorage.Get(clientId);
            if(client == null)
                return false;
            var str = clientId + timestamp + client.UserKey;
            var encrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            return token == encrypt;
        }

        public bool ValidatePublisher(string clientId, string timestamp, string token)
        {
            var client = _clientStorage.Get(clientId);
            if (client == null)
                return false;
            var str = clientId + timestamp + client.PublisherKey;
            var encrypt = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
            return token == encrypt;
        }
    }
}