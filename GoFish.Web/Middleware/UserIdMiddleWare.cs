using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

using GoFish.Web.Factories;
using GoFish.Web.Models.Events;
using GoFish.Web.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GoFish.Web.Middleware
{
    public class UserIdMiddleWare
    {
        private const string _keyname = nameof(UserIdMiddleWare) + "|key";
        private const string _ivname = nameof(UserIdMiddleWare) + "|iv";

        private readonly RequestDelegate _next;

        public UserIdMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IKeyFactory keyFactory, IAsyncEventEmitter<UserActivity> userActivityEvent, ILogger<UserIdMiddleWare> logger)
        {
            Guid userId = default;
            bool generateNew = true;
            if (httpContext.Request.Cookies.TryGetValue("UserSession", out string UserSession))
            {
                generateNew = !TryDecryptUserId(keyFactory, out userId, UserSession, logger);
            }

            if (generateNew)
            {
                userId = Guid.NewGuid();
            }

            httpContext.Items["GameUserId"] = userId;

            string encryptedSession = EncryptUserId(keyFactory, userId);
            httpContext.Response.Cookies.Append("UserSession", encryptedSession, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(5)
            });

            await userActivityEvent.Trigger(this, new UserActivity(userId));
            await _next(httpContext);
        }

        private static string EncryptUserId(IKeyFactory keyFactory, Guid userId)
        {
            using Aes aes = Aes.Create();
            aes.Key = keyFactory.Create(_keyname);
            aes.IV = keyFactory.Create(_ivname);

            ICryptoTransform encryptor = aes.CreateEncryptor();

            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using StreamWriter sw = new StreamWriter(cs);

            sw.Write(userId.ToString());
            sw.Flush();
            cs.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }

        private static bool TryDecryptUserId(IKeyFactory keyFactory, out Guid userId, string UserSession, ILogger<UserIdMiddleWare> logger)
        {
            try
            {
                using Aes aes = Aes.Create();
                aes.Key = keyFactory.Create(_keyname);
                aes.IV = keyFactory.Create(_ivname);

                ICryptoTransform decryptor = aes.CreateDecryptor();

                using MemoryStream ms = new MemoryStream(Convert.FromBase64String(UserSession));
                using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using StreamReader sr = new StreamReader(cs);

                string decryptedUserSession = sr.ReadToEnd();

                return Guid.TryParse(decryptedUserSession, out userId);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Unable to decrypt the user session cookie.");
                userId = default;
                return false;
            }
        }
    }

    public static class UserIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseGameUserContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserIdMiddleWare>();
        }
    }
}
