using System.ServiceModel;
namespace RoutingServer
{
    [ServiceContract]
    public interface ISoap
    {
        [OperationContract]
        string FindPath(string startLat, string startLong, string endLat, string endLong);
    }
}