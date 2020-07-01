using LibraryApp.View.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UserControl usc = new Main();
            GridMain.Children.Add(usc);
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
           Close();
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {

                case "ItemHome":
                    usc = new Main();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemBooks":
                    usc = new Books();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemUsers": 
                    usc = new Users();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemReservations":
                    usc = new Reservations();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    usc = new Main();
                    GridMain.Children.Add(usc);
                    break;
            }
        }
    }
}
