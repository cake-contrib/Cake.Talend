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
        IEnumerable<Models.Server> GetServerList();

        /// <summary>
        /// Lists all tasks on this API.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Models.Task> GetTaskList();

        /// <summary>
        /// Lists all ESB tasks on this API.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Models.EsbTask> GetEsbTaskList();

        /// <summary>
        /// Gets the ID of a task from its name.
        /// </summary>
        /// <returns></returns>
        int GetTaskIdByName(string taskName);

        /// <summary>
        /// Gets the ID of a task from its name.
        /// </summary>
        /// <returns></returns>
        int GetEsbTaskIdByName(string esbTaskName);

        /// <summary>
        /// Updates an ESB task with the given information.
        /// </summary>
        /// <param name="request"></param>
        void UpdateEsbTask(Models.UpdateEsbTaskSettings request);

        /// <summary>
        /// Starts an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        void StartEsbTask(int taskId);

        /// <summary>
        /// Stops an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        void StopEsbTask(int taskId);

        /// <summary>
        /// Deploys an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        void DeployEsbTask(int taskId);

        /// <summary>
        /// Undeploys an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        void UndeployEsbTask(int taskId);
    }
}
