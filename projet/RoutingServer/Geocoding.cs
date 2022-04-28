public class Geocoding
{
    public Featuress[] features { get; set; }
}

public class Featuress
{
    public Geometry geometry { get; set; }
}

public class Geometry
{
    public string[] coordinates { get; set; }
}
