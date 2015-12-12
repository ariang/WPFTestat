﻿using System.Windows;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using System.Text.RegularExpressions;


namespace Gadgeothek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ObservableCollection<Gadget> gadgets;
        ObservableCollection<Loan> loans;
        LibraryAdminService service;

        public MainWindow()
        {
            InitializeComponent();

            service = new LibraryAdminService("http://mge7.dev.ifs.hsr.ch/");
            gadgets = new ObservableCollection<Gadget>(service.GetAllGadgets());
            gadgetsGrid.ItemsSource = gadgets;

            loans = new ObservableCollection<Loan>(service.GetAllLoans());
            loansGrid.ItemsSource = loans;

            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += refreshLoans;
            timer.Start();
        }


        //TabControl handling
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get TabControl reference.
            var item = sender as TabControl;
            // ... Set Title to selected tab header.
            var selected = item.SelectedItem as TabItem;
            this.Title = selected.Header.ToString();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to delete this gadget?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Gadget gadget = (Gadget)gadgetsGrid.SelectedItem;
                service.DeleteGadget(gadget);

                var button = sender as Button;
                if (button != null)
                {
                    var command = button.Tag as ICommand;
                    if (command != null)
                    {
                        command.Execute(button.CommandParameter);
                    }
                }
            }
        }

        private void TheDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Delete_Click(sender, e);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (nameBox.Text.Length == 0)
            {
                errormessage.Text = "Enter name";
                nameBox.Focus();
            }
            else if (manufacturerBox.Text.Length == 0)
            {
                errormessage.Text = "Enter manufacturer";
                manufacturerBox.Focus();
            }
            else if (priceBox.Text.Length == 0)
            {
                errormessage.Text = "Enter Price";
                priceBox.Focus();
            }
            else
            {
                string name = nameBox.Text;
                string manufacturer = manufacturerBox.Text;
                double price = double.Parse(priceBox.Text);

                Gadget gadget = new Gadget(name, manufacturer, price);

                service.AddGadget(gadget);

                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }

        }


        private void refreshLoans(object sender, EventArgs e)
        {
            loans.Clear();
            Console.WriteLine("Refresh");
            service.GetAllLoans().ForEach(l => loans.Add(l));

        }


        private void priceBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextAllowed(e.Text))
            {
                errormessage.Text = "Price has to be a number";
            }

        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }
    }
}

