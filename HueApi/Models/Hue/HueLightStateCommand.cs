namespace HueApi.Models.Hue;

public class HueLightStateCommand
{
    public HueLightStateCommand(bool isOn)
    {
        on = new On()
        {
            on = isOn
        };
    }
    
    public On on { get; set; }

    public class On
    {
        public bool on { get; set; }
    }
}