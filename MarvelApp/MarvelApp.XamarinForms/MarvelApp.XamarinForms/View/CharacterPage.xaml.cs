using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms.View
{
    /// <summary>
    /// Vista de personaje
    /// </summary>
    public partial class CharacterPage : TabbedPage
    {
        private object Parameter { get; set; }

        public CharacterPage(object parameter)
        {
            InitializeComponent ();

            BindingContext = App.ServiceLocator.CharacterViewModel;

            Parameter = parameter;
        }

        protected override void OnAppearing()
        {
            Messenger.Default.Register<ErrorMessage>(this, OnErrorMessage);

            (BindingContext as CharacterViewModel).OnNavigatedTo(Parameter);
        }

        protected override void OnDisappearing()
        {
            (BindingContext as CharacterViewModel).OnNavigatedFrom();

            Messenger.Default.Unregister<ErrorMessage>(this, OnErrorMessage);
        }

        protected void OnErrorMessage(ErrorMessage error)
        {
            Debug.WriteLine(string.Format("CharacterPage.OnErrorMessage: {0} - {1}", error.Title, error.Message));

            DisplayAlert(error.Title, error.Message, "ok");
        }

        void OnComicTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as CharacterViewModel).ShowComicCommand.Execute(e.Item);
        }
    }
}
