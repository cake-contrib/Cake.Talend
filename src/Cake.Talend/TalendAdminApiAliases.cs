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
        [CakeAliasCategory("GetServerList")]
        public static IEnumerable<Models.Server> GetServerList(this ICakeContext context, TalendAdminApiSettings settings) {
            var api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            return api.GetServerList();
        }
        
        /// <summary>
        /// Lists all tasks on the Talend Admin Center.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        [CakeAliasCategory("GetTaskList")]
        public static IEnumerable<Models.Task> GetTaskList(this ICakeContext context, TalendAdminApiSettings settings) {
            var api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            return api.GetTaskList();
        }

        /// <summary>
        /// Lists all ESB tasks on the Talend Admin Center.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        [CakeAliasCategory("GetEsbTaskList")]
        public static IEnumerable<Models.EsbTask> GetEsbTaskList(this ICakeContext context, TalendAdminApiSettings settings) {
            var api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            return api.GetEsbTaskList();
        }

        /// <summary>
        /// Gets the ID of a task, given its name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="taskName"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        [CakeAliasCategory("GetTaskIdByName")]
        public static int GetTaskIdByName(this ICakeContext context, string taskName, TalendAdminApiSettings settings) {
            var api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            return api.GetTaskIdByName(taskName);
        }

        /// <summary>
        /// Gets the ID of a task, given its name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="esbTaskName"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        [CakeAliasCategory("GetEsbTaskIdByName")]
        public static int GetEsbTaskIdByName(this ICakeContext context, string esbTaskName, TalendAdminApiSettings settings) {
            var api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            return api.GetEsbTaskIdByName(esbTaskName);
        }
    }
}
