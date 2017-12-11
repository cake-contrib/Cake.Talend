using RestSharp.Deserializers;
using System.Collections.Generic;

namespace Cake.Talend.Models {
    /// <summary>
    /// The template to return lists from Talend API, but with a different root name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TalendApiListResponseRaw<T> : TalendApiResponse {
#pragma warning disable 1591
        [DeserializeAs(Name = "root")]
        public List<T> Results { get; set; }
#pragma warning restore 1591
    }
}
