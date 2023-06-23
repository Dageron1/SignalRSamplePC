using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
    public class DeathlyHallowsHub : Hub
    {
        public Dictionary<string, int> GetReceStatus()
        {
            return SD.DealthyHallowRace;
        }
    }
}
