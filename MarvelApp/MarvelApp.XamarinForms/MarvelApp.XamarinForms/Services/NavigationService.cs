using MarvelApp.Portable.Services;
using MarvelApp.Portable.ViewModel;
using MarvelApp.XamarinForms.View;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MarvelApp.XamarinForms.Services
{
    /// <summary>
    /// Con este servicio los vista-modelo navegarán a las vistas asociadas a otros vista-modelo.
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// Tabla que asocia cada vista-modelo con su vista correspondiente
        /// </summary>
        private IDictionary<Type, Type> viewModelRouting = new Dictionary<Type, Type>()
        {
            { typeof(MainViewModel), typeof(MainPage) },
            { typeof(SearchViewModel), typeof(SearchPage) },
            { typeof(CharacterViewModel), typeof(CharacterPage) },
            { typeof(ComicViewModel), typeof(ComicPage) },
            { typeof(CreatorViewModel), typeof(CreatorPage) },
        };

        /// <summary>
        /// Navega a la vista asociada a un vista-modelo
        /// </summary>
        /// <typeparam name="TDestinationViewModel">Tipo del vista-modelo asociado a la vista</typeparam>
        /// <param name="navigationContext">Parámetros a pasar al vista-modelo</param>
        public void NavigateTo<TDestinationViewModel>(object navigationContext = null)
        {
            Type pageType = viewModelRouting[typeof(TDestinationViewModel)];
            Application.Current.MainPage.Navigation.PushAsync(Activator.CreateInstance(pageType, new object[] { navigationContext }) as Page);
        }

        /// <summary>
        /// Navega a la vista anterior
        /// </summary>
        public void NavigateBack()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        /// <summary>
        /// Navega a la primera vista limpiando todo el stack
        /// </summary>
        public void NavigateBackToFirst()
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
        }
    }
}