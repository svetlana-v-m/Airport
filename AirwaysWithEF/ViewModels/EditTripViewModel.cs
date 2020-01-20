using AirwaysWithEF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirwaysWithEF.ViewModels
{
    class EditTripViewModel : INotifyPropertyChanged
    {
        #region Properties
        public TripModel Trip { get; set; }
        private int tripNo;
        public int TripNo
        {
            get { return tripNo; }
            set { tripNo = value; OnPropertyChanged("TripNo"); }
        }
        private DateTime depTime;
        public DateTime DepTime
        {
            get { return depTime; }
            set
            {
                depTime = new DateTime(1900,01,01,value.Hour,value.Minute,value.Second);
                if (FlightTime != TimeSpan.FromSeconds(0)) ArrTime = DepTime + FlightTime;
                OnPropertyChanged("DepTime");
            }
        }
        private DateTime arrTime;
        public DateTime ArrTime
        {
            get { return arrTime; }
            set
            {
                arrTime = new DateTime(1900,01,01,value.Hour,value.Minute,value.Second);
                ArrTimeString = value.ToShortTimeString();
                OnPropertyChanged("ArrTime");
            }
        }
        private string arrTimeString;
        public string ArrTimeString
        {
            get { return arrTimeString; }
            set { arrTimeString = value;OnPropertyChanged("ArrtimeString"); }
        }
        private TimeSpan flightTime;
        public TimeSpan FlightTime
        {
            get { return flightTime; }
            set
            {
                flightTime = value;
                ArrTime = DepTime + FlightTime;
                OnPropertyChanged("FlightTime");
            }
        }
        private string flightTimeIsEnabled;
        public string FlightTimeIsEnabled
        {
            get { return flightTimeIsEnabled; }
            set { flightTimeIsEnabled = value; OnPropertyChanged("FlightTimeIsEnabled"); }
        }
        private string airwayCompanyName;
        public string AirwayCompanyName
        {
            get { return airwayCompanyName; }
            set
            {
                if (value.Length ==1) airwayCompanyName = FirstLetterToUpper(value);
                else airwayCompanyName = value;
                if (AirwayCompanyName != "Enter airline company name" && !String.IsNullOrWhiteSpace(AirwayCompanyName)) AirwayCompFontColor = "Black";
                AirwayCompany.name = AirwayCompanyName;
                foreach (var c in FlightsViewModel.GetAllCompanies())
                {
                    if (c.name.Trim().Equals(AirwayCompanyName.Trim())) { AirwayCompany.ID_comp = c.ID_comp; AirwayCompany.name = c.name; break; }
                }
                OnPropertyChanged("AirwayCompanyName");
            }
        }
        private CompanyModel AirwayCompany { get; set; }
        private string plane;
        public string Plane
        {
            get { return plane.Trim(); }
            set
            {
                if (value.Length ==1) plane = FirstLetterToUpper(value);
                else plane = value;
                if (Plane != "Enter plane model name" && !String.IsNullOrWhiteSpace(Plane)) PlaneFontColor = "Black";
                OnPropertyChanged("Plane");
            }
        }
        private string depCity;
        public string DepCity
        {
            get { return depCity.Trim(); }
            set
            {
                if (value.Length ==1) depCity = FirstLetterToUpper(value);
                else depCity = value;
                if (DepCity != null)
                {
                    if (DepCity != "Enter city of departure" && !String.IsNullOrWhiteSpace(DepCity)) DepCityFontColor = "Black";
                    if (ArrCity != null) FindFlightTime();
                }
                OnPropertyChanged("DepCity");
            }
        }
        private string arrCity;
        public string ArrCity
        {
            get { return arrCity; }
            set
            {
                if (value.Length ==1) arrCity = FirstLetterToUpper(value);
                else arrCity = value;
                if (ArrCity != null)
                {
                    if (ArrCity != "Enter city of arrival" && !String.IsNullOrWhiteSpace(ArrCity)) ArrCityFontColor = "Black";
                    if (DepCity != null) FindFlightTime();
                }
                OnPropertyChanged("ArrCity");
            }
        }
        private string airwayCompFontColor;
        public string AirwayCompFontColor
        {
            get { return airwayCompFontColor; }
            set { airwayCompFontColor = value; OnPropertyChanged("AirwayCompFontColor"); }
        }
        private string planeFontColor;
        public string PlaneFontColor
        {
            get { return planeFontColor; }
            set { planeFontColor = value; OnPropertyChanged("PlaneFontColor"); }
        }
        private string depCityFontColor;
        public string DepCityFontColor
        {
            get { return depCityFontColor; }
            set { depCityFontColor = value; OnPropertyChanged("DepCityFontColor"); }
        }
        private string arrCityFontColor;
        public string ArrCityFontColor
        {
            get { return arrCityFontColor; }
            set { arrCityFontColor = value; OnPropertyChanged("ArrCityFontColor"); }
        }
        private string airwayCompanySymbolsLeft;
        public string AirwayCompanySymbolsLeft
        {
            get { return airwayCompanySymbolsLeft; }
            set { airwayCompanySymbolsLeft = value; OnPropertyChanged("AirwayCompanySymbolsLeft"); }
        }
        private string planeSymbolsLeft;
        public string PlaneSymbolsLeft
        {
            get { return planeSymbolsLeft; }
            set { planeSymbolsLeft = value; OnPropertyChanged("PlaneSymbolsLeft"); }
        }
        private string depCitySymbolsLeft;
        public string DepCitySymbolsLeft
        {
            get { return depCitySymbolsLeft; }
            set { depCitySymbolsLeft = value; OnPropertyChanged("DepCitySymbolsLeft"); }
        }
        private string arrCitySymbolsLeft;
        public string ArrCitySymbolsLeft
        {
            get { return arrCitySymbolsLeft; }
            set { arrCitySymbolsLeft = value; OnPropertyChanged("ArrCitySymbolsLeft"); }
        }
        
        #endregion

        #region Collections
        public ObservableCollection<string> Companies { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public List<string> Planes { get; set; }
        List<TripModel> Trips { get; set; }
        #endregion

        #region UIElementsVisibility

        private string arrTimeIsEnabled;
        public string ArrTimeIsEnabled
        {
            get { return arrTimeIsEnabled; }
            set { arrTimeIsEnabled = value; OnPropertyChanged("ArrTimeIsEnabled"); }
        }
        private string pSymbolsLeftVisibility;
        public string PSymbolsLeftVisibility
        {
            get { return pSymbolsLeftVisibility; }
            set { pSymbolsLeftVisibility = value; OnPropertyChanged("PSymbolsLeftVisibility"); }
        }
        private string aCSymbolsLeftVisibility;
        public string ACSymbolsLeftVisibility
        {
            get { return aCSymbolsLeftVisibility; }
            set { aCSymbolsLeftVisibility = value; OnPropertyChanged("ACSymbolsLeftVisibility"); }
        }
        private string dCSymbolsLeftVisibility;
        public string DCSymbolsLeftVisibility
        {
            get { return dCSymbolsLeftVisibility; }
            set { dCSymbolsLeftVisibility = value; OnPropertyChanged("DCSymbolsLeftVisibility"); }
        }
        private string arCSymbolsLeftVisibility;
        public string ArCSymbolsLeftVisibility
        {
            get { return arCSymbolsLeftVisibility; }
            set { arCSymbolsLeftVisibility = value; OnPropertyChanged("ArCSymbolsLeftVisibility"); }
        }
        #endregion

        #region Commands

        private RelayCommand airwayCompanyFieldGotFocusCommand;
        public RelayCommand AirwayCompanyFieldGotFocusCommand
        {
            get
            {
                return airwayCompanyFieldGotFocusCommand ?? (airwayCompanyFieldGotFocusCommand = new RelayCommand(obj =>
                {
                    if (AirwayCompanyName.Equals("Enter airline company name")) { AirwayCompanyName = ""; AirwayCompFontColor = "Black"; ACSymbolsLeftVisibility = "Visible"; }
                }));
            }
        }

        private RelayCommand airwayCompanyFieldLostFocusCommand;
        public RelayCommand AirwayCompanyFieldLostFocusCommand
        {
            get
            {
                return airwayCompanyFieldLostFocusCommand ?? (airwayCompanyFieldLostFocusCommand = new RelayCommand(obj =>
                {
                    if (String.IsNullOrWhiteSpace(AirwayCompanyName)) { AirwayCompanyName = "Enter airline company name"; AirwayCompFontColor = "LightGray"; ACSymbolsLeftVisibility = "Hidden"; AirwayCompanySymbolsLeft = "10"; }
                }));
            }
        }

        private RelayCommand airwayCompanyNameChangedCommand;
        public RelayCommand AirwayCompanyNameChangedCommand
        {
            get
            {
                return airwayCompanyNameChangedCommand ?? (airwayCompanyNameChangedCommand = new RelayCommand(obj =>
                {
                    AirwayCompanySymbolsLeft = (10 - AirwayCompanyName.Length).ToString();
                }));
            }
        }

        private RelayCommand planeFieldGotFocusCommand;
        public RelayCommand PlaneFieldGotFocusCommand
        {
            get
            {
                return planeFieldGotFocusCommand ?? (planeFieldGotFocusCommand = new RelayCommand(obj =>
                {
                    if (Plane.Equals("Enter plane model name")) { Plane = ""; PlaneFontColor = "Black"; PSymbolsLeftVisibility = "Visible"; }
                }));
            }
        }

        private RelayCommand planeFieldLostFocusCommand;
        public RelayCommand PlaneFieldLostFocusCommand
        {
            get
            {
                return planeFieldLostFocusCommand ?? (planeFieldLostFocusCommand = new RelayCommand(obj =>
                {
                    if (String.IsNullOrWhiteSpace(Plane)) { Plane = "Enter plane model name"; PlaneFontColor = "LightGray"; PSymbolsLeftVisibility = "Hidden"; PlaneSymbolsLeft = "10"; }
                }));
            }
        }

        private RelayCommand planeChangedCommand;
        public RelayCommand PlaneChangedCommand
        {
            get
            {
                return planeChangedCommand ?? (planeChangedCommand = new RelayCommand(obj =>
                {
                    PlaneSymbolsLeft = (10 - Plane.Length).ToString();
                }));
            }
        }

        private RelayCommand departureCityChangedCommand;
        public RelayCommand DepartureCityChangedCommand
        {
            get
            {
                return departureCityChangedCommand ?? (departureCityChangedCommand = new RelayCommand(obj =>
                {
                    DepCitySymbolsLeft = (25 - DepCity.Length).ToString();
                }));
            }
        }

        private RelayCommand depCityFieldGotFocusCommand;
        public RelayCommand DepCityFieldGotFocusCommand
        {
            get
            {
                return depCityFieldGotFocusCommand ?? (depCityFieldGotFocusCommand = new RelayCommand(obj =>
                {
                    if (DepCity.Equals("Enter city of departure")) { DepCity = ""; DepCityFontColor = "Black"; DCSymbolsLeftVisibility = "Visible"; }
                }));
            }
        }

        private RelayCommand depCityFieldLostFocusCommand;
        public RelayCommand DepCityFieldLostFocusCommand
        {
            get
            {
                return depCityFieldLostFocusCommand ?? (depCityFieldLostFocusCommand = new RelayCommand(obj =>
                {
                    if (String.IsNullOrWhiteSpace(DepCity)) { DepCity = "Enter city of departure"; DepCityFontColor = "LightGray"; DCSymbolsLeftVisibility = "Hidden"; DepCitySymbolsLeft = "25"; }
                }));
            }
        }

        private RelayCommand arrivalCityChangedCommand;
        public RelayCommand ArrivalCityChangedCommand
        {
            get
            {
                return arrivalCityChangedCommand ?? (arrivalCityChangedCommand = new RelayCommand(obj =>
                {
                    ArrCitySymbolsLeft = (25 - ArrCity.Length).ToString();
                }));
            }
        }

        private RelayCommand arrCityFieldGotFocusCommand;
        public RelayCommand ArrCityFieldGotFocusCommand
        {
            get
            {
                return arrCityFieldGotFocusCommand ?? (arrCityFieldGotFocusCommand = new RelayCommand(obj =>
                {
                    if (ArrCity.Equals("Enter city of arrival")) { ArrCity = ""; ArrCityFontColor = "Black"; ArCSymbolsLeftVisibility = "Visible"; }
                }));
            }
        }

        private RelayCommand arrCityFieldLostFocusCommand;
        public RelayCommand ArrCityFieldLostFocusCommand
        {
            get
            {
                return arrCityFieldLostFocusCommand ?? (arrCityFieldLostFocusCommand = new RelayCommand(obj =>
                {
                    if (String.IsNullOrWhiteSpace(ArrCity)) { ArrCity = "Enter city of arrival"; ArrCityFontColor = "LightGray"; ArCSymbolsLeftVisibility = "Hidden"; ArrCitySymbolsLeft = "25"; }
                }));
            }
        }

        private RelayCommand flightTimeChangedCommand;
        public RelayCommand FlightTimeChangedCommand
        {
            get
            {
                return flightTimeChangedCommand ?? (flightTimeChangedCommand = new RelayCommand(obj =>
                {
                    FindFlightTime();
                }));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(obj =>
                {
                    Save();
                }));
            }
        }

        #endregion

        public EditTripViewModel(TripModel trip)
        {
            InitializeCollections();
            GetCollectionsDataFromDB();
            SetPropertiesOnStart(trip);
        }

        private void InitializeCollections()
        {
            Companies = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Planes = new List<string>();
            Trips = new List<TripModel>();
        }

        private void GetCollectionsDataFromDB()
        {
            FlightsViewModel.GetAllCompanies().ForEach(c=>Companies.Add(c.name));
            FlightsViewModel.GetAllCities().ForEach(c => Cities.Add(c));
            FlightsViewModel.GetExistingPlanes().ForEach(p => Planes.Add(p));
            FlightsViewModel.GetTrips().ForEach(f => Trips.Add(f));
        }

        private void SetPropertiesOnStart(TripModel trip)
        {
            Trip = trip;
            TripNo = trip.TripNumber;
            AirwayCompanyName = trip.AirwayCompany.name;
            Plane = Trip.Plane;
            DepCity = Trip.TownFrom;
            ArrCity = Trip.TownTo;
            DepTime = trip.DepTime;
            ArrTime = trip.ArrTime;
            ArrTimeIsEnabled = "False";
            ACSymbolsLeftVisibility = "Hidden";
            PSymbolsLeftVisibility = "Hidden";
            DCSymbolsLeftVisibility = "Hidden";
            ArCSymbolsLeftVisibility = "Hidden";
        }

        private bool CheckDepAndArrCities()
        {
            if (DepCity != null && ArrCity != null)
            {
                if (DepCity.Equals(ArrCity)) { MessageBox.Show("City of departure can't coinside with city of arrival."); ArrCity = null; return false; }
                else return true;
            }
            else return false;
        }
        private void Save()
        {
            if (CheckDepAndArrCities())
            {
                FlightsViewModel.ChangedTrip=new TripModel(Trip.TripNumber, null, AirwayCompany, Plane, DepCity, ArrCity, DepTime, ArrTime, null, null);
                FlightsViewModel.SaveChangedTrip(FlightsViewModel.ChangedTrip);
            }
        }
        private bool FindFlightTime()
        {
            foreach (var t in Trips)
            {
                if ((DepCity == t.TownFrom && ArrCity == t.TownTo) || (DepCity == t.TownTo && ArrCity == t.TownFrom))
                {
                    if (t.ArrTime < t.DepTime) FlightTime = t.ArrTime - t.DepTime + TimeSpan.FromHours(12);
                    else FlightTime = t.ArrTime - t.DepTime;
                    FlightTimeIsEnabled = "False"; return true;
                }
                else { FlightTimeIsEnabled = "True"; FlightTime = TimeSpan.FromDays(0); }
            }
            return false;
        }
        private string FirstLetterToUpper(string s)
        {
            if (string.IsNullOrWhiteSpace(s) || string.IsNullOrEmpty(s)) return string.Empty;
            else return char.ToUpper(s[0]) + s.Substring(1);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
