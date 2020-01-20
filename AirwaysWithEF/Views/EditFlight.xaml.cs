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
using System.Windows.Shapes;
using AirwaysWithEF.ViewModels;
using AirwaysWithEF.Models;

namespace AirwaysWithEF.Views
{
    /// <summary>
    /// Логика взаимодействия для EditFlight.xaml
    /// </summary>
    public partial class EditFlight : Window
    {
        public EditFlight(TripModel trip)
        {
            InitializeComponent();
            DataContext = new EditFlightViewModel(trip);
        }

        private void OnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
