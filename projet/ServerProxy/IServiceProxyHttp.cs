using System.ServiceModel;
using System.ServiceModel.Web;

namespace ServerProxy
{
    [ServiceContract]
    public interface IServiceProxyHttp
    {

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetStations")]
        string GetAllStations();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "GetStation/{id}")]
        string GetSpecificStation(string id);
    }
}