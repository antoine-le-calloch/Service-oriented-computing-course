using System.Diagnostics;

public class Program
{
    public static async Task Main()
    {
        var NB_REQ = 20;
        var client = new ServiceReference1.SoapClient(ServiceReference1.SoapClient.EndpointConfiguration.BasicHttpBinding_ISoap);
        
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < NB_REQ; i++)
        {
            await client.FindPathAsync("6", "6", "6", "6");
        }
        stopwatch.Stop();
        Console.WriteLine("20 requests of FindPathAsync : {0} ms \n", stopwatch.ElapsedMilliseconds);
    }
}