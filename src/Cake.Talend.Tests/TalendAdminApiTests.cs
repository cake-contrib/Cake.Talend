using Xunit;
using Should;

namespace Cake.Talend.Tests {
    public class TalendAdminApiTests {
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
    }
}
