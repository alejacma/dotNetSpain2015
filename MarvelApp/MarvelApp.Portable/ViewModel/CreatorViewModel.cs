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
    /// El vista-modelo de la vista de creador
    /// </summary>
    public class CreatorViewModel : BaseViewModel
    {
        // Propiedades que puede utilizar la vista
        private bool loading;
        public bool Loading { get { return loading; } private set { SetProperty(ref loading, value); } }

        private Creator creator;
        public Creator Creator { get { return creator; } private set { SetProperty(ref creator, value); } }

        private ObservableCollection<Comic> comics = new ObservableCollection<Comic>();
        public ObservableCollection<Comic> Comics { get { return comics; } }

        // Comandos que puede llamar la vista
        public RelayCommand AddToFavoritesCommand { get; private set; }

        public RelayCommand RemoveFromFavoritesCommand { get; private set; }

        public RelayCommand ShowFavoritesCommand { get; private set; }

        public RelayCommand<Comic> ShowComicCommand { get; private set; }

        // Propiedades que modifican el estado de los comandos
        private FavoriteCreator favoriteCreator;
        private FavoriteCreator FavoriteCreator
        {
            get
            {
                return favoriteCreator;
            }
            set
            {
                favoriteCreator = value;
                AddToFavoritesCommand.RaiseCanExecuteChanged();
                RemoveFromFavoritesCommand.RaiseCanExecuteChanged();
            }
        }

        // Dependencias del vista-modelo
        private MarvelAPI MarvelAPI { get; set; }

        private MarvelBackend MarvelBackend { get; set; }

        private INavigationService NavigationService { get; set; }

        // Inicialización
        public CreatorViewModel(
            MarvelAPI marvelAPI,
            MarvelBackend marvelBackend,
            INavigationService navigationService)
        {
            MarvelAPI = marvelAPI;
            MarvelBackend = marvelBackend;
            NavigationService = navigationService;

            AddToFavoritesCommand = new RelayCommand(AddToFavoritesAsync, () => FavoriteCreator == null);
            RemoveFromFavoritesCommand = new RelayCommand(RemoveFromFavoritesAsync, () => FavoriteCreator != null);
            ShowFavoritesCommand = new RelayCommand(ShowFavorites);
            ShowComicCommand = new RelayCommand<Comic>(ShowComic);
        }

        public async override void OnNavigatedTo(object navigationContext)
        {
            Loading = true;
            try
            {
                Creator = null;
                Creator = navigationContext as Creator;

                FavoriteCreator = await MarvelBackend.FavoriteCreators.FindAsync(Creator.Id);

                Comics.Clear();
                await MarvelAPI.ComicsCreators.FindComicsAsync(Creator.Id, comic => Comics.Add(comic));
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error loading creator details", ex.Message, ex));
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
                if (FavoriteCreator == null)
                {
                    FavoriteCreator = await MarvelBackend.FavoriteCreators.CreateAsync(new FavoriteCreator() { CreatorId = Creator.Id });
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error adding creator to favorites", ex.Message, ex));
            }
        }

        private async void RemoveFromFavoritesAsync()
        {
            try
            {
                if (FavoriteCreator != null)
                {
                    await MarvelBackend.FavoriteCreators.DeleteAsync(FavoriteCreator);
                    FavoriteCreator = null;
                }
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<ErrorMessage>(new ErrorMessage("Error removing creator from favorites", ex.Message, ex));
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
