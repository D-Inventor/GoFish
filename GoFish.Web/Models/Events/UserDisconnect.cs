using System;

using Microsoft.AspNetCore.SignalR;

namespace GoFish.Web.Models.Events
{
    public class UserDisconnect<THub> : EventArgs where THub : Hub
    {
        public UserDisconnect(string connectionId)
        {
            ConnectionId = connectionId;
        }

        public string ConnectionId { get; }
    }
}
