using System;
using System.Linq;
using WxTeamsSharp.Api;

namespace WxTeamsSharp.IntegrationTests
{
    public class RoomsFixture : IDisposable
    {
        public void Dispose()
        {
            var rooms = WxTeamsApi.GetRoomsAsync().GetAwaiter().GetResult();
            var testRooms = rooms.Items.Where(x => x.Title.Contains("Awesomesauce") || x.Title.Contains("WooHoo"));

            foreach (var testRoom in testRooms)
            {
                testRoom.DeleteAsync().GetAwaiter().GetResult();
            }
        }
    }
}
