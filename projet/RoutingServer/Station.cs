using System.Runtime.Serialization;

public class Station
{
    public int number { get; set; }
    public string contract_name { get; set; }
    public string name { get; set; }
    public string address { get; set; }
    public Position position { get; set; }
    public bool banking { get; set; }
    public bool bonus { get; set; }
    public string status { get; set; }

    public string lastUpdate { get; set; }

    public bool connected { get; set; }
    public bool overflow { get; set; }
    public TotalStands totalStands { get; set; }
    public MainStands mainStands { get; set; }

}

public class MainStands
{
    public Availabilities availabilities { get; set; }
    public int capacity { get; set; }
}

public class TotalStands
{
    public Availabilities availabilities { get; set; }
    public int capacity { get; set; }
}

public class Availabilities
{
    public int bikes { get; set; }
    public int stands { get; set; }
    public int mechanicalBikes { get; set; }
    public int electricalBikes { get; set; }
    public int electricalInternalBatteryBikes { get; set; }
    public int electricalRemovableBatteryBikes { get; set; }

}
public class Position
{
    public string longitude { get; set; }
    public string latitude { get; set; }
    public Position(string longitude, string latitude)
    {
        this.longitude = longitude;
        this.latitude = latitude;
    }
    static public Position setWithTab(string[] tab)
    {
        return new Position(tab[0], tab[1]);
    }
}
