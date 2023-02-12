﻿using System;
using System.Windows;
using System.Windows.Input;
using Azure_Blobs_FileManager.ViewModels;

namespace Azure_Blobs_FileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow? _window;
        private FileManagerViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _window = this;
            _viewModel = new FileManagerViewModel();
            this.DataContext = _viewModel;
        }

        private void DragWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                _window?.DragMove();
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MinimizeWindow(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
