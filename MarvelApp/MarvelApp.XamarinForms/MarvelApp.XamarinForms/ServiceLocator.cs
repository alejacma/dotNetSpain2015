﻿using MarvelApp.DataContext;
using MarvelApp.Portable.Model;
using MarvelApp.Portable.Model.DataContext;
using MarvelApp.Portable.Services;
using MarvelApp.Portable.ViewModel;
using MarvelApp.XamarinForms.Services;
using Microsoft.Practices.Unity;

namespace MarvelApp.XamarinForms
{
    /// <summary>
    /// Instanciamos un objeto de esta clase en el App.xaml para que las vistas puedan localizar sus 
    /// vista-modelo correspondientes. Cada vez que una vista pida su vista-modelo (en esta app lo 
    /// hacen directamente en su XAML), dicho vista-modelo se instanciará si es necesario y se le 
    /// pasarán los servicios de los que dependa según hayamos indicado en su constructor
    /// </summary>
    public class ServiceLocator
    {
        private UnityContainer container = new UnityContainer();

        public ServiceLocator()
        {
            // Modelo
#if WINDOWS_PHONE
            container.RegisterType<MarvelAPIDataContext, MarvelAPIDataContextWP8>(new ContainerControlledLifetimeManager());
            container.RegisterType<MarvelBackendDataContext, MarvelBackendDataContextWP8>(new ContainerControlledLifetimeManager());
#elif ANDROID
            container.RegisterType<MarvelAPIDataContext, MarvelAPIDataContextAndroid>(new ContainerControlledLifetimeManager());
            container.RegisterType<MarvelBackendDataContext, MarvelBackendDataContextAndroid>(new ContainerControlledLifetimeManager());
#endif
            container.RegisterType<MarvelAPI>();
            container.RegisterType<MarvelBackend>();

            // Servicios
            container.RegisterType<INavigationService, NavigationService>();

            // Vista Modelo
            container.RegisterType<MainViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<SearchViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<CharacterViewModel>();
            container.RegisterType<ComicViewModel>();
            container.RegisterType<CreatorViewModel>();
        }

        public MarvelBackendDataContext MarvelBackendDataContext { get { return container.Resolve<MarvelBackendDataContext>(); } }

        public MainViewModel MainViewModel { get { return container.Resolve<MainViewModel>(); }  }

        public SearchViewModel SearchViewModel { get { return container.Resolve<SearchViewModel>(); } }

        public CharacterViewModel CharacterViewModel { get { return container.Resolve<CharacterViewModel>(); } }

        public ComicViewModel ComicViewModel { get { return container.Resolve<ComicViewModel>(); } }

        public CreatorViewModel CreatorViewModel { get { return container.Resolve<CreatorViewModel>(); } }
    }
}
