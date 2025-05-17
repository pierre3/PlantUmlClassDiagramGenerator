namespace Planets;

public sealed class Moon(string name, double diameter, double mass, double distanceFromPlanet)
{
    public string Name { get; } = name;
    public double Diameter { get; } = diameter;
    public double Mass { get; } = mass;
    public double DistanceFromPlanet { get; } = distanceFromPlanet;
}
