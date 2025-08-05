using System.Globalization;

namespace Dawa.Models.Parameters;

public class Cirkel
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Radius { get; set; }

    public Cirkel(double x, double y, double radius)
    {
        X = x;
        Y = y;
        Radius = radius;
    }

    public override string ToString() =>
        $"{X.ToString(CultureInfo.InvariantCulture)},{Y.ToString(CultureInfo.InvariantCulture)},{Radius.ToString(CultureInfo.InvariantCulture)}";
}
