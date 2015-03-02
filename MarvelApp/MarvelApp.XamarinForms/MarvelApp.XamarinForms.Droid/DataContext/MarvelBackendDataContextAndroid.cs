using Android.Content;
using Gcm.Client;
using MarvelApp.Portable.Model.DataContext;
using MarvelApp.XamarinForms.Droid.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MarvelApp.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos del backend de
    /// Azure Mobile Services. Trabajará en modo offline.
    /// 
    /// Para que todo vaya bien hemos añadido el componente de Azure Mobile Services al proyecto de Android,
    /// así como CurrentPlatform.Init(); en MainActivity.cs.
    /// Para poder usar las notificaciones Push hemos añadido el componente Google Cloud Messaging Client 
    /// al proyecto de Android.
    /// </summary>
    public class MarvelBackendDataContextAndroid : MarvelBackendDataContext
    {
        public override MobileServiceClient MobileServiceClient { get; set; }

        public async override Task<bool> LoginAsync(bool canShowUI)
        {
            ISharedPreferences preferences = Forms.Context.GetSharedPreferences("Credentials", FileCreationMode.Private);
            string userName = preferences.GetString("UserName", null);
            string password = preferences.GetString("Password", null);

            if (userName != null && password != null)
            {
                var user = new MobileServiceUser(userName);
                user.MobileServiceAuthenticationToken = password;

                MobileServiceClient.CurrentUser = user;

                // Prueba las credenciales para ver si siguen siendo válidas
                try
                {
                    var result = await MobileServiceClient.InvokeApiAsync<string>("Test", HttpMethod.Get, null);
                }
                catch (MobileServiceInvalidOperationException ex)
                {
                    if (ex.Response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        ISharedPreferencesEditor editor = preferences.Edit();
                        editor.Remove("UserName");
                        editor.Remove("Password");
                        editor.Commit();
                        userName = password = null;
                    }
                }
            }

            if (canShowUI && (userName == null || password == null))
            {
                // Pide al usuario su nombre y contraseña
                var user = await MobileServiceClient.LoginAsync(Forms.Context, Configuration.AuthenticationProvider);
                userName = user.UserId;
                password = user.MobileServiceAuthenticationToken;

                ISharedPreferencesEditor editor = preferences.Edit();
                editor.PutString("UserName", userName);
                editor.PutString("Password", password);
                editor.Commit();
            }

            return userName != null && password != null;
        }

        public override void Logout()
        {
            ISharedPreferences preferences = Forms.Context.GetSharedPreferences("Credentials", FileCreationMode.Private);
            string userName = preferences.GetString("UserName", null);
            string password = preferences.GetString("Password", null);

            if (userName != null && password != null)
            {
                ISharedPreferencesEditor editor = preferences.Edit();
                editor.Remove("UserName");
                editor.Remove("Password");
                editor.Commit();
            }

            MobileServiceClient.Logout();
        }

        public override Task RegisterPushAsync()
        {
            return Task.Run(() =>
                {
                    GcmClient.CheckDevice(Forms.Context);
                    GcmClient.CheckManifest(Forms.Context);
                    GcmClient.Register(Forms.Context, GcmBroadcastReceiver.SENDER_IDS);
                }
            );
        }
    }
}
