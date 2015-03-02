using MarvelApp.View.Base;
using Windows.System;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;

namespace MarvelApp.View
{
    /// <summary>
    /// Vista de búsqueda de personajes
    /// </summary>
    public sealed partial class SearchPage : BasePage
    {
        private void OnSearchTextBoxKeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(SearchButton);
                (peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider).Invoke();
            }
        }
    }
}
