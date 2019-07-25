using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TicTacToe.Services;
using TicTacToe.Extensions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using TicTacToe.Options;

namespace TicTacToe
{
    public class Startup
    {
        public IConfiguration m_configuration { get; }

        public Startup(IConfiguration configuration)
        {
            m_configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IUserService, UserService>();
            services.AddRouting();
            services.AddDistributedMemoryCache();
            //services.AddDirectoryBrowser();

            services.AddSingleton<IEmailService, EmailService>();

            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddLocalization(options => options.ResourcesPath = "Localization");

            services.AddMvc().AddViewLocalization(
                LanguageViewLocationExpanderFormat.Suffix,
                options => options.ResourcesPath = "Localization");

            services.Configure<EmailServiceOptions>(m_configuration.GetSection("Email"));
            services.AddSingleton<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Map("/api", ApiPipeline);
            app.Map("/web", WebPipeline);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseWebSockets();
            app.UseSession();
            app.UseCommunicationMiddleware();
            //app.UseDirectoryBrowser();

            var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            };

            localizationOptions.RequestCultureProviders.Clear();
            localizationOptions.RequestCultureProviders.Add(new CultureProviderResolverService());
            app.UseRequestLocalization(localizationOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Use(next => async context =>
            {
                await context.Response.WriteAsync("Called Use.");
                await next.Invoke(context);
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Finished with Run.");
            });
        }

        private static void ApiPipeline(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Branched to Api Pipeline.");
            });
        }

        private static void WebPipeline(IApplicationBuilder app)
        {
            app.MapWhen(context =>
            {
                return context.Request.Query.ContainsKey("usr");
            }, UserPipeline);
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Branched to Web Pipeline.");
            });
        }

        private static void UserPipeline(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Branched to User Pipeline.");
            });
        }
    }
}
