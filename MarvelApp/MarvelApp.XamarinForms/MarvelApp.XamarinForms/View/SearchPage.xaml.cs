using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms.View
{
    /// <summary>
    /// Vista de búsqueda de personajes
    /// </summary>
    public partial class SearchPage : ContentPage
    {
        private object Parameter { get; set; }

        public SearchPage(object parameter)
        {
            InitializeComponent ();

            BindingContext = App.ServiceLocator.SearchViewModel;

            Parameter = parameter;
        }

        protected override void OnAppearing()
        {
            Messenger.Default.Register<ErrorMessage>(this, OnErrorMessage);

            (BindingContext as SearchViewModel).OnNavigatedTo(Parameter);
        }

        protected override void OnDisappearing()
        {
            (BindingContext as SearchViewModel).OnNavigatedFrom();

            Messenger.Default.Unregister<ErrorMessage>(this, OnErrorMessage);
        }

        protected void OnErrorMessage(ErrorMessage error)
        {
            Debug.WriteLine(string.Format("SearchPage.OnErrorMessage: {0} - {1}", error.Title, error.Message));

            DisplayAlert(error.Title, error.Message, "ok");
        }

        void OnSearchButtonClicked(object sender, EventArgs e)
        {
            (this.BindingContext as SearchViewModel).SearchCommand.Execute(QueryEntry.Text);
        }

        void OnCharacterTapped(object sender, ItemTappedEventArgs e)
        {
            (this.BindingContext as SearchViewModel).ShowCharacterCommand.Execute(e.Item);
        }
    }
}
