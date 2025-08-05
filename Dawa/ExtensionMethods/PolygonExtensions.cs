using System.Globalization;
using System.Text;

namespace Dawa.ExtensionMethods;

public static class PolygonExtensions
{
    public static string ToPolygonString(this double[][] coordinates)
    {
        var sb = new StringBuilder();
        sb.Append("[[");

        for (int i = 0; i < coordinates.Length; i++)
        {
            if (i > 0) sb.Append(',');
            var point = coordinates[i];
            sb.Append($"[{point[0].ToString(CultureInfo.InvariantCulture)},{point[1].ToString(CultureInfo.InvariantCulture)}]");
        }

        sb.Append("]]");
        return sb.ToString();
    }
}