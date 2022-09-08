using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EviCRM.Server.Core
{
    [Authorize]
    public class signalR_chat : Hub
    {
        public async Task SendAll(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendAllExceptMe(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.Others.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendBack(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.Caller.SendAsync("broadcastMessage", name, message);
        }

    } //Канал чата - /ch_chat

    [Authorize]
    public class signalR_live : Hub
    {
        public async Task SendAll(string name, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendAllExceptMe(string name, string message)
        {
            await Clients.Others.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendBack(string name, string message)
        {
            await Clients.Caller.SendAsync("broadcastMessage", name, message);
        }

    } //Канал живых оповещений - /ch_live

    [Authorize]
    public class signalR_live_data : Hub
    {
        public async Task SendAll(string name, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendAllExceptMe(string name, string message)
        {
            await Clients.Others.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendBack(string name, string message)
        {
            await Clients.Caller.SendAsync("broadcastMessage", name, message);
        }

    }//Канал живых обновлений - /ch_data

    [Authorize]
    public class signalR_general : Hub
    {
        public async Task SendAll(string name, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendAllExceptMe(string name, string message)
        {
            await Clients.Others.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendBack(string name, string message)
        {
            await Clients.Caller.SendAsync("broadcastMessage", name, message);
        }

    }//Канал живых обновлений - /ch_general

    [Authorize]
    public class signalR_alexandra : Hub
    {
        public async Task SendAll(string name, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendAllExceptMe(string name, string message)
        {
            await Clients.Others.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendBack(string name, string message)
        {
            await Clients.Caller.SendAsync("broadcastMessage", name, message);
        }

    }//Канал Александры - /ch_general

    [Authorize]
    public class signalR_debug : Hub
    {
        public async Task SendAll(string name, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendAllExceptMe(string name, string message)
        {
            await Clients.Others.SendAsync("broadcastMessage", name, message);
        }
        public async Task SendBack(string name, string message)
        {
            await Clients.Caller.SendAsync("broadcastMessage", name, message);
        }
    }//Канал для отладки - /ch_debug

}