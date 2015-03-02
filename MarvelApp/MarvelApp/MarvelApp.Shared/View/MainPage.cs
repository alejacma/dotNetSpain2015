using MarvelApp.Portable.Model.Messages;
using MarvelApp.View.Base;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace MarvelApp.View
{
    /// <summary>
    /// Vista principal de favoritos
    /// </summary>
    public sealed partial class MainPage : BasePage
    {
        public MainPage()
        {
            InitializeComponent();

#if WINDOWS_PHONE_APP
            var statusBar = StatusBar.GetForCurrentView();
            statusBar.BackgroundColor = (Color)Application.Current.Resources["BrandColor"];
            statusBar.ForegroundColor = Color.FromArgb(0x00, 0xFF, 0xFF, 0xFF);
            statusBar.BackgroundOpacity = 1;
            var ignoreResult = statusBar.ShowAsync();
#endif
        }

        protected override void ShowErrorToUser(ErrorMessage error)
        {
            ErrorMessage.DataContext = error;
            ErrorMessage.Visibility = error != null ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
