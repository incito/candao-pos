using System;
using CnSharp.Windows.Updater.Service.API.Authorization;
using CnSharp.Windows.Updater.Service.API.UpdateLogStorage;
using CnSharp.Windows.Updater.Util;

namespace CnSharp.Windows.Updater.Service.Hosting
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly IAuthorization _auth = new XmlAuthorization();
        private readonly IUpdateLogStorage _updateLogStorage = new UpdateLogXmlStorage();

        protected void Page_Load(object sender, EventArgs e)
        {
            var clientId = Request.QueryString["cid"];
            var timestamp = Request.QueryString["time"];
            var token = Request.QueryString["token"];
            if (IsStringBlank(clientId) || IsStringBlank(timestamp) || IsStringBlank(token))
            {
                Response.Write("Invalid Request");
                Response.End();
                return;
            }
            bool pass;
            switch(Request.QueryString["m"].ToLower())
            {
                case "publish":
                    pass = _auth.ValidatePublisher(clientId, timestamp, token);
                    if (!pass)
                    {
                        Response.Write("Forbidden");
                        Response.End();
                        return;
                    }
                    var version = Request.Params["v"];
                    if (IsStringBlank(version))
                    {
                        Response.Write("Invalid Request");
                        Response.End();
                        return;
                    }
                    _updateLogStorage.Write(clientId, new UpdateLog
                    {
                        Description = Request.Params["desc"],
                        Version = version,
                        ReleaseDate = DateTime.Now.ToString()
                    });
                    Response.Write("OK");
                    break;
                case "getlog":
                    var baseVersion = Request.QueryString["baseversion"];
                    if(IsStringBlank(baseVersion))
                    {
                        Response.Write("Invalid Request");
                        Response.End();
                        return;
                    }
                    var topVersion = Request.QueryString["topversion"];
                    if (IsStringBlank(topVersion))
                    {
                        Response.Write("Invalid Request");
                        Response.End();
                        return;
                    }
                    pass = _auth.ValidateUser(clientId, timestamp, token);
                    if (!pass)
                    {
                        Response.Write("Forbidden");
                        Response.End();
                        return;
                    }
                    var versions = _updateLogStorage.GetBetweenVersion(clientId, baseVersion,topVersion);
                    Response.Write(XmlSerializerHelper.GetXmlStringFromObject(versions));
                    break;
            }
        }

        private static bool IsStringBlank(string str)
        {
            return string.IsNullOrEmpty(str) || str.Trim().Length == 0;
        }
    }
}