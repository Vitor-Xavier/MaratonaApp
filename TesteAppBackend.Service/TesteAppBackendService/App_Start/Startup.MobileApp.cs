using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using TesteAppBackendService.DataObjects;
using TesteAppBackendService.Models;
using Owin;

namespace TesteAppBackendService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new TesteAppBackendInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<TesteAppBackendContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class TesteAppBackendInitializer : CreateDatabaseIfNotExists<TesteAppBackendContext>
    {
        protected override void Seed(TesteAppBackendContext context)
        {
            Cart cart = new Cart { Id = Guid.NewGuid().ToString(), Name = "Carrinho 1", Complete = true, PlannedFor = DateTime.Today };
            context.Set<Cart>().Add(cart);

            List<Item> todoItems = new List<Item>
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false, Cart = cart },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false, Cart = cart },
            };

            foreach (Item todoItem in todoItems)
            {
                context.Set<Item>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
}

