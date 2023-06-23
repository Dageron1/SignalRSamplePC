using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
    public class UserHub : Hub
    {
        public static int TotalViews { get; set; } = 0;
        public static int TotalUsers { get; set; } = 0;

        //методы коннект и дисконнект запускаются автоматически при подключении и отключении
        public override Task OnConnectedAsync()
        {
            TotalUsers++;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            TotalUsers--;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnDisconnectedAsync(exception);
        }
        public async Task<string> NewWindowLoaded(string name) //теперь возвращаемый тип string
        {
            TotalViews++;
            //send update to all clients that total views have been updated
            //updateTotalViews расположен на client side
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
            return $"total views from {name} - {TotalViews}";
        }
    }
}
