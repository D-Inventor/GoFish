using System;

namespace GoFish.Web.Models.Events
{
    public class UserActivity : EventArgs
    {
        public UserActivity(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
