namespace CnSharp.Windows.Updater.Service.API.Authorization
{
    public interface IAuthorization
    {
        bool ValidateUser(string clientId, string timestamp, string token);
        bool ValidatePublisher(string clientId, string timestamp, string token);
    }
}