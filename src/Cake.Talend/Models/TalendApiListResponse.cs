using RestSharp.Deserializers;
using System.Collections.Generic;

namespace Cake.Talend.Models {
    /// <summary>
    /// The template to return lists from Talend API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TalendApiListResponse<T> : TalendApiResponse {
#pragma warning disable 1591

        [DeserializeAs(Name = "result")]
        public List<T> Results { get; set; }
#pragma warning restore 1591
    }
}
