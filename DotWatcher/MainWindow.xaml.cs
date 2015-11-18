﻿using System.Windows;
using DotWatcher.Controls;
using DotWatcher.ViewModels;
using Microsoft.Win32;

namespace DotWatcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _ViewModel = new MainWindowViewModel();
        }

        private async void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                DefaultExt = ".dot",
                Filter = "DOT Files(*.dot;*.gv)|*.dot;*.gv|All files (*.*)|*.*",
                Multiselect = true
            };

            var filesSelected = openDialog.ShowDialog();
            if (filesSelected == false)
            {
                return;
            }

            foreach (var file in openDialog.FileNames)
            {
                var tab = new DotFileTabItem(file);
                await tab.LoadAsync();

                _ViewModel.Tabs.Add(tab);
            }
        }

        private async void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialogBuilder = new SaveFileDialogBuilder();
            var dialog = dialogBuilder.Build();

            var result = dialog.ShowDialog();
            if (result != false && Tabs.SelectedTab != null)
            {
                await Tabs.SelectedTab.SaveImageAsync(dialog.FileName);
            }
        }
    }
}