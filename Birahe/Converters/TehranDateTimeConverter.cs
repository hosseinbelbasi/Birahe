using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Birahe.EndPoint.JsonConverters;

public class TehranDateTimeConverter : JsonConverters<DateTime>{
    private static readonly TimeZoneInfo TehranTimeZone =
        TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var localTime = reader.GetDateTime();
        return TimeZoneInfo.ConvertTimeToUtc(localTime, TehranTimeZone);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var tehranTime = TimeZoneInfo.ConvertTimeFromUtc(value, TehranTimeZone);
        writer.WriteStringValue(tehranTime);
    }
}