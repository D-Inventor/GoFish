using System;

namespace GoFish.Web.Providers
{
    public interface IUserContextProvider
    {
        Guid UserId { get; }
    }
}