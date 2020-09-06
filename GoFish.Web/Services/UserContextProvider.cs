using System;

using GoFish.Web.Providers;

using Microsoft.AspNetCore.Http;

namespace GoFish.Web.Services
{
    public class UserContextProvider : IUserContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => (Guid)_httpContextAccessor.HttpContext.Items["GameUserId"];
    }
}
