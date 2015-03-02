using MarvelApp.XamarinForms.View;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms
{
    public partial class App : Application
    {
        private static ServiceLocator serviceLocator;
        public static ServiceLocator ServiceLocator
        {
            get
            {
                return serviceLocator = serviceLocator ?? new ServiceLocator();
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
