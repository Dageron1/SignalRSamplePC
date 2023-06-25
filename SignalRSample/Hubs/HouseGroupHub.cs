using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
    public class HouseGroupHub : Hub
    {
        public static List<string> GroupsJoined {  get; set; } = new List<string>(); //тут еть ид всех подключений

        public async Task JoinHouse(string houseName)
        {
            if(!GroupsJoined.Contains(Context.ConnectionId+":"+houseName))
            {
                //Connectionid это уникальный Номер который имеет каждый браузер
                //если у пользователя 3 окна открыто, это будут 3 отдельный подключения
                GroupsJoined.Add(Context.ConnectionId + ":" + houseName);
                //do something else
                string houseList = "";
                foreach(var str in GroupsJoined)
                {
                    if(str.Contains(Context.ConnectionId))
                    {
                        houseList += str.Split(':')[1] + " ";
                    }
                }
                await Clients.Caller.SendAsync("subscriptionStatus", houseList, houseName.ToLower(), true);
                await Clients.Others.SendAsync("newMemberAddedToHouse", houseName);
                await Groups.AddToGroupAsync(Context.ConnectionId, houseName);
                //if(houseName == "Gryffindor")
                //{
                    //await Clients.Group("Gryffindor").SendAsync("gryffindorNotification", "Gryffindor"); //можем отпраить сообщ в определенную группу

                //}
            }
        }
        public async Task LeaveHouse(string houseName)
        {
            if (GroupsJoined.Contains(Context.ConnectionId + ":" + houseName))
            {
                //Connectionid это уникальный Номер который имеет каждый браузер
                //если у пользователя 3 окна открыто, это будут 3 отдельный подключения
                GroupsJoined.Remove(Context.ConnectionId + ":" + houseName);
                //do something else

                string houseList = "";
                foreach (var str in GroupsJoined)
                {
                    if (str.Contains(Context.ConnectionId))
                    {
                        houseList += str.Split(':')[1] + " ";
                    }
                }
                //вызываем только у того кто вызвал
                await Clients.Caller.SendAsync("subscriptionStatus", houseList, houseName.ToLower(), false);
                await Clients.Others.SendAsync("newMemberRemovedFromHouse", houseName);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, houseName);
            }
        }
        public async Task TriggerHouseNotify(string houseName)
        {
            await Clients.Group(houseName).SendAsync("triggerHouseNotification", houseName);
        }
    }
}
