using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;

/// <summary>
/// This class makes it possible to recover then to store
/// the list of the stations existing in the city at the initialization.
/// </summary>
public class Stations
{
    static readonly HttpClient client = new HttpClient();
    private List<Station> stations;

    /// <summary>
    /// The constructor recovers, once and for all,
    /// all the stations present in the city
    /// </summary>
    public Stations()
    {
        string api_key = "cfe9a0679020b361f73a04113fae576b56ae6ed8";
        HttpResponseMessage response = client.GetAsync($"https://api.jcdecaux.com/vls/v3/stations?contract=toulouse&apiKey={api_key}").Result;
        string responseBody = response.Content.ReadAsStringAsync().Result;
        stations = JsonConvert.DeserializeObject<List<Station>>(responseBody);
    }

    /// <summary>
    /// This method allows you to retrieve the list of stations
    /// which is stored in the Stations class
    /// </summary>
    public List<Station> getStations()
    {
        return stations;
    }
}
