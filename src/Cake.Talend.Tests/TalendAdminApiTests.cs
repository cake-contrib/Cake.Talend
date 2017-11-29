using Xunit;
using Should;

namespace Cake.Talend.Tests {
    public class TalendAdminApiTests {

        //[Fact]
        public void Actually_Call_Api() {
            var settings = new TalendAdminApiSettings {
                TalendAdminPassword = "admin",
                TalendAdminUsername = "admin@company.com",
                TalendAdminAddress = "http://localhost:8080/tac"
            };

            ITalendAdminApi api = new TalendAdminApi(settings.TalendAdminAddress, settings.TalendAdminUsername, settings.TalendAdminPassword);
            var items = api.GetServerList();
            items.ShouldNotBeEmpty();
        }
    }
}
