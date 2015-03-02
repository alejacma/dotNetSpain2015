using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms.View
{
    /// <summary>
    /// Vista de creador
    /// </summary>
    public partial class CreatorPage : TabbedPage
    {
        private object Parameter { get; set; }

        public CreatorPage(object parameter)
        {
            InitializeComponent ();

            BindingContext = App.ServiceLocator.CreatorViewModel;

            Parameter = parameter;
        }


        protected override void OnAppearing()
        {
            Messenger.Default.Register<ErrorMessage>(this, OnErrorMessage);

            (BindingContext as CreatorViewModel).OnNavigatedTo(Parameter);
        }

        protected override void OnDisappearing()
        {
            (BindingContext as CreatorViewModel).OnNavigatedFrom();

            Messenger.Default.Unregister<ErrorMessage>(this, OnErrorMessage);
        }

        protected void OnErrorMessage(ErrorMessage error)
        {
            Debug.WriteLine(string.Format("CreatorPage.OnErrorMessage: {0} - {1}", error.Title, error.Message));

            DisplayAlert(error.Title, error.Message, "ok");
        }

        void OnComicTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as CreatorViewModel).ShowComicCommand.Execute(e.Item);
        }
    }
}
