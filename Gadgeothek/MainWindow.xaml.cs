using System.Windows;
using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;

namespace Gadgeothek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<Gadget> gadgets;
        LibraryAdminService service;
        
        public MainWindow()
        {
            InitializeComponent();

            service = new LibraryAdminService("http://mge7.dev.ifs.hsr.ch/");
            gadgets = new ObservableCollection<Gadget>(service.GetAllGadgets());
            gadgetsGrid.ItemsSource = gadgets;

            loansGrid.ItemsSource = new ObservableCollection<Loan>(service.GetAllLoans());


            #region websocket

            #endregion
        }



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

        private void newGadget_Click(object sender, RoutedEventArgs e)
        {
            newGadget newGadget = new newGadget(service);
            newGadget.Show();
            this.Close();
        }
    }


}