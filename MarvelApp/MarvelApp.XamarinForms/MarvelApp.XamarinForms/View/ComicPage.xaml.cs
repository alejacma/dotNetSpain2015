using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms.View
{
    /// <summary>
    /// Vista de cómic
    /// </summary>
    public partial class ComicPage : TabbedPage
    {
        private object Parameter { get; set; }

        public ComicPage(object parameter)
        {
            InitializeComponent ();

            BindingContext = App.ServiceLocator.ComicViewModel;

            Parameter = parameter;
        }

        protected override void OnAppearing()
        {
            Messenger.Default.Register<ErrorMessage>(this, OnErrorMessage);

            (BindingContext as ComicViewModel).OnNavigatedTo(Parameter);
        }

        protected override void OnDisappearing()
        {
            (BindingContext as ComicViewModel).OnNavigatedFrom();

            Messenger.Default.Unregister<ErrorMessage>(this, OnErrorMessage);
        }

        protected void OnErrorMessage(ErrorMessage error)
        {
            Debug.WriteLine(string.Format("ComicPage.OnErrorMessage: {0} - {1}", error.Title, error.Message));

            DisplayAlert(error.Title, error.Message, "ok");
        }

        void OnCharacterTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as ComicViewModel).ShowCharacterCommand.Execute(e.Item);
        }
        void OnCreatorTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as ComicViewModel).ShowCreatorCommand.Execute(e.Item);
        }
    }
}
