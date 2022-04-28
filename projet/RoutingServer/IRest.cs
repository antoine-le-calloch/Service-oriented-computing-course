using System.ServiceModel;
using System.ServiceModel.Web;

namespace RoutingServer
{
    [ServiceContract]
    public interface IRest
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "FindNearestStation?coord1={coord1}&coord2={coord2}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        string FindNearestStation(string coord1, string coord2);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetCoord?address={address}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json)]
        string GetCoord(string address);
    }
}