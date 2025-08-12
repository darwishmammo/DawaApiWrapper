using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Dawa.Models;

[JsonConverter(typeof(IntOrStringJsonConverter))]
public readonly struct IntOrString
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? IntValue { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StringValue { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public bool IsString { get; }

    public IntOrString(int value)
    {
        IntValue = value;
        StringValue = null;
        IsString = false;
    }

    public IntOrString(string? value)
    {
        IntValue = null;
        StringValue = value;
        IsString = true;
    }

    public override readonly string ToString() =>
    IsString
        ? StringValue ?? string.Empty
        : (IntValue?.ToString() ?? string.Empty);
}

public class IntOrStringJsonConverter : JsonConverter<IntOrString>
{
    public override IntOrString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => new IntOrString(reader.GetInt32()),
            JsonTokenType.String => new IntOrString(reader.GetString()),
            JsonTokenType.Null => new IntOrString(null),
            _ => throw new JsonException($"Unexpected token type {reader.TokenType}")
        };
    }

    public override void Write(Utf8JsonWriter writer, IntOrString value, JsonSerializerOptions options)
    {
        if (value.IsString)
            writer.WriteStringValue(value.StringValue);
        else if (value.IntValue.HasValue)
            writer.WriteNumberValue(value.IntValue.Value);
        else
            writer.WriteNullValue();
    }
}