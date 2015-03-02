using MarvelApp.Portable.Model.Messages;
using MarvelApp.View.Base;
using Windows.UI.Xaml;

namespace MarvelApp.View
{
    /// <summary>
    /// Vista de creador
    /// </summary>
    public sealed partial class CreatorPage : BasePage
    {
        public CreatorPage()
        {
            InitializeComponent();
        }

        protected override void ShowErrorToUser(ErrorMessage error)
        {
            ErrorMessage.DataContext = error;
            ErrorMessage.Visibility = error != null ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
