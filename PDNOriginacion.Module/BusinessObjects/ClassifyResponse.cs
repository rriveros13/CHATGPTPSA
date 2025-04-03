using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PDNOriginacion.Module.BusinessObjects
{
    public partial class ClassifyResponse
    {
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("iteration")]
        public Guid Iteration { get; set; }

        [JsonProperty("predictions")]
        public List<Prediction> Predictions { get; set; }

        [JsonProperty("project")]
        public Guid Project { get; set; }
    }

    public class Prediction
    {
        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("tagId")]
        public Guid TagId { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }
    }

    public partial class ClassifyResponse
    {
        public static ClassifyResponse FromJson(string json) => JsonConvert.DeserializeObject<ClassifyResponse>(json,
                                                                                                                Converter.Settings);
    }
    //public static class Serialize
    //{
    //    public static string ToJson(this ClassifyResponse self) => JsonConvert.SerializeObject(self, PDNOriginacion.Module.BusinessObjects.Converter.Settings);
    //}
    //internal static class Converter
    //{
    //    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    //    {
    //        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
    //        DateParseHandling = DateParseHandling.None,
    //        Converters =
    //        {
    //            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
    //        },
    //    };
    //}
}
