using ch.hsr.wpf.gadgeothek.domain;
using ch.hsr.wpf.gadgeothek.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Gadgeothek
{
    /// <summary>
    /// Interaction logic for newGadget.xaml
    /// </summary>
    public partial class newGadget : Window
    {
        LibraryAdminService newservice;

        public newGadget(LibraryAdminService service)
        {
            InitializeComponent();
            newservice = service;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
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
            else if(priceBox.Text.Length == 0)
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

                newservice.AddGadget(gadget);

                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }

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
