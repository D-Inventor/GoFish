using System;

using Microsoft.AspNetCore.SignalR;

namespace GoFish.Web.Models.Events
{
    public class UserConnect<THub> : EventArgs where THub : Hub
    {
        public UserConnect(string connectionId, Guid userId)
        {
            ConnectionId = connectionId;
            UserId = userId;
        }

        public string ConnectionId { get; }
        public Guid UserId { get; }
    }
}
