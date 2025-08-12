using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dawa.Models;

[JsonConverter(typeof(GeometryJsonConverter))]
public record Geometry
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Coordinates { get; set; }

    public Geometry(string? type, object? coordinates)
    {
        Type = type;
        Coordinates = coordinates;
    }
}

public record PointGeometry : Geometry
{
    public new double[]? Coordinates { get; set; }
    public PointGeometry(double[]? coordinates) : base("Point", coordinates)
    {
        Coordinates = coordinates;
    }
}

public record MultiPolygonGeometry : Geometry
{
    public new double[][][][]? Coordinates { get; set; }
    public MultiPolygonGeometry(double[][][][]? coordinates) : base("MultiPolygon", coordinates)
    {
        Coordinates = coordinates;
    }
}

public record PolygonGeometry : Geometry
{
    public new double[][][]? Coordinates { get; set; }
    public PolygonGeometry(double[][][]? coordinates) : base("Polygon", coordinates)
    {
        Coordinates = coordinates;
    }
}

public record MultiLineStringGeometry : Geometry
{
    public new double[][][]? Coordinates { get; set; }
    public MultiLineStringGeometry(double[][][]? coordinates) : base("MultiLineString", coordinates)
    {
        Coordinates = coordinates;
    }
}

public class GeometryJsonConverter : JsonConverter<Geometry>
{
    public override Geometry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var type = root.GetProperty("type").GetString();

        Geometry geometry = type switch
        {
            "Point" => new PointGeometry(
                JsonSerializer.Deserialize<double[]>(root.GetProperty("coordinates"))
            ),
            "Polygon" => new PolygonGeometry(
                JsonSerializer.Deserialize<double[][][]>(root.GetProperty("coordinates"))
            ),
            "MultiPolygon" => new MultiPolygonGeometry(
                JsonSerializer.Deserialize<double[][][][]>(root.GetProperty("coordinates"))
            ),
            "MultiLineString" => new MultiLineStringGeometry(
                JsonSerializer.Deserialize<double[][][]>(root.GetProperty("coordinates"))
            ),
            _ => throw new JsonException($"Unsupported geometry type: {type}")
        };

        return geometry;
    }

    public override void Write(Utf8JsonWriter writer, Geometry value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("type", value.Type);

        writer.WritePropertyName("coordinates");

        switch (value)
        {
            case PointGeometry point:
                JsonSerializer.Serialize(writer, point.Coordinates, options);
                break;
            case MultiPolygonGeometry multiPolygon:
                JsonSerializer.Serialize(writer, multiPolygon.Coordinates, options);
                break;
            case PolygonGeometry polygon:
                JsonSerializer.Serialize(writer, polygon.Coordinates, options);
                break;
            case MultiLineStringGeometry multiLineString:
                JsonSerializer.Serialize(writer, multiLineString.Coordinates, options);
                break;
            default:
                JsonSerializer.Serialize(writer, value.Coordinates, options);
                break;
        }

        writer.WriteEndObject();
    }
}