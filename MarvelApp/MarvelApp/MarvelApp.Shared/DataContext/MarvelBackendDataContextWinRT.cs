using MarvelApp.Portable.Model.DataContext;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Networking.PushNotifications;
using Windows.Security.Credentials;

namespace MarvelApp.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos del backend de
    /// Azure Mobile Services. Trabajará en modo offline
    /// </summary>
    public class MarvelBackendDataContextWinRT : MarvelBackendDataContext
    {
        public override MobileServiceClient MobileServiceClient { get; set; }

        public async override Task<bool> LoginAsync(bool canShowUI)
        {
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;
            try
            {
                credential = vault.FindAllByResource(Configuration.AuthenticationProviderName).FirstOrDefault();
            }
            catch (Exception)
            {
                // Lanza una excepción si no encuentra las credenciales. Puede ignorarse
            }

            if (credential != null)
            {
                var user = new MobileServiceUser(credential.UserName);
                credential.RetrievePassword();
                user.MobileServiceAuthenticationToken = credential.Password;

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
                        vault.Remove(credential);
                        credential = null;
                    }
                }
            }

            if (canShowUI && credential == null)
            {
                // Pide al usuario su nombre y contraseña
                var user = await MobileServiceClient.LoginAsync(Configuration.AuthenticationProvider);

                credential = new PasswordCredential(
                    Configuration.AuthenticationProviderName,
                    user.UserId,
                    user.MobileServiceAuthenticationToken);
                vault.Add(credential);
            }

            return credential != null;
        }

        public override void Logout()
        {
            PasswordVault vault = new PasswordVault();
            PasswordCredential credential = null;

            try
            {
                credential = vault.FindAllByResource(Configuration.AuthenticationProviderName).FirstOrDefault();
            }
            catch (Exception)
            {
                // Lanza una excepción si no encuentra las credenciales. Puede ignorarse
            }

            if (credential != null)
            {
                vault.Remove(credential);
            }

            MobileServiceClient.Logout();
        }

        public async override Task RegisterPushAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            channel.PushNotificationReceived += OnPushNotification;
            await MobileServiceClient.GetPush().RegisterNativeAsync(channel.Uri);
        }

        private void OnPushNotification(PushNotificationChannel sender, PushNotificationReceivedEventArgs e)
        {
            String notificationContent = String.Empty;

            switch (e.NotificationType)
            {
                case PushNotificationType.Badge:
                    notificationContent = e.BadgeNotification.Content.GetXml();
                    break;

                case PushNotificationType.Tile:
                    notificationContent = e.TileNotification.Content.GetXml();
                    break;

                case PushNotificationType.Toast:
                    notificationContent = e.ToastNotification.Content.GetXml();
                    break;

                case PushNotificationType.Raw:
                    notificationContent = e.RawNotification.Content;
                    break;
            }

            // Poner a true para que no se vea el toast cuando la app esté funcionando
            e.Cancel = false;
        }


#if WINDOWS_PHONE_APP
        // Esto es necesario para completar el login en Windows Phone 8.1 
        // https://social.msdn.microsoft.com/forums/azure/en-US/95c6569e-2fa2-43c8-af71-939e006a9b27/mobile-services-loginasync-remote-procedure-call-failed-hresult-0x800706be?forum=azuremobile
        public void LoginComplete(WebAuthenticationBrokerContinuationEventArgs args)
        {
            MobileServiceClient.LoginComplete(args);
        }
#endif
    }
}
