using System;

namespace Dawa.Models.Parameters;

public enum SRID
{
    WGS84,      // 4326
    UTMZone32   // 25832
}

public static class SridTypeExtensions
{
    public static string ToSRIDString(this SRID srid) => srid switch
    {
        SRID.WGS84 => "4326",
        SRID.UTMZone32 => "25832",
        _ => throw new ArgumentOutOfRangeException(nameof(srid), srid, null)
    };
}