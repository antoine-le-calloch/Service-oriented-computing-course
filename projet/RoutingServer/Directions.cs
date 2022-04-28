public class Directions
{
    public Features[] features { get; set; }
    public string type { get; set; }
}

public class Features
{
    public Properties properties { get; set; }
    public string type { get; set; }
}

public class Properties
{
    public Segments[] segments { get; set; }
}

public class Segments
{
    public double distance { get; set; }
    public double duration { get; set; }
}