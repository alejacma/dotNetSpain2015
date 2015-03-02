using GalaSoft.MvvmLight.Messaging;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
#if WINDOWS_PHONE_APP
using Windows.Phone.UI.Input;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel.Base;
#else
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using MarvelApp.Portable.Model.Messages;
using MarvelApp.Portable.ViewModel.Base;
#endif

namespace MarvelApp.View.Base
{
    /// <summary>
    /// Clase de la que tienen que heredar todas las páginas.
    /// Gestiona la navegación por botones software (Windows 8.1) y hardware (Windows Phone 8.1).
    /// También recibe los mensajes de error de los vista-modelo.
    /// </summary>
    public class BasePage : Page
    {
        private DispatcherTimer timer;

        public BasePage()
        {
#if !WINDOWS_PHONE_APP
            GoBackCommand = new RelayCommand(GoBack);
#endif
            timer = new DispatcherTimer();
            timer.Tick += OnTimerTick;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed += OnBackPressed;
#endif
            Messenger.Default.Register<ErrorMessage>(this, OnErrorMessage);

            BaseViewModel viewModel = DataContext as BaseViewModel;
            if (viewModel != null)
            {
                viewModel.OnNavigatedTo(e.Parameter);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            BaseViewModel viewModel = DataContext as BaseViewModel;
            if (viewModel != null)
            {
                viewModel.OnNavigatedFrom();
            }

            Messenger.Default.Unregister<ErrorMessage>(this, OnErrorMessage);

#if WINDOWS_PHONE_APP
            HardwareButtons.BackPressed -= OnBackPressed;
#endif
        }

#if WINDOWS_PHONE_APP
        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }
#else
        public ICommand GoBackCommand { get; private set; }

        public void GoBack()
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
#endif

        protected void OnErrorMessage(ErrorMessage error)
        {
            Debug.WriteLine(string.Format("BasePage.OnErrorMessage: {0} - {1}", error.Title, error.Message));

            ShowErrorToUser(error);
            timer.Interval = TimeSpan.FromMilliseconds(6000);
            timer.Start();
        }

        private void OnTimerTick(object sender, object e)
        {
            ShowErrorToUser(null);
            timer.Stop();
        }

        protected virtual void ShowErrorToUser(ErrorMessage error)
        {
        }
    }
}
