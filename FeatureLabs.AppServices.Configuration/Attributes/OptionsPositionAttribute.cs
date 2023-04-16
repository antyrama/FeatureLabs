namespace FeatureLabs.AppServices.Configuration.Attributes;

public class OptionsPositionAttribute : Attribute
{
    public OptionsPositionAttribute(string position)
    {
        Position = position;
    }
    public string Position { get; }
}
