using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;

using Autofac;

using Game.Lib;

using GoFish.Lib.Factories;
using GoFish.Lib.Providers;
using GoFish.Web.Factories;
using GoFish.Web.HostedServices;
using GoFish.Web.Hubs;
using GoFish.Web.Mappers;
using GoFish.Web.Middleware;
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
            services.AddHostedService<GameChangeNotifier>();
            services.AddHostedService<IdlePlayerDetection>();
            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddHttpContextAccessor();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UserContextProvider>().As<IUserContextProvider>().InstancePerLifetimeScope();
            builder.RegisterType<GameAccessor>().As<IGameAccessor>().SingleInstance();
            builder.RegisterType<GameService>().As<IGameService>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsClosedTypesOf(typeof(IMapper<,>)).InstancePerDependency();
            builder.RegisterType<GameFactory>().As<IGameFactory>().InstancePerDependency();
            builder.RegisterGeneric(typeof(GameManager<>)).As(typeof(IGameManager<>)).InstancePerDependency();
            builder.RegisterGenericDecorator(typeof(EventfulGameManager<>), typeof(IGameManager<>));
            builder.RegisterType<JsonFileCardCollectionSource>().As<IFileCardCollectionSource>().InstancePerDependency();
            builder.Register<ICardCollectionProvider>(cc => new FileCardCollectionProvider("cards.json", cc.Resolve<IEnumerable<IFileCardCollectionSource>>()));
            builder.RegisterType<DeckFactory>().As<IDeckFactory>().InstancePerDependency();
            builder.RegisterType<RNGCryptoServiceProvider>().SingleInstance();
            builder.RegisterType<KeyFactory>().As<IKeyFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(GenericEventEmitter<>)).As(typeof(IAsyncEventEmitter<>)).SingleInstance();
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
