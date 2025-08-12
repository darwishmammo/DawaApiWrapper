using System.Globalization;

namespace Dawa.Models.Parameters;

public class Cirkel(double x, double y, double radius)
{
    public double X { get; } = x;
    public double Y { get; } = y;
    public double Radius { get; } = radius;

    public override string ToString() =>
        $"{X.ToString(CultureInfo.InvariantCulture)},{Y.ToString(CultureInfo.InvariantCulture)},{Radius.ToString(CultureInfo.InvariantCulture)}";
}