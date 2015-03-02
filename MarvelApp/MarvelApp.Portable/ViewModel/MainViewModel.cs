using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MarvelApp.Portable.Model;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.Services;
using MarvelApp.Portable.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MarvelApp.Portable.ViewModel
{
    /// <summary>
    /// El vista-modelo de la vista principal de favoritos
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        // Propiedades que puede utilizar la vista
        private bool loading;
        public bool Loading { get { return loading; } private set { SetProperty(ref loading, value); } }

        private ObservableCollection<Character> favoriteCharacters = new ObservableCollection<Character>();
        public ObservableCollection<Character> FavoriteCharacters { get { return favoriteCharacters; } }

        private ObservableCollection<Comic> favoriteComics = new ObservableCollection<Comic>();
        public ObservableCollection<Comic> FavoriteComics { get { return favoriteComics; } }

        private ObservableCollection<Creator> favoriteCreators = new ObservableCollection<Creator>();
        public ObservableCollection<Creator> FavoriteCreators { get { return favoriteCreators; } }

        // Comandos que puede llamar la vista
        public RelayCommand LoginCommand { get; private set; }

        public RelayCommand LogoutCommand { get; private set; }

        public RelayCommand SyncCommand { get; private set; }

        public RelayCommand SearchCommand { get; private set; }

        public RelayCommand<Character> ShowCharacterCommand { get; private set; }

        public RelayCommand<Comic> ShowComicCommand { get; private set; }

        public RelayCommand<Creator> ShowCreatorCommand { get; private set; }

        // Propiedades que modifican el estado de los comandos
        private bool successfulLogin = false;
        private bool SucessfulLogin
        {
            get { return successfulLogin; }
            set
            {
                successfulLogin = value;
                LoginCommand.RaiseCanExecuteChanged();
                LogoutCommand.RaiseCanExecuteChanged();
                SyncCommand.RaiseCanExecuteChanged();
            }
        }

        // Dependencias del vista-modelo
        private MarvelAPI MarvelAPI { get; set; }

        private MarvelBackend MarvelBackend { get; set; }

        private INavigationService NavigationService { get; set; }

        // Inicialización
        public MainViewModel(
            MarvelAPI apiRepositories,
            MarvelBackend backendRepositories,
            INavigationService navigationService)
        {
            MarvelAPI = apiRepositories;
            MarvelBackend = backendRepositories;
            NavigationService = navigationService;

            LoginCommand = new RelayCommand(LoginAsync, () => !SucessfulLogin);
            LogoutCommand = new RelayCommand(Logout, () => SucessfulLogin);
            SyncCommand = new RelayCommand(SyncAsync, () => SucessfulLogin);
            SearchCommand = new RelayCommand(Search);
            ShowCharacterCommand = new RelayCommand<Character>(ShowCharacter);
            ShowComicCommand = new RelayCommand<Comic>(ShowComic);
            ShowCreatorCommand = new RelayCommand<Creator>(ShowCreator);
        }

        public async override void OnNavigatedTo(object navigationContext)
        {
            try
            {
                await MarvelBackend.InitializeAsync();

                RefreshFavoritesAsync();

                LoginAsync(false);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error initializing favorites", ex.Message, ex));
            }
        }

        private async void RefreshFavoritesAsync()
        {
            Loading = true;
            try
            {
                await RefreshFavoriteCharactersAsync();
                await RefreshFavoriteComicsAsync();
                await RefreshFavoriteCreatorsAsync();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error refreshing favorites", ex.Message, ex));
            }
            finally
            {
                Loading = false;
            }
        }

        private async Task RefreshFavoriteCharactersAsync()
        {
            FavoriteCharacters.Clear();
            var favoriteCharacters = await MarvelBackend.FavoriteCharacters.GetAllAsync();
            foreach (var favoriteCharacter in favoriteCharacters)
            {
                var character = await MarvelAPI.Characters.FindAsync(favoriteCharacter.CharacterId, true);
                FavoriteCharacters.Add(character);
            }
        }

        private async Task RefreshFavoriteComicsAsync()
        {
            FavoriteComics.Clear();
            var favoriteComics = await MarvelBackend.FavoriteComics.GetAllAsync();
            foreach (var favoriteComic in favoriteComics)
            {
                var comic = await MarvelAPI.Comics.FindAsync(favoriteComic.ComicId, true);
                FavoriteComics.Add(comic);
            }
        }

        private async Task RefreshFavoriteCreatorsAsync()
        {
            FavoriteCreators.Clear();
            var favoriteCreators = await MarvelBackend.FavoriteCreators.GetAllAsync();
            foreach (var favoriteCreator in favoriteCreators)
            {
                var creator = await MarvelAPI.Creators.FindAsync(favoriteCreator.CreatorId, true);
                FavoriteCreators.Add(creator);
            }
        }

        private async void LoginAsync(bool canShowUI)
        {
            try
            {
                if (!SucessfulLogin)
                {
                    SucessfulLogin = await MarvelBackend.LoginAsync(canShowUI);

                    RegisterPushAsync();
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error logging in", ex.Message, ex));
            }
        }

        private async void RegisterPushAsync()
        {
            try
            {
                if (SucessfulLogin)
                {
                    await MarvelBackend.RegisterPushAsync();
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error registering for push notifications", ex.Message, ex));
            }
        }

        // Implementación de los comandos
        private void LoginAsync()
        {
            LoginAsync(true);
        }

        private void Logout()
        {
            try
            {
                MarvelBackend.Logout();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error logging out", ex.Message, ex));
            }
            finally
            {
                SucessfulLogin = false;
            }
        }

        private async void SyncAsync()
        {
            Loading = true;
            try
            {
                await MarvelBackend.SyncAllAsync();

                RefreshFavoritesAsync();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error synching favorites", ex.Message, ex));
                Loading = false;
            }
        }

        private void Search()
        {
            NavigationService.NavigateTo<SearchViewModel>();
        }

        private void ShowCharacter(Character character)
        {
            NavigationService.NavigateTo<CharacterViewModel>(character);
        }

        private void ShowComic(Comic comic)
        {
            NavigationService.NavigateTo<ComicViewModel>(comic);
        }

        private void ShowCreator(Creator creator)
        {
            NavigationService.NavigateTo<CreatorViewModel>(creator);
        }
    }
}
