using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Cake.Talend.Models {
#pragma warning disable 1591
    internal class ApiCommandRequest {
        public string authUser { get; set; }
        
        public string authPass { get; set; }
        
        public string actionName { get; set; }
    }
#pragma warning restore 1591
}
