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

namespace AirwaysWithEF.Views
{
    /// <summary>
    /// Логика взаимодействия для AddPassenger.xaml
    /// </summary>
    public partial class AddPassenger : Window
    {
        public AddPassenger(List<PassengersInFlightModel> pass,string param)
        {
            InitializeComponent();
            DataContext = new AddPassengerViewModel(pass,param);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
