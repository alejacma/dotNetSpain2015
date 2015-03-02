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
    /// El vista-modelo de la vista de cómic
    /// </summary>
    public class ComicViewModel : BaseViewModel
    {
        // Propiedades que puede utilizar la vista
        private bool loading;
        public bool Loading { get { return loading; } private set { SetProperty(ref loading, value); } }

        private Comic comic;
        public Comic Comic { get { return comic; } private set { SetProperty(ref comic, value); } }

        private ObservableCollection<Character> characters = new ObservableCollection<Character>();
        public ObservableCollection<Character> Characters { get { return characters; } }

        private ObservableCollection<Creator> creators = new ObservableCollection<Creator>();
        public ObservableCollection<Creator> Creators { get { return creators; } }

        // Comandos que puede llamar la vista
        public RelayCommand AddToFavoritesCommand { get; private set; }

        public RelayCommand RemoveFromFavoritesCommand { get; private set; }

        public RelayCommand ShowFavoritesCommand { get; private set; }

        public RelayCommand<Character> ShowCharacterCommand { get; private set; }

        public RelayCommand<Creator> ShowCreatorCommand { get; private set; }

        // Propiedades que modifican el estado de los comandos
        private FavoriteComic favoriteComic;
        private FavoriteComic FavoriteComic
        {
            get
            {
                return favoriteComic;
            }
            set
            {
                favoriteComic = value;
                AddToFavoritesCommand.RaiseCanExecuteChanged();
                RemoveFromFavoritesCommand.RaiseCanExecuteChanged();
            }
        }

        // Dependencias del vista-modelo
        private MarvelAPI MarvelAPI { get; set; }

        private MarvelBackend MarvelBackend { get; set; }

        private INavigationService NavigationService { get; set; }

        // Inicialización
        public ComicViewModel(
            MarvelAPI apiRepositories,
            MarvelBackend marvelBackend,
            INavigationService navigationService)
        {
            MarvelAPI = apiRepositories;
            MarvelBackend = marvelBackend;
            NavigationService = navigationService;

            AddToFavoritesCommand = new RelayCommand(AddToFavoritesAsync, () => FavoriteComic == null);
            RemoveFromFavoritesCommand = new RelayCommand(RemoveFromFavoritesAsync, () => FavoriteComic != null);
            ShowFavoritesCommand = new RelayCommand(ShowFavorites);
            ShowCharacterCommand = new RelayCommand<Character>(ShowCharacter);
            ShowCreatorCommand = new RelayCommand<Creator>(ShowCreator);
        }

        public async override void OnNavigatedTo(object navigationContext)
        {
            Loading = true;
            try
            {
                Comic = null;
                Comic = navigationContext as Comic;

                FavoriteComic = await MarvelBackend.FavoriteComics.FindAsync(Comic.Id);

                Characters.Clear();
                await MarvelAPI.CharactersComics.FindCharactersAsync(Comic.Id, character => Characters.Add(character));

                Creators.Clear();
                await MarvelAPI.ComicsCreators.FindCreatorsAsync(Comic.Id, creator => Creators.Add(creator));
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error loading comic details", ex.Message, ex));
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
                if (FavoriteComic == null)
                {
                    FavoriteComic = await MarvelBackend.FavoriteComics.CreateAsync(new FavoriteComic() { ComicId = Comic.Id });
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error adding comic to favorites", ex.Message, ex));
            }
        }

        private async void RemoveFromFavoritesAsync()
        {
            try
            {
                if (FavoriteComic != null)
                {
                    await MarvelBackend.FavoriteComics.DeleteAsync(FavoriteComic);
                    FavoriteComic = null;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error removing comic from favorites", ex.Message, ex));
            }
        }

        private void ShowFavorites()
        {
            NavigationService.NavigateBackToFirst();
        }

        private void ShowCharacter(Character character)
        {
            NavigationService.NavigateTo<CharacterViewModel>(character);
        }

        private void ShowCreator(Creator creator)
        {
            NavigationService.NavigateTo<CreatorViewModel>(creator);
        }
    }
}
