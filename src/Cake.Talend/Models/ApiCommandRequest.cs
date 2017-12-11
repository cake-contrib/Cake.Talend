using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Cake.Talend.Models {
    /// <summary>
    /// The base class from which all Talend API commands inherit.
    /// </summary>
    internal class ApiCommandRequest {
#pragma warning disable 1591
        public string authUser { get; set; }
        
        public string authPass { get; set; }
        
        public string actionName { get; set; }
#pragma warning restore 1591
    }
}
