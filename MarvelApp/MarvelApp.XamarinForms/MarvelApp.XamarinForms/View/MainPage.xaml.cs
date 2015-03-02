using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms.View
{
    /// <summary>
    /// Vista principal de favoritos
    /// </summary>
    public partial class MainPage : TabbedPage
    {
        public MainPage ()
        {
            InitializeComponent ();

            BindingContext = App.ServiceLocator.MainViewModel;
        }

        protected override void OnAppearing()
        {
            Messenger.Default.Register<ErrorMessage>(this, OnErrorMessage);

            (BindingContext as MainViewModel).OnNavigatedTo(null);
        }

        protected override void OnDisappearing()
        {
            (BindingContext as MainViewModel).OnNavigatedFrom();

            Messenger.Default.Unregister<ErrorMessage>(this, OnErrorMessage);
        }

        protected void OnErrorMessage(ErrorMessage error)
        {
            Debug.WriteLine(string.Format("MainPage.OnErrorMessage: {0} - {1}", error.Title, error.Message));

            DisplayAlert(error.Title, error.Message, "ok");
        }

        void OnCharacterTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as MainViewModel).ShowCharacterCommand.Execute(e.Item);
        }

        void OnComicTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as MainViewModel).ShowComicCommand.Execute(e.Item);
        }
        
        void OnCreatorTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as MainViewModel).ShowCreatorCommand.Execute(e.Item);
        }
    }
}
