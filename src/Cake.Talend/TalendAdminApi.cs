using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cake.Talend {
    /// <summary>
    /// Provides functionality for performing Talend Admin API calls.
    /// </summary>
    public class TalendAdminApi: ITalendAdminApi {
        private readonly string _address;
        private readonly IRestClient _restClient;
        private readonly string _username;
        private readonly string _password;


#pragma warning disable RCS1041 // Remove empty initializer.
        /// <summary>
        /// Initializes a new instance of the <see cref="TalendAdminApi"/> class.
        /// </summary>
        /// <param name="talendAdminAddress"></param>
        /// <param name="talendAdminUsername"></param>
        /// <param name="talendAdminPassword"></param>
        public TalendAdminApi(string talendAdminAddress, string talendAdminUsername, string talendAdminPassword): this(talendAdminAddress, talendAdminUsername, talendAdminPassword, new RestClient(talendAdminAddress)) {
        }
#pragma warning restore RCS1041 // Remove empty initializer.

        /// <summary>
        /// Initializes a new instance of the <see cref="TalendAdminApi"/> class.
        /// </summary>
        /// <param name="talendAdminAddress"></param>
        /// <param name="talendAdminUsername"></param>
        /// <param name="talendAdminPassword"></param>
        /// <param name="restClient">RestSharp RestClient</param>
        public TalendAdminApi(string talendAdminAddress, string talendAdminUsername, string talendAdminPassword, IRestClient restClient) {
            if(string.IsNullOrWhiteSpace(talendAdminAddress)) {
                throw new ArgumentNullException(nameof(talendAdminAddress));
            }
            if (string.IsNullOrWhiteSpace(talendAdminUsername)) {
                throw new ArgumentNullException(nameof(talendAdminUsername));
            }
            if (string.IsNullOrWhiteSpace(talendAdminPassword)) {
                throw new ArgumentNullException(nameof(talendAdminPassword));
            }

            _address = talendAdminAddress.TrimEnd(new[] { '/' });
            _username = talendAdminUsername;
            _password = talendAdminPassword;
            _restClient = restClient;
        }

        private string GetMetaservletCommand(object item) {
            var serializer = new RestSharp.Serializers.JsonSerializer();
            var serialized = serializer.Serialize(item);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serialized));
        }

        /// <summary>
        /// Executes the given command against the API or throws an exception if error.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        private T ExecuteCommand<T>(object command) where T : new() {
            var encodedCommand = GetMetaservletCommand(command);

            var request = new RestRequest($"metaServlet?{encodedCommand}", Method.GET);
            var response = _restClient.Execute<T>(request);
            if (response.ErrorException != null) {
                throw new Exception("Error when calling API: " + response.ErrorMessage);
            }

            return response.Data;
        }

        /// <summary>
        /// Lists all servers on this API.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Server> GetServerList() {
            var command = new Models.ApiCommandRequest {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.LIST_SERVERS
            };

            var data = ExecuteCommand<Models.TalendApiListResponse<Models.Server>>(command);

            if (data.ReturnCode != 0) {
                throw new Exception("Failed to list servers: " + data.Error);
            }

            return data.Results;
        }

        /// <summary>
        /// Lists all tasks on this API.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Task> GetTaskList() {
            var command = new Models.ApiCommandRequest {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.LIST_TASKS
            };

            var data = ExecuteCommand<Models.TalendApiListResponseRaw<Models.Task>>(command);

            if (data.ReturnCode != 0) {
                throw new Exception("Failed to list Tasks: " + data.Error);
            }

            return data.Results;
        }

        /// <summary>
        /// Lists all ESB tasks on this API.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.EsbTask> GetEsbTaskList() {
            var command = new Models.ApiCommandRequest {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.LIST_ESB_TASKS
            };

            var data = ExecuteCommand<Models.TalendApiListResponse<Models.EsbTask>>(command);

            if (data.ReturnCode != 0) {
                throw new Exception("Failed to list ESB Tasks: " + data.Error);
            }

            return data.Results;
        }
        
        /// <summary>
        /// Gets the ID of a task from its name.
        /// </summary>
        /// <returns></returns>
        public int GetTaskIdByName(string taskName) {
            var command = new Models.ApiCommandRequestGetTaskIdByName {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.GET_TASK_ID_BY_NAME,
                taskName = taskName
            };

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);

            if (data.ReturnCode != 0) {
                throw new Exception("Failed to get Task ID: " + data.Error);
            }

            return data.taskId;
        }

        /// <summary>
        /// Gets the ID of a task from its name.
        /// </summary>
        /// <returns></returns>
        public int GetEsbTaskIdByName(string esbTaskName) {
            var command = new Models.ApiCommandRequestGetTaskIdByName {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.GET_ESB_TASK_ID_BY_NAME,
                taskName = esbTaskName
            };

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);

            if (data.ReturnCode != 0) {
                throw new Exception("Failed to get ESB Task ID: " + data.Error);
            }

            return data.taskId;
        }

        /// <summary>
        /// Gets the first ESB Task to match the name.
        /// </summary>
        /// <param name="esbTaskName"></param>
        /// <returns></returns>
        private Models.EsbTask GetEsbTaskByName(string esbTaskName) {
            return 
                GetEsbTaskList()
                .FirstOrDefault(x => x.label.Equals(esbTaskName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Starts an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        public void StartEsbTask(int taskId) {
            var command = new Models.ApiCommandRequestTaskId {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.START_ESB_TASK,
                taskId = taskId
            };

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);
        }

        /// <summary>
        /// Stops an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        public void StopEsbTask(int taskId) {
            var command = new Models.ApiCommandRequestTaskId {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.STOP_ESB_TASK,
                taskId = taskId
            };

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);
        }

        /// <summary>
        /// Deploys an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        public void DeployEsbTask(int taskId) {
            var command = new Models.ApiCommandRequestTaskId {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.REQUEST_DEPLOY_ESB_TASK,
                taskId = taskId
            };

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);
        }
        
        /// <summary>
        /// Undeploys an ESB task.
        /// </summary>
        /// <param name="taskId"></param>
        public void UndeployEsbTask(int taskId) {
            var command = new Models.ApiCommandRequestTaskId {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.REQUEST_UNDEPLOY_ESB_TASK,
                taskId = taskId
            };

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);
        }


        private Models.ApiCommandRequestUpdateEsbTask CreateEsbUpdateRequest(Models.UpdateEsbTaskSettings updateSettings, Models.EsbTask esbTask) {
            var repositoryName = string.IsNullOrWhiteSpace(updateSettings.Repository) ? esbTask.repositoryName : updateSettings.Repository;
            var repositoryType = RepositoryEnumExtensions.Parse(repositoryName);
            var snapshotString = (repositoryType == RepositoryEnum.Snapshots) ? "-SNAPSHOT" : String.Empty;
            var versionID = string.IsNullOrWhiteSpace(updateSettings.VersionID) ? esbTask.applicationVersion.Replace("-SNAPSHOT", String.Empty) : updateSettings.VersionID;
            var featureVersion = $"{versionID}{snapshotString}";
            var contextName = string.IsNullOrWhiteSpace(updateSettings.ContextName) ? esbTask.contextName : updateSettings.ContextName;
            var cleanFeatureName = string.IsNullOrWhiteSpace(updateSettings.FeatureName) ? esbTask.applicationName.Replace("-feature", String.Empty) : updateSettings.FeatureName;
                        
            var mvnUrl = esbTask.applicationFeatureURL.Replace("mvn:", String.Empty);
            var originalGroupID = mvnUrl.Substring(0, mvnUrl.IndexOf('/'));
            var groupID = string.IsNullOrWhiteSpace(updateSettings.JobGroup) ? originalGroupID : updateSettings.JobGroup;

            return new Models.ApiCommandRequestUpdateEsbTask {
                authPass = _password,
                authUser = _username,
                actionName = TalendAdminApiCommands.UPDATE_ESB_TASK,

                description = updateSettings.Description ?? cleanFeatureName,
                featureName = $"{cleanFeatureName}-feature",
                featureUrl = $"mvn:{groupID}/{cleanFeatureName}-feature/{featureVersion}/xml",
                featureType = esbTask.applicationType,
                repository = repositoryName,
                featureVersion = featureVersion,
                runtimeContext = contextName,
                runtimePropertyId = esbTask.pid,
                runtimeServerName = esbTask.jobServerLabelHost,
                taskId = updateSettings.EsbTaskID ?? esbTask.id,
                taskName = updateSettings.EsbTaskName
            };
        }

        /// <summary>
        /// Updates an ESB task with the given information.
        /// </summary>
        /// <param name="request"></param>
        public void UpdateEsbTask(Models.UpdateEsbTaskSettings request) {
            if (string.IsNullOrWhiteSpace(request.EsbTaskName)) {
                throw new ArgumentNullException(nameof(request.EsbTaskName));
            }

            var taskDetails = GetEsbTaskByName(request.EsbTaskName);
            if (taskDetails == null) {
                throw new ArgumentException($"Unable to find task {request.EsbTaskName}.");
            }

            var command = CreateEsbUpdateRequest(request, taskDetails);

            var data = ExecuteCommand<Models.TalendApiResponseTaskId>(command);

            if (data.ReturnCode != 0) {
                throw new Exception($"Failed to update task {request.EsbTaskName}: " + data.Error);
            }

        }
    }
}
