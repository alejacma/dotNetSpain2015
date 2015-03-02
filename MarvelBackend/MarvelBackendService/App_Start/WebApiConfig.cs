using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using MarvelBackendService.DataObjects;
using MarvelBackendService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace MarvelBackendService
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();
            options.PushAuthorization = AuthorizationLevel.User;

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MarvelBackendInitializer());
        }
    }

    public class MarvelBackendInitializer : ClearDatabaseSchemaIfModelChanges<MarvelBackendContext>
    {
        //protected override void Seed(MarvelBackendContext context)
        //{
        //    List<TodoItem> todoItems = new List<TodoItem>
        //    {
        //        new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
        //        new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false },
        //    };

        //    foreach (TodoItem todoItem in todoItems)
        //    {
        //        context.Set<TodoItem>().Add(todoItem);
        //    }

        //    base.Seed(context);
        //}
    }
}

