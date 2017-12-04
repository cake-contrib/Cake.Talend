using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.Talend {
    /// <summary>
    /// Provides functionality for performing Talend Admin API calls.
    /// </summary>
    public class TalendAdminApi: ITalendAdminApi {
        private readonly string _address;
        private readonly IRestClient _restClient;
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// Initializes a new instance of the <see cref="TalendAdminApi"/> class.
        /// </summary>
        /// <param name="talendAdminAddress"></param>
        /// <param name="talendAdminUsername"></param>
        /// <param name="talendAdminPassword"></param>
        public TalendAdminApi(string talendAdminAddress, string talendAdminUsername, string talendAdminPassword) {
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
            _restClient = new RestClient(_address);
            _username = talendAdminUsername;
            _password = talendAdminPassword;
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
        private Models.TalendApiResponse<T> ExecuteCommand<T>(object command) where T : new() {
            var encodedCommand = GetMetaservletCommand(command);

            var request = new RestRequest($"metaServlet?{encodedCommand}", Method.GET);
            var response = _restClient.Execute<Models.TalendApiResponse<T>>(request);
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

            var data = ExecuteCommand<List<Models.Server>>(command);

            if (data.ReturnCode != 0) {
                throw new Exception("Failed to list servers: " + data.Error);
            }

            return data.Results;
        }

        
    }
}
