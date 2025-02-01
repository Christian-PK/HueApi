namespace HueApi.Models.Hue;

public class HueGetDeviceResponse
{
    public object[] errors { get; set; }
    public Data[] data { get; set; }
}

public class Data
{
    public string id { get; set; }
    public string id_v1 { get; set; }
    public Product_data product_data { get; set; }
    public Metadata metadata { get; set; }
    public Identify identify { get; set; }
    public Services[] services { get; set; }
    public string type { get; set; }
    public Usertest usertest { get; set; }
}

public class Product_data
{
    public string model_id { get; set; }
    public string manufacturer_name { get; set; }
    public string product_name { get; set; }
    public string product_archetype { get; set; }
    public bool certified { get; set; }
    public string software_version { get; set; }
    public string hardware_platform_type { get; set; }
}

public class Metadata
{
    public string name { get; set; }
    public string archetype { get; set; }
}

public class Identify
{
}

public class Services
{
    public string rid { get; set; }
    public string rtype { get; set; }
}

public class Usertest
{
    public string status { get; set; }
    public bool usertest { get; set; }
}