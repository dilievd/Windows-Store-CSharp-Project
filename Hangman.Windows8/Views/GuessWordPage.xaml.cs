using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hangman.Windows8.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Hangman.Windows8.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuessWordPage : Page
    {
        public GuessWordPage()
        {
            this.InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Snapped)
            {
                landscapeContent.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                snapViewContent.Visibility = Windows.UI.Xaml.Visibility.Visible;

            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.Filled || 
                Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenLandscape)
            {
                gamefieldSP.Orientation = Orientation.Horizontal;
                landscapeContent.Visibility = Windows.UI.Xaml.Visibility.Visible;
                snapViewContent.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if (Windows.UI.ViewManagement.ApplicationView.Value == Windows.UI.ViewManagement.ApplicationViewState.FullScreenPortrait)
            {
                gamefieldSP.Orientation = Orientation.Vertical;
            }
        }

        DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dataTransferManager.DataRequested += ShareTextHandler;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            dataTransferManager.DataRequested -= ShareTextHandler;
        }

        private void ShareTextHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = "Играя 'Бесеница'"; // You must have to set title.
            request.Data.SetText("Познах " + guessedwordsTB.Text + " думи в играта 'Бесеница'. Изтегли си я от Windows Store и пробвай да ме изпревариш!");
        }
    }
}
