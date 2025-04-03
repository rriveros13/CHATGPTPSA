using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace PDNOriginacion.Module.BusinessObjects
{
    public partial class Buscame
    {
        [JsonProperty("birthdate")]
        public DateTimeOffset Birthdate { get; set; }

        [JsonProperty("birthplace")]
        public string Birthplace { get; set; }

        [JsonProperty("civilstatus")]
        public string Civilstatus { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }
    }

    public partial class Buscame
    {
        public static Buscame FromJson(string json) => JsonConvert.DeserializeObject<Buscame>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Buscame self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
                {
                    new IsoDateTimeConverter
                        { DateTimeStyles = DateTimeStyles.AssumeUniversal }
                },
        };
    }
}
