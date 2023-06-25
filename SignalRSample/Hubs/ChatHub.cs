using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;
using System.Security.Claims;

namespace SignalRSample.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db)
        {
            _db = db;
        }
        //когда коннект создан, пытаемся найти userId
        public override Task OnConnectedAsync()
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(UserId))
            {
                //находим имя пользователся с помощью его собственного ид
                var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserConnected", UserId, userName);

                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(HubConnections.HasUserConnection(UserId, Context.ConnectionId))
            {
                var UserConnections = HubConnections.Users[UserId]; //получаем все connections из хаба подключений
                UserConnections.Remove(Context.ConnectionId); //we only want to remove one connection that has been disconnected

                HubConnections.Users.Remove(UserId);
                if (UserConnections.Any())
                {
                    HubConnections.Users.Add(UserId, UserConnections);
                }
            }

            if (!String.IsNullOrEmpty(UserId))
            {
                //находим имя пользователся с помощью его собственного ид
                var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("ReceiveUserDisconnected", UserId, userName);

                HubConnections.AddUserConnection(UserId, Context.ConnectionId);
            }
            return base.OnDisconnectedAsync(exception);
        }

        //public async Task SendMessageToAll(string user, string message)
        //{
        //    await Clients.All.SendAsync("MessageReceived", user, message);
        //}
        //[Authorize] //можно добавить и роль и полиси
        //public async Task SendMessageToReceiver(string sender, string receiver, string message)
        //{
        //    var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == receiver.ToLower()).Id;

        //    if(!string.IsNullOrEmpty(userId) )
        //    {
        //        await Clients.User(userId).SendAsync("MessageReceived", sender, message);
        //    }

        //}
    }
}
