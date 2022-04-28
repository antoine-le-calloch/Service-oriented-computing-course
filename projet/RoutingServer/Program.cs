using System;
using System.ServiceModel;

public class Program
{
    public static void Main(string[] args)
    {
        ServiceHost svcRest = new ServiceHost(typeof(RoutingServer.Rest));
        svcRest.Open();
        Console.ReadLine();
        ServiceHost svcSoap = new ServiceHost(typeof(RoutingServer.Soap));
        svcSoap.Open();
    }
}