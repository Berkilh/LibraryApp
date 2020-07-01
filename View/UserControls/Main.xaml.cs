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
using LibraryApp.ViewModel;
using LiveCharts;

namespace LibraryApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        private HomeViewModel dtContext;
        public Main()
        {
            InitializeComponent();

            dtContext = new HomeViewModel();
            DataContext = dtContext;
        }
         
    }
}
