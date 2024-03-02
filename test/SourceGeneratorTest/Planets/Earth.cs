using SourceGeneratorTest.Planets.BaseTypes;

namespace SourceGeneratorTest.Planets;

public class Earth : PlanetBase
{
    public Earth() : base("Earth")
    {
        Diameter = 12742;
        Mass = 5.972e24;
        DistanceFromSun = 149.6e6;
        OrbitalPeriod = 365.26;
        SurfaceTemperature = 288;
        AddMoon(new Moon("Moon", 3474, 7.35e22, 384400));
    }

    public override async Task Orbit()
    {
        await Task.Delay(5000);
    }

    public override async Task Rotate()
    {
        await Task.Delay(5000);
    }
}