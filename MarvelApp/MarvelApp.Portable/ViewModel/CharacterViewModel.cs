using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Entities.MarvelBackend;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.Services;
using MarvelApp.Portable.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MarvelApp.Portable.ViewModel
{
    /// <summary>
    /// El vista-modelo de la vista de personaje
    /// </summary>
    public class CharacterViewModel : BaseViewModel
    {
        // Propiedades que puede utilizar la vista
        private bool loading;
        public bool Loading { get { return loading; } private set { SetProperty(ref loading, value); } }

        private Character character;
        public Character Character { get { return character; } private set { SetProperty(ref character, value); } }

        private ObservableCollection<Comic> comics = new ObservableCollection<Comic>();
        public ObservableCollection<Comic> Comics { get { return comics; } }

        // Comandos que puede llamar la vista
        public RelayCommand AddToFavoritesCommand { get; private set; }

        public RelayCommand RemoveFromFavoritesCommand { get; private set; }

        public RelayCommand ShowFavoritesCommand { get; private set; }

        public RelayCommand<Comic> ShowComicCommand { get; private set; }

        // Propiedades que modifican el estado de los comandos
        private FavoriteCharacter favoriteCharacter;
        private FavoriteCharacter FavoriteCharacter
        {
            get
            {
                return favoriteCharacter;
            }
            set
            {
                favoriteCharacter = value;
                AddToFavoritesCommand.RaiseCanExecuteChanged();
                RemoveFromFavoritesCommand.RaiseCanExecuteChanged();
            }
        }

        // Dependencias del vista-modelo
        private MarvelAPI MarvelAPI { get; set; }

        private MarvelBackend MarvelBackend { get; set; }

        private INavigationService NavigationService { get; set; }

        // Inicialización
        public CharacterViewModel(
            MarvelAPI marvelAPI,
            MarvelBackend marvelBackend,
            INavigationService navigationService)
        {
            MarvelAPI = marvelAPI;
            MarvelBackend = marvelBackend;
            NavigationService = navigationService;

            AddToFavoritesCommand = new RelayCommand(AddToFavoritesAsync, () => FavoriteCharacter == null);
            RemoveFromFavoritesCommand = new RelayCommand(RemoveFromFavoritesAsync, () => FavoriteCharacter != null);
            ShowFavoritesCommand = new RelayCommand(ShowFavorites);
            ShowComicCommand = new RelayCommand<Comic>(ShowComic);
        }

        public async override void OnNavigatedTo(object navigationContext)
        {
            Loading = true;
            try
            {
                Character = null;
                Character = navigationContext as Character;

                FavoriteCharacter = await MarvelBackend.FavoriteCharacters.FindAsync(Character.Id);

                Comics.Clear();
                await MarvelAPI.CharactersComics.FindComicsAsync(Character.Id, comic => Comics.Add(comic));
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error loading character details", ex.Message, ex));
            }
            finally
            {
                Loading = false;
            }
        }

        // Implementación de los comandos
        private async void AddToFavoritesAsync()
        {
            try
            {
                if (FavoriteCharacter == null)
                {
                    FavoriteCharacter = await MarvelBackend.FavoriteCharacters.CreateAsync(new FavoriteCharacter() { CharacterId = Character.Id });
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error adding character to favorites", ex.Message, ex));
            }
        }

        private async void RemoveFromFavoritesAsync()
        {
            try
            {
                if (FavoriteCharacter != null)
                {
                    await MarvelBackend.FavoriteCharacters.DeleteAsync(FavoriteCharacter);
                    FavoriteCharacter = null;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error removing character from favorites", ex.Message, ex));
            }
        }

        private void ShowFavorites()
        {
            NavigationService.NavigateBackToFirst();
        }

        private void ShowComic(Comic comic)
        {
            NavigationService.NavigateTo<ComicViewModel>(comic);
        }
    }
}
