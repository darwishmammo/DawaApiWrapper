using System.Globalization;
using System.Linq;

namespace Dawa.ExtensionMethods;

public static class DoubleArrayExtensions
{
    public static string ToCoordinateString(this double[] coordinates)
    {
        return $"[{string.Join(",", coordinates.Select(c => c.ToString(CultureInfo.InvariantCulture)))}]";
    }
}