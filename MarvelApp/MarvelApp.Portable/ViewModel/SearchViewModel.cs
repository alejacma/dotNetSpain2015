using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model;
using MarvelApp.Portable.Model.Clients;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.Services;
using MarvelApp.Portable.ViewModel.Base;
using System;
using System.Collections.ObjectModel;

namespace MarvelApp.Portable.ViewModel
{
    /// <summary>
    /// El vista-modelo de la vista de búsqueda de personajes
    /// </summary>
    public class SearchViewModel : BaseViewModel
    {
        // Propiedades que puede utilizar la vista
        private bool loading;
        public bool Loading
        {
            get { return loading; }

            private set
            {
                SetProperty(ref loading, value);
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<Character> characters = new ObservableCollection<Character>();
        public ObservableCollection<Character> Characters { get { return characters; } }

        // Comandos que puede llamar la vista
        public RelayCommand<string> SearchCommand { get; private set; }

        public RelayCommand<Character> ShowCharacterCommand { get; private set; }

        // Dependencias del vista-modelo
        private MarvelAPI APIRepositories { get; set; }

        private INavigationService NavigationService { get; set; }

        // Inicialización
        public SearchViewModel(
            MarvelAPI apiRepositories,
            INavigationService navigationService)
        {
            APIRepositories = apiRepositories;
            NavigationService = navigationService;

            SearchCommand = new RelayCommand<string>(SearchAsync, (whatever) => !Loading);
            ShowCharacterCommand = new RelayCommand<Character>(ShowCharacter);
        }

        // Implementación de los comandos
        private async void SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            Loading = true;
            try
            {
                MarvelAPIClient client = new MarvelAPIClient();
                var characters = await client.GetCharactersAsync(query);
                Characters.Clear();
                foreach (var character in characters)
                {
                    Characters.Add(character);
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error searching characters", ex.Message, ex));
            }
            finally
            {
                Loading = false;
            }
        }

        private void ShowCharacter(Character character)
        {
            NavigationService.NavigateTo<CharacterViewModel>(character);
        }
    }
}
