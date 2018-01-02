using Xunit;
using Should;
using NSubstitute;
using RestSharp;
using System.Linq;

namespace Cake.Talend.Tests {
    public class TalendAdminApiTests {
        private static T DeserializeMetaservletCommand<T>(string metaservletText) {
            var data = System.Convert.FromBase64String(metaservletText);
            var decodeString = System.Text.Encoding.UTF8.GetString(data);
            var deserializer = new RestSharp.Deserializers.JsonDeserializer();
            var response = new RestResponse<T>();
            response.Content = decodeString;
            return deserializer.Deserialize<T>(response);
        }

        private static string GetMetaservletCommand(object item) {
            var serializer = new RestSharp.Serializers.JsonSerializer();
            var serialized = serializer.Serialize(item);
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serialized));
        }


        private readonly TalendAdminApiSettings _settings = new TalendAdminApiSettings {
            TalendAdminPassword = "admin",
            TalendAdminUsername = "admin@company.com",
            TalendAdminAddress = "http://localhost:8080/tac"
        };

        //[Fact]
        public void Actually_Call_Api() {
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword);
            var items = api.GetServerList();
            items.ShouldNotBeEmpty();
        }

        //[Fact]
        public void Actually_Call_Api_Tasks() {
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword);
            var items = api.GetTaskList();
            items.ShouldNotBeEmpty();
        }

        //[Fact]
        public void Actually_Call_Api_Update_ESB_Tasks() {
            var updateEsbTask = new Models.UpdateEsbTaskSettings {
                EsbTaskID = 18,
                EsbTaskName = "ConsumeAzureMessage4",
                Description = "Did this work",
                JobGroup = "org.rsc"
            };

            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword);
            api.UpdateEsbTask(updateEsbTask);
        }

        [Fact]
        public void TestGetServerListReturnsItems() {
            // GIVEN
            var serverList = new Models.TalendApiListResponse<Models.Server> {
                ReturnCode = 0,
                ExecutionTime = new Models.TalendApiResponse.Executiontime {
                    millis = 500,
                    seconds = 2
                },
                Results = new System.Collections.Generic.List<Models.Server> {
                    new Models.Server {
                        active = true,
                        id = 123,
                        host ="localhost",
                        label = "test-server",
                        port = 20,
                        useSSL = true
                    }
                }
            };
            var response = Substitute.For < RestResponse<Models.TalendApiListResponse<Models.Server> > >();
            response.Data = serverList;

            var apiCommand = new Models.ApiCommandRequest {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.LIST_SERVERS
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponse<Models.Server>>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}"))).Returns(response);

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);
            var items = api.GetServerList();

            // THEN
            items.ShouldNotBeEmpty();
            items.First().port.ShouldEqual(20);
            items.First().label.ShouldEqual("test-server");
        }


        [Fact]
        public void TestGetServerListFailsIfInvalid() {
            // GIVEN
            var serverList = new Models.TalendApiListResponse<Models.Server> {
                ReturnCode = 5
            };
            var response = Substitute.For<RestResponse<Models.TalendApiListResponse<Models.Server>>>();
            response.Data = serverList;

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponse<Models.Server>>(Arg.Any<RestRequest>()).Returns(response);

            // WHEN & THEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            Assert.Throws<System.Exception>(() => api.GetServerList());
        }

    }
}
