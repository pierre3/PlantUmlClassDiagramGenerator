namespace SourceGeneratorTest.Planets.BaseTypes
{
    public abstract class PlanetBase(string name)
    {
        public string Name { get; set; } = name;
        public double Diameter { get; protected set; }
        public double Mass { get; protected set; }
        public double DistanceFromSun { get; protected set; }
        public double OrbitalPeriod { get; protected set; }
        public double SurfaceTemperature { get; protected set; }
        public IList<Moon> Moons { get; } = new List<Moon>();

        protected void AddMoon(Moon moon)
        {
            Moons.Add(moon);
        }
        public abstract Task Orbit();
        public abstract Task Rotate();
    }
}
