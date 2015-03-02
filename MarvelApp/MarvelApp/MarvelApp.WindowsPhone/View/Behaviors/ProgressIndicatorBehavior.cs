using Microsoft.Xaml.Interactivity;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace MarvelApp.View.Behaviors
{
    /// <summary>
    /// Para poder modificar texto del indicador de progreso de las vistas. Lo uso para ponerle el título a las páginas
    /// 
    /// Using a behavior to control the ProgressIndicator in Windows Phone 8.1 XAML Apps 
    /// http://www.visuallylocated.com/post/2014/04/06/Using-a-behavior-to-control-the-ProgressIndicator-in-Windows-Phone-81-XAML-Apps.aspx
    /// </summary>
    public class ProgressIndicatorBehavior : DependencyObject, IBehavior
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text",
            typeof(string),
            typeof(ProgressIndicatorBehavior),
            new PropertyMetadata(null, OnTextChanged));

        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
        }

        public void Detach()
        {
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            string text = null;
            if (e.NewValue != null)
            {
                text = e.NewValue.ToString();
            }
            var progressIndicator = StatusBar.GetForCurrentView().ProgressIndicator;
            progressIndicator.Text = text;
            progressIndicator.ProgressValue = 0;
            var ignoreResult = progressIndicator.ShowAsync();
        }
    }
}
