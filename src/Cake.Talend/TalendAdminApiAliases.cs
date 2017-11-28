using Cake.Core;
using Cake.Core.Annotations;
using System.Collections.Generic;

namespace Cake.Talend {
    /// <summary>
    /// Provides functionality for calling Talend Admin Center API.
    /// </summary>
    [CakeAliasCategory("TalendAdminApi")]
    [CakeNamespaceImport("Cake.Talend.Models")]
    public static class TalendAdminApiAliases {


        /// <summary>
        /// Lists all servers on the Talend Admin Center.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        [CakeAliasCategory("ListServers")]
#pragma warning disable RCS1175 // Unused this parameter.
        public static IEnumerable<Models.ListServerApiResponse.Server> GetServerList(this ICakeContext context, TalendAdminApiSettings settings) {
#pragma warning restore RCS1175 // Unused this parameter.
            var api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            return api.GetServerList();
        }
    }
}
