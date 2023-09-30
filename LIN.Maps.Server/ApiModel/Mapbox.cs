namespace LIN.Maps.Server.ApiModel;


using Newtonsoft.Json;
using System.Collections.Generic;

public class Properties
{
    public string ShortCode { get; set; } = string.Empty;
    public string Wikidata { get; set; } = string.Empty;
    public string MapboxId { get; set; } = string.Empty;
}

public class Context
{
    public string Id { get; set; } = string.Empty;
    public string ShortCode { get; set; } = string.Empty;
    public string Wikidata { get; set; } = string.Empty;
    public string MapboxId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

public class Geometry
{
    public string Type { get; set; } = string.Empty;
    public List<double> Coordinates { get; set; } = new();
}

public class Feature
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    [JsonProperty("place_type")]
    public List<string> PlaceType { get; set; } = new();
    public double Relevance { get; set; }
    public Properties Properties { get; set; } = new();
    public string Text { get; set; } = string.Empty;

    [JsonProperty("place_name")]
    public string PlaceName { get; set; } = string.Empty;
    public List<double> Bbox { get; set; } = new();
    public List<double> Center { get; set; } = new();
    public Geometry Geometry { get; set; } = new();
    public List<Context> Context { get; set; } = new();
}

public class MapboxGeocodingResponse
{
    public string Type { get; set; } = string.Empty;
    public List<string> Query { get; set; } = new();
    public List<Feature> Features { get; set; } = new();
    public string Attribution { get; set; } = string.Empty;
}