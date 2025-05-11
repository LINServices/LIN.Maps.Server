using Newtonsoft.Json;

namespace LIN.Maps.Server.ApiModel;

public class RootObject
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("features")]
    public List<Feature> Features { get; set; }
}

public class Feature
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("geometry")]
    public Geometry Geometry { get; set; }

    [JsonProperty("properties")]
    public Properties Properties { get; set; }
}

public class Geometry
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("coordinates")]
    public List<double> Coordinates { get; set; }
}

public class Properties
{
    [JsonProperty("mapbox_id")]
    public string MapboxId { get; set; }

    [JsonProperty("feature_type")]
    public string FeatureType { get; set; }

    [JsonProperty("full_address")]
    public string FullAddress { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("name_preferred")]
    public string NamePreferred { get; set; }

    [JsonProperty("coordinates")]
    public Coordinates Coordinates { get; set; }

    [JsonProperty("place_formatted")]
    public string PlaceFormatted { get; set; }

    [JsonProperty("context")]
    public Context Context { get; set; }
}

public class Coordinates
{
    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("latitude")]
    public double Latitude { get; set; }
}

public class Context
{
    [JsonProperty("street")]
    public NamedEntity Street { get; set; }

    [JsonProperty("postcode")]
    public NamedEntity Postcode { get; set; }

    [JsonProperty("place")]
    public NamedEntity Place { get; set; }

    [JsonProperty("region")]
    public Region Region { get; set; }

    [JsonProperty("country")]
    public Country Country { get; set; }
}

public class NamedEntity
{
    [JsonProperty("mapbox_id")]
    public string MapboxId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("wikidata_id")]
    public string WikidataId { get; set; } // Puede venir null en algunos casos
}

public class Region : NamedEntity
{
    [JsonProperty("region_code")]
    public string RegionCode { get; set; }

    [JsonProperty("region_code_full")]
    public string RegionCodeFull { get; set; }
}

public class Country : NamedEntity
{
    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("country_code_alpha_3")]
    public string CountryCodeAlpha3 { get; set; }
}

