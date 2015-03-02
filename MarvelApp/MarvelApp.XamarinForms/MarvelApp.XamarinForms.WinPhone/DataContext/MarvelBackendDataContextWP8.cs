using MarvelApp.Portable.Model.DataContext;
using Microsoft.Phone.Notification;
using Microsoft.WindowsAzure.MobileServices;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace MarvelApp.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos del backend de
    /// Azure Mobile Services. Trabajará en modo offline.
    /// </summary>
    public class MarvelBackendDataContextWP8 : MarvelBackendDataContext
    {
        public override MobileServiceClient MobileServiceClient { get; set; }

        public async override Task<bool> LoginAsync(bool canShowUI)
        {
            string userName;
            string password;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("Credentials.UserName", out userName);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("Credentials.Password", out password);

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
                        IsolatedStorageSettings.ApplicationSettings.Remove("Credentials.UserName");
                        IsolatedStorageSettings.ApplicationSettings.Remove("Credentials.Password");
                        userName = password = null;
                    }
                }
            }

            if (canShowUI && (userName == null || password == null))
            {
                // Pedimos al usuario su nombre y contraseña
                var user = await MobileServiceClient.LoginAsync(Configuration.AuthenticationProvider);
                userName = user.UserId;
                password = user.MobileServiceAuthenticationToken;

                IsolatedStorageSettings.ApplicationSettings.Add("Credentials.UserName", userName);
                IsolatedStorageSettings.ApplicationSettings.Add("Credentials.Password", password);
            }

            return userName != null && password != null;
        }

        public override void Logout()
        {
            string userName;
            string password;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("Credentials.UserName", out userName);
            IsolatedStorageSettings.ApplicationSettings.TryGetValue<string>("Credentials.Password", out password);

            if (userName != null)
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("Credentials.UserName");
            }
            if (password != null)
            {
                IsolatedStorageSettings.ApplicationSettings.Remove("Credentials.Password");
            }

            MobileServiceClient.Logout();
        }

        public async override Task RegisterPushAsync()
        {
            HttpNotificationChannel httpChannel = HttpNotificationChannel.Find("MyChannel");

            if (null == httpChannel)
            {
                httpChannel = new HttpNotificationChannel("MyChannel");
                httpChannel.Open();
                httpChannel.BindToShellToast();
            }

            httpChannel.ShellToastNotificationReceived += OnPushNotification;


            await MobileServiceClient.GetPush().RegisterNativeAsync(httpChannel.ChannelUri.ToString());
        }

        void OnPushNotification(object sender, NotificationEventArgs e)
        {
            string message = "";
            foreach (string key in e.Collection.Keys)
            {
                message += e.Collection[key] + ": ";
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                // Muestra las notificaciones toast, ya que no se muestran si la app está ejecutándose
                MessageBox.Show(message, "Push notification", MessageBoxButton.OK);
            });
        }
    }
}
