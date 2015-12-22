using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ElectionsMobile.Resources;
using ElectionsMobile.ViewModels;

namespace ElectionsMobile
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Assign the sample data to the control data context LongListSelector
            DataContext = App.ViewModel;

        }

        // Load data for the ViewModel elements
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Manage the modified selection on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If the selected item is null (no selection), do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ItemViewModel).ID, UriKind.Relative));

            // Reset the selected element to Null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.IsDataLoaded = false;
            App.ViewModel.LoadData();
        }
    }
}