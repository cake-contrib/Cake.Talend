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
        public void TestApiShouldThrowNullExceptions() {

            // WHEN & THEN
            Assert.Throws<System.ArgumentNullException>(() => new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, null));
            Assert.Throws<System.ArgumentNullException>(() => new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, null, null));
            Assert.Throws<System.ArgumentNullException>(() => new TalendAdminApi(_settings.TalendAdminAddress, null, null, null));
            Assert.Throws<System.ArgumentNullException>(() => new TalendAdminApi(null, null, null, null));
            Assert.Throws<System.ArgumentNullException>(() => new TalendAdminApi(null, null, null));
        }

        [Fact]
        public void TestGetServerListThrowsExceptionIfNoResponse() {
            // GIVEN
            var response = Substitute.For<RestResponse<Models.TalendApiListResponse<Models.Server>>>();
            response.ErrorException = new System.Exception("ERROR");


            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponse<Models.Server>>(Arg.Any<RestRequest>()).Returns(response);

            // WHEN & THEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            Assert.Throws<System.Exception>(() => api.GetServerList());
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
                        useSSL = true,
                        adminConsolePort = 8000,
                        fileTransferPort = 8001,
                        instance = "instance 2",
                        isRuntimeServer = true,
                        mgmtRegPort = 8002,
                        mgmtServerPort = 8003,
                        monitoringPort = 8004,
                        timeOutUnknownState = 30
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

        [Fact]
        public void TestGetTaskListReturnsItems() {
            // GIVEN
            var taskList = new Models.TalendApiListResponseRaw<Models.Task> {
                ReturnCode = 0,
                ExecutionTime = new Models.TalendApiResponse.Executiontime {
                    millis = 500,
                    seconds = 2
                },
                Results = new System.Collections.Generic.List<Models.Task> {
                    new Models.Task {
                        active = "true",
                        id = "123",
                        label = "test-task",
                        addStatisticsCodeEnabled = "true",
                        applicationBundleName ="org.rsc",
                        applicationFeatureURL = "url",
                        applicationGroup = "org.rsc",
                        applicationId = "50",
                        applicationName = "one",
                        applicationType = "first",
                        applicationVersion = "0.1.1",
                        applyContextToChildren = "true",
                        artifactGroupId = "org.rsc",
                        artifactId = "org.rsc",
                        artifactVersion = "0.5",
                        awaitingExecutions = "true",
                        branch = "branch",
                        commandLineVersion = "0.1",
                        concurrentExecution = "true",
                        contextName = "context",
                        description = "describe this",
                        errorStatus = "errored",
                        execStatisticsEnabled = "false",
                        executionServerId = "013",
                        featuresName = "feature2",
                        featuresVersion = "feature0.5.1",
                        framework = "framework1",
                        frozenExecutions = "execute2",
                        idQuartzJob = "id1",
                        jobscriptarchivefilename = "/first/folder",
                        jobServerLabelHost = "label",
                        lastDeploymentDate = "one",
                        lastEndedRunDate = "1/1/1999",
                        lastRunDate = "first",
                        lastScriptGenerationDate = "1/12/2011",
                        lastTaskTraceError = "errored somewhere",
                        latestVersion = "0.2",
                        log4jLevel = "WARN",
                        nextFireDate = "05/05/2022",
                        onlineStatus = "online",
                        onUnknownStateJob = "unknown",
                        originType = "origin",
                        pid = "123",
                        processingState = "BLOCKED",
                        projectId = "42",
                        projectName = "project one",
                        regenerateJobOnChange = "regenerate",
                        remaingTimeForNextFire = "10 minutes",
                        repositoryName = "repo2",
                        runAsUser = "sa",
                        snapshot = "true",
                        status = "active",
                        svnConnectionAvailable = "false",
                        svnRevision = "0.1.5.2",
                        taskType = "ESB",
                        timeOut = "5 minutes",
                        triggersStatus = "status",
                        virtualServerLabel = "virtual label"
                    }
                }
            };
            var response = Substitute.For<RestResponse<Models.TalendApiListResponseRaw<Models.Task>>>();
            response.Data = taskList;

            var apiCommand = new Models.ApiCommandRequest {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.LIST_TASKS
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponseRaw<Models.Task>>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}"))).Returns(response);

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);
            var items = api.GetTaskList();

            // THEN
            items.ShouldNotBeEmpty();
            items.First().id.ShouldEqual("123");
            items.First().label.ShouldEqual("test-task");
        }

        [Fact]
        public void TestGetTaskListFailsIfInvalid() {
            // GIVEN
            var taskList = new Models.TalendApiListResponseRaw<Models.Task> {
                ReturnCode = 5
            };
            var response = Substitute.For<RestResponse<Models.TalendApiListResponseRaw<Models.Task>>>();
            response.Data = taskList;

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponseRaw<Models.Task>>(Arg.Any<RestRequest>()).Returns(response);

            // WHEN & THEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            Assert.Throws<System.Exception>(() => api.GetTaskList());
        }

        [Fact]
        public void TestGetEsbTaskListReturnsItems() {
            // GIVEN
            var taskList = new Models.TalendApiListResponse<Models.EsbTask> {
                ReturnCode = 0,
                ExecutionTime = new Models.TalendApiResponse.Executiontime {
                    millis = 500,
                    seconds = 2
                },
                Results = new System.Collections.Generic.List<Models.EsbTask> {
                    new Models.EsbTask {
                        label = "test-task",
                        applicationFeatureURL = "feature 2",
                        applicationName = "application 35",
                        applicationType = "type 45",
                        applicationVersion = "version 0.54",
                        contextName = "default",
                        id = 42,
                        jobServerLabelHost = "test-label",
                        pid = "102",
                        repositoryName = "repository 12"
                    }
                }
            };
            var response = Substitute.For<RestResponse<Models.TalendApiListResponse<Models.EsbTask>>>();
            response.Data = taskList;

            var apiCommand = new Models.ApiCommandRequest {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.LIST_ESB_TASKS
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponse<Models.EsbTask>>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}"))).Returns(response);

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);
            var items = api.GetEsbTaskList();

            // THEN
            items.ShouldNotBeEmpty();
            items.First().label.ShouldEqual("test-task");
        }

        [Fact]
        public void TestGetEsbTaskListFailsIfInvalid() {
            // GIVEN
            var taskList = new Models.TalendApiListResponse<Models.EsbTask> {
                ReturnCode = 5
            };
            var response = Substitute.For<RestResponse<Models.TalendApiListResponse<Models.EsbTask>>>();
            response.Data = taskList;

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiListResponse<Models.EsbTask>>(Arg.Any<RestRequest>()).Returns(response);

            // WHEN & THEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            Assert.Throws<System.Exception>(() => api.GetEsbTaskList());
        }

        [Fact]
        public void TestGetEsbTaskIdByName() {
            // GIVEN
            var taskIdResponse = new Models.TalendApiResponseTaskId {
                ReturnCode = 0,
                ExecutionTime = new Models.TalendApiResponse.Executiontime {
                    millis = 500,
                    seconds = 2
                },
                taskId = 50
            };
            var response = Substitute.For<RestResponse<Models.TalendApiResponseTaskId>>();
            response.Data = taskIdResponse;

            var apiCommand = new Models.ApiCommandRequestGetTaskIdByName {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.GET_ESB_TASK_ID_BY_NAME,
                taskName = "one"
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}"))).Returns(response);

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);
            var taskID = api.GetEsbTaskIdByName("one");

            // THEN
            taskID.ShouldEqual(50);
        }

        [Fact]
        public void TestGetEsbTaskIdByNameFailsIfInvalid() {
            // GIVEN
            var taskIdResponse = new Models.TalendApiResponseTaskId {
                ReturnCode = 5
            };
            var response = Substitute.For<RestResponse<Models.TalendApiResponseTaskId>>();
            response.Data = taskIdResponse;

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(Arg.Any<RestRequest>()).Returns(response);

            // WHEN & THEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            Assert.Throws<System.Exception>(() => api.GetEsbTaskIdByName("one"));
        }

        [Fact]
        public void TestGetTaskIdByName() {
            // GIVEN
            var taskIdResponse = new Models.TalendApiResponseTaskId {
                ReturnCode = 0,
                ExecutionTime = new Models.TalendApiResponse.Executiontime {
                    millis = 500,
                    seconds = 2
                },
                taskId = 50
            };
            var response = Substitute.For<RestResponse<Models.TalendApiResponseTaskId>>();
            response.Data = taskIdResponse;

            var apiCommand = new Models.ApiCommandRequestGetTaskIdByName {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.GET_TASK_ID_BY_NAME,
                taskName = "one"
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}"))).Returns(response);

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);
            var taskID = api.GetTaskIdByName("one");

            // THEN
            taskID.ShouldEqual(50);
        }

        [Fact]
        public void TestGetTaskIdByNameFailsIfInvalid() {
            // GIVEN
            var taskIdResponse = new Models.TalendApiResponseTaskId {
                ReturnCode = 5
            };
            var response = Substitute.For<RestResponse<Models.TalendApiResponseTaskId>>();
            response.Data = taskIdResponse;

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(Arg.Any<RestRequest>()).Returns(response);

            // WHEN & THEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            Assert.Throws<System.Exception>(() => api.GetTaskIdByName("one"));
        }

        [Fact]
        public void TestStartEsbTaskDoesntThrow() {
            // GIVEN
            var apiCommand = new Models.ApiCommandRequestTaskId {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.START_ESB_TASK,
                taskId = 42
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}")));

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            // THEN
            api.StartEsbTask(42);
            restClient.Execute<Models.TalendApiResponseTaskId>(Arg.Any<IRestRequest>()).Received();
        }

        [Fact]
        public void TestStopEsbTaskDoesntThrow() {
            // GIVEN
            var apiCommand = new Models.ApiCommandRequestTaskId {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.STOP_ESB_TASK,
                taskId = 42
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}")));

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            // THEN
            api.StopEsbTask(42);
            restClient.Execute<Models.TalendApiResponseTaskId>(Arg.Any<IRestRequest>()).Received();
        }

        [Fact]
        public void TestDeployEsbTaskDoesntThrow() {
            // GIVEN
            var apiCommand = new Models.ApiCommandRequestTaskId {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.REQUEST_DEPLOY_ESB_TASK,
                taskId = 42
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}")));

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            // THEN
            api.DeployEsbTask(42);
            restClient.Execute<Models.TalendApiResponseTaskId>(Arg.Any<IRestRequest>()).Received();
        }

        [Fact]
        public void TestUndeployEsbTaskDoesntThrow() {
            // GIVEN
            var apiCommand = new Models.ApiCommandRequestTaskId {
                authPass = _settings.TalendAdminPassword,
                authUser = _settings.TalendAdminUsername,
                actionName = TalendAdminApiCommands.REQUEST_UNDEPLOY_ESB_TASK,
                taskId = 42
            };
            var encodedApiCommand = GetMetaservletCommand(apiCommand);

            var restClient = Substitute.For<IRestClient>();
            restClient.Execute<Models.TalendApiResponseTaskId>(
                Arg.Do<RestRequest>(x => x.Resource.ShouldEqual($"metaServlet?{encodedApiCommand}")));

            // WHEN
            ITalendAdminApi api = new TalendAdminApi(_settings.TalendAdminAddress, _settings.TalendAdminUsername, _settings.TalendAdminPassword, restClient);

            // THEN
            api.UndeployEsbTask(42);
            restClient.Execute<Models.TalendApiResponseTaskId>(Arg.Any<IRestRequest>()).Received();
        }
    }
}
