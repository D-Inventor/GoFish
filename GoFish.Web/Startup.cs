using System.Collections.Generic;
using System.Security.Cryptography;

using Game.Lib;

using GoFish.Lib.Factories;
using GoFish.Lib.Models;
using GoFish.Lib.Providers;
using GoFish.Web.Factories;
using GoFish.Web.Hubs;
using GoFish.Web.Mappers;
using GoFish.Web.Middleware;
using GoFish.Web.Models;
using GoFish.Web.Providers;
using GoFish.Web.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GoFish.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddHttpContextAccessor();

            services.AddScoped<IUserContextProvider, UserContextProvider>();
            services.AddSingleton<IGameAccessor, GameAccessor>();
            services.AddScoped<IGameService, GameService>();
            services.AddTransient<IMapper<GoFishGame, GameViewModel>, GameMapper>();
            services.AddTransient<IMapper<Player, PlayerViewModel>, PlayerMapper>();
            services.AddTransient<IMapper<Card, CardViewModel>, CardMapper>();
            services.AddTransient<IGameFactory, GameFactory>();
            services.AddTransient<IGameManager<GoFishGame>, GameManager<GoFishGame>>();

            services.AddTransient<IFileCardCollectionSource, JsonFileCardCollectionSource>();
            services.AddTransient<ICardCollectionProvider>(sp => new FileCardCollectionProvider("cards.json", sp.GetRequiredService<IEnumerable<IFileCardCollectionSource>>()));
            services.AddTransient<IDeckFactory, DeckFactory>();

            services.AddSingleton<RNGCryptoServiceProvider>();
            services.AddSingleton<IKeyFactory, KeyFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpMethodOverride();

            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();
            app.UseAuthorization();

            app.UseGameUserContext();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "gofish/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<GameHub>("/gofish/gamehub");
            });
        }
    }
}
