using System.Collections.Generic;

namespace Cake.Talend {
    /// <summary>
    /// The available methods to call on the Talend Admin API
    /// </summary>
    public interface ITalendAdminApi {
        /// <summary>
        /// Get the list of servers currently available.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Models.ListServerApiResponse.Server> GetServerList();
    }
}
