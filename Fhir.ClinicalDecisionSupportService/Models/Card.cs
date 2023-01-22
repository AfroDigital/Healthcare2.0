using Newtonsoft.Json;

namespace Fhir.ClinicalDecisionSupportService.Models;

public class Card
{
    public Card(string summary, Indicator indicator, Link source, string detail ="")
    {
        Summary = summary;
        Indicator = indicator;
        Detail = detail;
        Source = source;
        Links = new List<Link>();
    }

    [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
    public string Summary { get; internal set; }

    [JsonProperty("indicator", NullValueHandling = NullValueHandling.Ignore)]
    public Indicator Indicator { get; internal set; }

    [JsonProperty("detail", NullValueHandling = NullValueHandling.Ignore)]
    public string Detail { get; internal set; }

    [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
    public Link Source { get; internal set; }

    [JsonProperty("links", NullValueHandling = NullValueHandling.Ignore)]
    public IList<Link> Links { get; internal set; }

}

[JsonConverter(typeof(IndicatorConverter))]
public class Indicator
{
    public static Indicator Success
    {
        get { return new Indicator("success"); }
    }

    public static Indicator Info
    {
        get { return new Indicator("info"); }
    }

    public static Indicator Warning
    {
        get { return new Indicator("warning"); }
    }

    public static Indicator HardStop
    {
        get { return new Indicator("hard-stop"); }
    }
    private Indicator(string value)
    {
        Value = value;
    }

    public string Value { get; internal set; }

    public override string ToString()
    {
        return Value;
    }
}

class IndicatorConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var indicator = (Indicator)value;
        writer.WriteValue(indicator.Value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var indicator = (string)reader.Value;
        switch (indicator)
        {
            case "success":
                return Indicator.Success;
            case "info":
                return Indicator.Info;
            case "warning":
                return Indicator.Warning;
            case "hard-stop":
                return Indicator.HardStop;
        }
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Indicator);
    }
}

public class Link
{
    public Link(string label, string url = "")
    {
        Label = label;
        Url = url;
    }

    [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
    public string Label { get; internal set; }

    [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
    public string Url { get; internal set; }
}

public class Service
{
    public Service(string id, string hook, string name, string description)
    {
        Id = id;
        Hook = hook;
        Name = name;
        Prefetch = new Dictionary<string, string>();
        Description = description;
    }

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; internal set; }

    [JsonProperty("hook", NullValueHandling = NullValueHandling.Ignore)]
    public string Hook { get; internal set; }

    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string Name { get; internal set; }

    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string Description { get; internal set; } 

    [JsonProperty("prefetch", NullValueHandling = NullValueHandling.Ignore)]
    public IDictionary<string, string> Prefetch { get; internal set; }
}