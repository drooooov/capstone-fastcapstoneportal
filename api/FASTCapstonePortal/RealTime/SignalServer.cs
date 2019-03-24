using FASTCapstonePortal.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace FASTCapstonePortal
{
    [Authorize]
    public class SignalServer : Hub
    {
        INotification _notification;

        public SignalServer(INotification notification)
        {
            _notification = notification;
        }
        public async override Task OnConnectedAsync()
        {
            string name = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, name);
            await base.OnConnectedAsync();
        }
        
        public void Test()
        {

        }
    }
}
