﻿using AirwaysWithEF.ViewModels;
using AirwaysWithEF.Models;
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

namespace AirwaysWithEF.Views
{
    /// <summary>
    /// Логика взаимодействия для EditTrip.xaml
    /// </summary>
    public partial class EditTrip : Window
    {
        public EditTrip(TripModel trip)
        {
            InitializeComponent();
            DataContext = new EditTripViewModel(trip);
        }

        private void OnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void OnNextOrSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
