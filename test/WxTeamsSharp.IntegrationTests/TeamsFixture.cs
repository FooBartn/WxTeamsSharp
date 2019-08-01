using System;
using System.Linq;
using WxTeamsSharp.Api;

namespace WxTeamsSharp.IntegrationTests
{
    public class TeamsFixture : IDisposable
    {
        public void Dispose()
        {
            var teams = WxTeamsApi.GetTeamsAsync().GetAwaiter().GetResult();
            var testTeams = teams.Items.Where(x => x.Name.Contains("Test Team"));

            foreach (var testTeam in testTeams)
            {
                testTeam.DeleteAsync().GetAwaiter().GetResult();
            }
        }
    }
}
