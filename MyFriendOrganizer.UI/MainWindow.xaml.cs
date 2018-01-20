﻿using MyFriendOrganizer.UI.ViewModel;
using System.Windows;

namespace MyFriendOrganizer.UI
{

    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded; //Als de MainWindow geladen is => voer deze event uit
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
