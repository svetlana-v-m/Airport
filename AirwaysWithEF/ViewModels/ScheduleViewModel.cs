using AirwaysWithEF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AirwaysWithEF.ViewModels;
using AirwaysWithEF.Models;
using System.Windows.Forms;

namespace AirwaysWithEF
{
    class ScheduleViewModel:INotifyPropertyChanged
    {
        #region Properties

        #region Collections for UI
        public ObservableCollection<TripModel> Departures { get; set; }
        public ObservableCollection<TripModel> Arrivals { get; set; }
        public ObservableCollection<string> CitiesList { get; set; }
        public ObservableCollection<string> CompaniesList { get; set; }
        #endregion

        #region Selections
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set {
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
            }
        }
        private string selectedCompany;
        public string SelectedCompany
        {
            get { return selectedCompany; }
            set { selectedCompany = value;
                  OnPropertyChanged("SelectedCompany"); }
        }
        private TripModel selectedFlight;
        public TripModel SelectedFlight
        {   get { return selectedFlight; }
            set { selectedFlight = value;if (value != null) { if (value.DepOrArrFlag.Equals("departure")) SelectedDepFlight = value; else SelectedArrFlight = value; } SelectedPassenger = null; OnPropertyChanged("SelectedFlight");}}
        private TripModel selectedDepFlight;
        public TripModel SelectedDepFlight
        {
            get { return selectedDepFlight; }
            set
            {
                selectedDepFlight = value;
                if (selectedDepFlight != null)
                {
                    DepFlightDetailsVisibility = "Visible";
                    ShowFlightDetails("departure");
                }
                OnPropertyChanged("SelectedDepFlight");
            }
        }
        private TripModel selectedArrFlight;
        public TripModel SelectedArrFlight
        {
            get { return selectedArrFlight; }
            set
            {
                selectedArrFlight = value;
                if (selectedArrFlight != null) ArrFlightDetailsVisibility = "Visible";ShowFlightDetails("arrival");
                OnPropertyChanged("SelectedArrFlight");
            }
        }
        public PassengersInFlightModel SelectedPassenger { get; set; }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value;
                if (!value.Equals("Поиск...")) SearchInFlights();
                OnPropertyChanged("SearchText"); }
        }
        private List<TripModel> DepSearchExcludedItems { get; set; }
        private List<TripModel> ArrSearchExcludedItems { get; set; }
        private List<TripModel> DepSearchResultItems { get; set; }
        private List<TripModel> ArrSearchResultItems { get; set; }
        #endregion

        #region UIElementsVisibility
        private string depFlightDetailsVisibility;
        public string DepFlightDetailsVisibility
        {
            get { return depFlightDetailsVisibility; }
            set { depFlightDetailsVisibility = value; OnPropertyChanged("DepFlightDetailsVisibility"); }
        }
        private string arrFlightDetailsVisibility;
        public string ArrFlightDetailsVisibility
        {
            get { return arrFlightDetailsVisibility; }
            set { arrFlightDetailsVisibility = value; OnPropertyChanged("ArrFlightDetailsVisibility"); }
        }
        private string selectedFiltersVisibility;
        public string SelectedFiltersVisibility
        {
            get { return selectedFiltersVisibility; }
            set {
                selectedFiltersVisibility = value;
                 OnPropertyChanged("SelectedFiltersVisibility"); }
        }
        #endregion

        #region LabelsContent
        private string departureFlightDetails;
        public string DepartureFlightDetails
        {
            get {return departureFlightDetails;}
            set { departureFlightDetails = value;OnPropertyChanged("DepartureFlightDetails"); }
        }
        private string arrivalFlightDetails;
        public string ArrivalFlightDetails
        {
            get { return arrivalFlightDetails; }
            set { arrivalFlightDetails = value; OnPropertyChanged("ArrivalFlightDetails"); }
        }
        private string selectedFiltersDep;
        public string SelectedFiltersDep
        {
            get { return selectedFiltersDep; }
            set
            {
                selectedFiltersDep = value;
                OnPropertyChanged("SelectedFiltersDep");
            }
        }
        private string selectedFiltersArr;
        public string SelectedFiltersArr
        {
            get { return selectedFiltersArr; }
            set
            {
                selectedFiltersArr = value;
                OnPropertyChanged("SelectedFiltersArr");
            }
        }
        #endregion

        #endregion

        #region Commands

        private RelayCommand selectTripsCommand;
        public RelayCommand SelectTripsCommand
        {
            get
            {
                return selectTripsCommand ??
                    (selectTripsCommand = new RelayCommand(obj =>
                    {
                        ClearDeparturesArrivalsTable();
                        SelectTrips();
                    }));
            }
        }
        
        private RelayCommand showFlightDetailsCommand;
        public RelayCommand ShowFlightDetailsCommand
        {
            get
            {
                return showFlightDetailsCommand ?? (
                    showFlightDetailsCommand = new RelayCommand(obj =>
                    {
                        if(SelectedCity!="Not selected")
                        {
                            string par = obj as string;
                            ShowFlightDetails(par);
                        }
                    }));
            }
        }

        private RelayCommand addPassengerCommand;
        public RelayCommand AddPassengerCommand
        {
            get {
                return addPassengerCommand ?? (addPassengerCommand = new RelayCommand(obj =>
              {
                  AddPassenger();
              },(obj)=>SelectedFlight!=null));
            }
        }

        private RelayCommand deletePassengerCommand;
        public RelayCommand DeletePassengerCommand
        {
            get
            {
                return deletePassengerCommand ?? (deletePassengerCommand = new RelayCommand(obj =>
                    {
                        DeletePassenger();
                    },(obj)=>SelectedPassenger!=null && SelectedFlight != null));
            }
        }

        private RelayCommand editPassengerDataCommand;
        public RelayCommand EditPassengerDataCommand
        {
            get {
                return editPassengerDataCommand ?? (editPassengerDataCommand = new RelayCommand(obj =>
              {
                  EditPassengerData();
              },(obj)=> SelectedPassenger != null&&SelectedFlight!=null));
            }
        }

        private RelayCommand addFlightCommand;
        public RelayCommand AddFlightCommand
        {
            get
            {
                return addFlightCommand ?? (addFlightCommand = new RelayCommand(obj =>
                    {
                        AddNewFlight();
                    }));
            }
        }

        private RelayCommand editFlightCommand;
        public RelayCommand EditFlightCommand
        {
            get
            {
                return editFlightCommand ?? (editFlightCommand = new RelayCommand(obj =>
                    {
                        EditFlight();
                    },(obj)=>SelectedFlight!=null));
            }
        }

        private RelayCommand deleteFlightCommand;
        public RelayCommand DeleteFlightCommand
        {
            get
            {
                return deleteFlightCommand ?? (deleteFlightCommand = new RelayCommand(obj =>
                    {
                        DeleteFlight();
                    },(obj)=>SelectedFlight!=null));
            }
        }

        private RelayCommand searchTextBoxGotFocusCommand;
        public RelayCommand SearchTextBoxGotFocusCommand
        {
            get
            {
                return searchTextBoxGotFocusCommand ??
                (searchTextBoxGotFocusCommand = new RelayCommand(obj =>
                {
                    if (SearchText.Equals("Поиск..."))
                        SearchText = "";
                    DepFlightDetailsVisibility = "Hidden";
                    ArrFlightDetailsVisibility = "Hidden";
                }));
            }
        }

        private RelayCommand searchTextBoxLostFocusCommand;
        public RelayCommand SearchTextBoxLostFocusCommand
        {
            get
            {
                return searchTextBoxLostFocusCommand ??
                (searchTextBoxLostFocusCommand = new RelayCommand(obj =>
                {
                    if (string.IsNullOrWhiteSpace(SearchText))
                    {
                        SearchText = "Поиск...";
                        DepSearchExcludedItems.ForEach(t => Departures.Add(t));
                        ArrSearchExcludedItems.ForEach(t => Arrivals.Add(t));
                    }

                }));
            }
        }

        #endregion

        public ScheduleViewModel()
        {
            InitializeCollections();
            InitializeUIComponents();
            GetUIDataFromDB();
        }

        private void InitializeCollections()
        {
            Departures = new ObservableCollection<TripModel>();
            Arrivals = new ObservableCollection<TripModel>();
            CompaniesList = new ObservableCollection<string>();
            CitiesList = new ObservableCollection<string>();
            DepSearchResultItems = new List<TripModel>();
            DepSearchExcludedItems = new List<TripModel>();
            ArrSearchResultItems = new List<TripModel>();
            ArrSearchExcludedItems = new List<TripModel>();
        }

        private void InitializeUIComponents()
        {
            DepFlightDetailsVisibility = "Hidden";
            ArrFlightDetailsVisibility = "Hidden";
            SearchText = "Поиск...";
        }

        private void GetUIDataFromDB()
        {
            FlightsViewModel.GetAllCities().ForEach(c => CitiesList.Add(c));
            CitiesList.Insert(0, "Not selected");
            SelectedCity = CitiesList[0];
            FlightsViewModel.GetAllCompanies().ForEach(c=>CompaniesList.Add(c.name));
            CompaniesList.Insert(0, "Not selected");
            SelectedCompany = CompaniesList[0];
            SelectTrips();
        }

        private void SelectTrips()
        {
            List<TripModel> depList = new List<TripModel>();
            List<TripModel> arrsList = new List<TripModel>();
            if (SelectedCity!="Not selected"&&SelectedCompany!="Not selected")
            {
                FlightsViewModel.GetFlights(SelectedCity, SelectedCompany, ref depList, ref arrsList);
                foreach (var t in depList) Departures.Add(t);
                if (Departures.Count == 1) { SelectedFiltersDep = "Found " + Departures.Count + " flight " + " from " + SelectedCity + " by " + SelectedCompany + "."; }
                else { SelectedFiltersDep = "Found " + Departures.Count + " flights " + " from " + SelectedCity + " by " + SelectedCompany + "."; }
                foreach (var t in arrsList) Arrivals.Add(t);
                if(Arrivals.Count==1) SelectedFiltersArr = "Found " + Arrivals.Count + " flight " + " to " + SelectedCity + " by " + SelectedCompany + ".";
                else SelectedFiltersArr = "Found " + Arrivals.Count + " flights " + " to " + SelectedCity + " by " + SelectedCompany + ".";
                SelectedFiltersVisibility = "Visible";
            }
            else if(SelectedCity=="Not selected"&&SelectedCompany!="Not selected")
            {
                
                FlightsViewModel.GetFlights(null, SelectedCompany, ref depList, ref arrsList);
                foreach (var t in depList) Departures.Add(t);
                if(Departures.Count==1) SelectedFiltersDep = "Found " + Departures.Count + " departure " + " by " + SelectedCompany + ".";
                else SelectedFiltersDep = "Found " + Departures.Count + " departures " + " by " + SelectedCompany + ".";
                foreach (var t in arrsList) Arrivals.Add(t);
                if(Arrivals.Count==1) SelectedFiltersArr = "Found " + Arrivals.Count + " arrival " + " by " + SelectedCompany + ".";
                else SelectedFiltersArr = "Found " + Arrivals.Count + " arrivals " + " by " + SelectedCompany + ".";
                SelectedFiltersVisibility = "Visible";
            }
            else if(SelectedCity != "Not selected" && SelectedCompany == "Not selected")
            {
                FlightsViewModel.GetFlights(SelectedCity, null, ref depList, ref arrsList);
                foreach (var t in depList) Departures.Add(t);
                if (Departures.Count == 1) { SelectedFiltersDep = "Found " + Departures.Count + " flight " + " from " + SelectedCity + "."; }
                else { SelectedFiltersDep = "Found " + Departures.Count + " flights " + " from " + SelectedCity + "."; }
                foreach (var t in arrsList) Arrivals.Add(t);
                
                SelectedFiltersArr = "Found " + Arrivals.Count + " flights " + " to " + SelectedCity + ".";
                SelectedFiltersVisibility = "Visible";
            }
            else 
            {
                FlightsViewModel.GetFlights(null, null, ref depList, ref arrsList);
                foreach (var t in depList) Departures.Add(t);
                foreach (var t in arrsList) Arrivals.Add(t);
                SelectedFiltersVisibility = "Hidden";
            }
            
        }

        private void ClearDeparturesArrivalsTable()
        {
            if (SelectedDepFlight != null) SelectedDepFlight = null;
            if (SelectedArrFlight != null) SelectedArrFlight = null;
            DepFlightDetailsVisibility = "Hidden";
            DepartureFlightDetails = null;
            ArrFlightDetailsVisibility = "Hidden";
            ArrivalFlightDetails = null;
            Departures.Clear();
            Arrivals.Clear();
        }

        private void AddNewFlight()
        {
            TripModel newFlight = FlightsViewModel.CreateNewFlight();
            if(newFlight!=null)
            {
                if (newFlight.DepOrArrFlag.Equals("departure"))
                {
                    Departures.Add(newFlight);
                    SelectedFlight = Departures.Last();
                }
                else if (newFlight.DepOrArrFlag.Equals("arrival"))
                {
                    Arrivals.Add(newFlight);
                    SelectedFlight = Arrivals.Last();
                }
            }
        }

        private void EditFlight()
        {
            TripModel changedFlight = FlightsViewModel.EditFlight(SelectedFlight);
            if (changedFlight != null)
            {
                if (changedFlight.DepOrArrFlag.Equals("departure"))
                {
                    int n = Departures.IndexOf(SelectedFlight);
                    Departures.Remove(SelectedDepFlight);
                    Departures.Insert(n,changedFlight);
                    SelectedFlight = Departures[n];
                }
                else
                {
                    int n = Arrivals.IndexOf(SelectedFlight);
                    Arrivals[n] = new TripModel(changedFlight);
                    SelectedFlight = Arrivals[n];
                }
            }
        }

        private void DeleteFlight()
        {
            if(SelectedFlight!=null)
            {
                    string message = "Delete flight" + SelectedFlight.TripNumber + " " + SelectedFlight.TownFrom + "-" + SelectedFlight.TownTo + " on " + SelectedFlight.Date + " without possibility of recovery?";
                    string caption = "Delete Flight";
                    DialogResult result= MessageBox.Show(message, caption, MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (result == DialogResult.Yes&&SelectedFlight!=null&&SelectedFlight.DepOrArrFlag.Equals("departure"))
                    {
                        FlightsViewModel.DeleteFlight(SelectedFlight);
                        Departures.Remove(SelectedFlight);
                        SelectedDepFlight = null;
                        DepFlightDetailsVisibility = "Hidden";
                        ShowFlightDetails("departure");
                    }
                if (result == DialogResult.Yes &&SelectedFlight!=null&&SelectedFlight.DepOrArrFlag.Equals("arrival"))
                {
                    FlightsViewModel.DeleteFlight(SelectedFlight);
                    Arrivals.Remove(SelectedFlight);
                    SelectedArrFlight = null;
                    ArrFlightDetailsVisibility = "Hidden";
                    ShowFlightDetails("arrival");
                }
            }
        }

        private void ShowFlightDetails(string parameter)
        {
            if(parameter.Equals("departure"))
            {
                if(SelectedDepFlight!=null)
                {
                    DepartureFlightDetails = "Flight " + SelectedDepFlight.TripNumber + " on " + SelectedDepFlight.Date + " " + SelectedDepFlight.TownFrom.Trim() + " - " + SelectedDepFlight.TownTo.Trim() + ":" + SelectedDepFlight.PassengersList.Count + " passenger(s)";
                }
            }
            else if(parameter.Equals("arrival"))
            {
                if(SelectedArrFlight!=null)
                {
                   ArrivalFlightDetails = "Flight " + SelectedArrFlight.TripNumber + " on " + SelectedArrFlight.Date + " " + SelectedArrFlight.TownFrom.Trim() + " - " + SelectedArrFlight.TownTo.Trim() + ":" + SelectedArrFlight.PassengersList.Count + " passenger(s)";
                }
            }
        }

        private void AddPassenger()
        {
                PassengersInFlightModel NewPassenger = PassengersViewModel.AddNewPassenger(SelectedFlight, " ");
                if(NewPassenger!=null)
                {
                    if(SelectedFlight.DepOrArrFlag.Equals("departure"))
                    {
                        int n = Departures.IndexOf(SelectedFlight);
                        Departures[n].PassengersList.Add(NewPassenger);
                        ShowFlightDetails("departure");
                    }
                    else
                    {
                        int n = Arrivals.IndexOf(SelectedFlight);
                        Arrivals[n].PassengersList.Add(NewPassenger);
                        ShowFlightDetails("arrival");
                    }
                }
        }

        private void DeletePassenger()
        {
            if(SelectedPassenger!=null)
            {
                PassengersViewModel.DeletePassenger(SelectedPassenger, SelectedFlight);
                if(SelectedFlight.DepOrArrFlag.Equals("departure"))
                {
                    int n = Departures.IndexOf(SelectedFlight);
                    Departures[n].PassengersList.Remove(SelectedPassenger);
                    ShowFlightDetails("departure");
                }
                else
                {
                    int n = Arrivals.IndexOf(SelectedFlight);
                    Arrivals[n].PassengersList.Remove(SelectedPassenger);
                    ShowFlightDetails("arrival");
                }
            }
        }

        private void EditPassengerData()
        {
            PassengersInFlightModel newData = PassengersViewModel.EditPassengerData(SelectedPassenger, SelectedFlight, "");
            if (newData != null)
            {
                newData.Id = SelectedPassenger.Id;
                newData.Date = SelectedPassenger.Date;
                if (SelectedFlight.DepOrArrFlag.Equals("departure"))
                {
                    int n = Departures.IndexOf(SelectedFlight);
                    int m = Departures[n].PassengersList.IndexOf(SelectedPassenger);
                    Departures[n].PassengersList.RemoveAt(m);
                    Departures[n].PassengersList.Insert(m, newData);
                }
                else
                {
                    int n = Arrivals.IndexOf(SelectedFlight);
                    int m = Arrivals[n].PassengersList.IndexOf(SelectedPassenger);
                    Arrivals[n].PassengersList.RemoveAt(m);
                    Arrivals[n].PassengersList.Insert(m, newData);
                }
            }
        }

        private void SelectedFiltersLabel()
        {
            if (SelectedCity != "Not selected" && SelectedCompany == "Not selected") ;

            else if (SelectedCity != "Not selected" && SelectedCompany != "Not selected")
                SelectedFiltersDep = "From " + SelectedCity + " , by " + SelectedCompany;
            else if (SelectedCity == "Not selected" && SelectedCompany != "Not selected")
                SelectedFiltersDep = "By " + SelectedCompany;
            else SelectedFiltersDep = string.Empty;
        }

        private void SearchInFlights()
        {
                DepSearchExcludedItems.ForEach(d => { if (!Departures.Contains(d)) Departures.Add(d); });
                ArrSearchExcludedItems.ForEach(d => { if (!Arrivals.Contains(d)) Arrivals.Add(d); });

                DepSearchResultItems.Clear();
                ArrSearchResultItems.Clear();
                DepSearchExcludedItems.Clear();
                ArrSearchExcludedItems.Clear();

                DepSearchResultItems = Departures.Where(d =>
                d.AirwayCompany.name.ToUpper().Contains(SearchText.ToUpper()) ||
                d.AirwayCompany.ID_comp.ToString().Contains(SearchText) ||
                d.DepTimeString.Contains(SearchText) ||
                d.ArrTimeString.Contains(SearchText) ||
                d.Date.Contains(SearchText) ||
                d.Plane.ToUpper().Contains(SearchText.ToUpper()) ||
                d.TownFrom.ToUpper().Contains(SearchText.ToUpper()) ||
                d.TownTo.ToUpper().Contains(SearchText.ToUpper()) ||
                d.TripNumber.ToString().Contains(SearchText)
                ).ToList();

                ArrSearchResultItems = Arrivals.Where(d =>
                d.AirwayCompany.name.ToUpper().Contains(SearchText.ToUpper()) ||
                d.AirwayCompany.ID_comp.ToString().Contains(SearchText) ||
                d.DepTimeString.Contains(SearchText) ||
                d.ArrTimeString.Contains(SearchText) ||
                d.Date.Contains(SearchText) ||
                d.Plane.ToUpper().Contains(SearchText.ToUpper()) ||
                d.TownFrom.ToUpper().Contains(SearchText.ToUpper()) ||
                d.TownTo.ToUpper().Contains(SearchText.ToUpper()) ||
                d.TripNumber.ToString().Contains(SearchText)
                ).ToList();

                DepSearchExcludedItems = Departures.Where(d =>
                !d.AirwayCompany.name.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.AirwayCompany.ID_comp.ToString().Contains(SearchText) ||
                !d.DepTimeString.Contains(SearchText) ||
                !d.ArrTimeString.Contains(SearchText) ||
                !d.Date.Contains(SearchText) ||
                !d.Plane.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.TownFrom.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.TownTo.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.TripNumber.ToString().Contains(SearchText)).ToList();
                Departures.Clear();

                ArrSearchExcludedItems = Arrivals.Where(d =>
                !d.AirwayCompany.name.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.AirwayCompany.ID_comp.ToString().Contains(SearchText) ||
                !d.DepTimeString.Contains(SearchText) ||
                !d.ArrTimeString.Contains(SearchText) ||
                !d.Date.Contains(SearchText) ||
                !d.Plane.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.TownFrom.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.TownTo.ToUpper().Contains(SearchText.ToUpper()) ||
                !d.TripNumber.ToString().Contains(SearchText)).ToList();
                Arrivals.Clear();

                DepSearchResultItems.ForEach(d => Departures.Add(d));
                ArrSearchResultItems.ForEach(a => Arrivals.Add(a));
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
