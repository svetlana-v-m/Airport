using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AirwaysWithEF.Models;
    
namespace AirwaysWithEF.ViewModels
{
    public class EditFlightViewModel: INotifyPropertyChanged
    {
        #region Properties
        public static TripModel OldTrip { get; set; }

        #region Collections
        public ObservableCollection<TripModel> Trips { get; set; }
        public ObservableCollection<PassengersInFlightModel> Passengers { get; set; }
        #endregion

        #region Selections
        private TripModel selectedTrip;
        public TripModel SelectedTrip
        {
            get { return selectedTrip; }
            set
            {
                selectedTrip = value;
                if (SelectedTrip != null) selectedTripLabel = "Selected trip: " + SelectedTrip.TripNumber + " " + SelectedTrip.TownFrom + "-" + SelectedTrip.TownTo + " " + SelectedTrip.DepTime + "-" + SelectedTrip.ArrTime;
                else selectedTripLabel = "Trip is notselected.";
                OnPropertyChanged("SelectedTrip");
            }
        }
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value; OnPropertyChanged("SelectedDate"); }
        }
        private string selectedTripLabel;
        public string SelectedTripLabel
        {
            get { return selectedTripLabel; }
            private set
            {
                selectedTripLabel = value;
                OnPropertyChanged("SelectedTripLabel");
            }
        }
        public PassengersInFlightModel SelectedPassenger { get; set; }
        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                if (!value.Equals("Поиск...")) SearchInTrips();
                OnPropertyChanged("SearchText");
            }
        }
        private List<TripModel> SearchExcludedItems { get; set; }
        private List<TripModel> SearchResultItems { get; set; }
        #endregion

        #region UI ElementsVisibility
        private string setNewFlightDataIsEnabled;
        public string SetNewFlightDataIsEnabled
        {
            get { return setNewFlightDataIsEnabled; }
            private set
            {
                if (SelectedTrip != null) setNewFlightDataIsEnabled = "True";
                else setNewFlightDataIsEnabled = "False";
                OnPropertyChanged("SetNewFlightDataIsEnabled");
            }
        }
        #endregion
        #endregion

        #region Commands
        
        private RelayCommand createNewTripCommand;
        public RelayCommand CreateNewTripCommand
        {
            get
            {
                return createNewTripCommand ?? (createNewTripCommand = new RelayCommand(obj =>
                {
                    CreateNewTrip();
                }));
            }
        }

        private RelayCommand addPassengerCommand;
        public RelayCommand AddPassengerCommand
        {
            get
            {
                return addPassengerCommand ?? (addPassengerCommand = new RelayCommand(obj =>
                {
                    string param = obj as string;
                    AddPassenger();
                }, (obj) => SelectedTrip != null));
            }
        }

        private RelayCommand deletePassengerCommand;
        public RelayCommand DeletePassengerCommand
        {
            get
            {
                return deletePassengerCommand ?? (deletePassengerCommand = new RelayCommand(obj =>
                {
                    string param = obj as string;
                    DeletePassenger();
                }, (obj) => SelectedPassenger != null));
            }
        }

        private RelayCommand editPassengerDataCommand;
        public RelayCommand EditPassengerDataCommand
        {
            get
            {
                return editPassengerDataCommand ?? (editPassengerDataCommand = new RelayCommand(obj =>
                {
                    string param = obj as string;
                    EditPassengerData();
                }, (obj) => SelectedPassenger != null));
            }
        }

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(obj =>
                {
                    SaveFlight();
                }, (obj) => SelectedTrip != null && SelectedTrip.PassengersList.Count > 0 && SelectedDate != null));
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
                        SearchExcludedItems.ForEach(t => Trips.Add(t));
                    }

                }));
            }
        }
        #endregion

        public EditFlightViewModel(TripModel trip)
        {
            OldTrip = trip;
            SelectedTrip = OldTrip;
            InitializeCollections();
            FillInCollections();
            SelectedDate = DateTime.Parse(SelectedTrip.Date);
            SearchText = "Поиск...";
        }

        private void InitializeCollections()
        {
            Trips = new ObservableCollection<TripModel>();
            Passengers = new ObservableCollection<PassengersInFlightModel>();
            SearchResultItems = new List<TripModel>();
            SearchExcludedItems = new List<TripModel>();
        }

        private void FillInCollections()
        {
            FlightsViewModel.GetTrips().ForEach(t => Trips.Add(t));
            Passengers = OldTrip.PassengersList;
        }

        private void CreateNewTrip()
        {
            List<TripModel> newTrip = FlightsViewModel.CreateNewTrip();
            if (newTrip != null)
            {
                if (newTrip.Count == 1)
                {
                    Trips.Add(newTrip[0]);
                    SelectedTrip = Trips.Last();
                }
                else if (newTrip.Count == 2)
                {
                    newTrip.ForEach(t => Trips.Add(t));
                    SelectedTrip = Trips[Trips.IndexOf(Trips.Last()) - 1];
                }
            }
        }

        private void AddPassenger()
        {
            PassengersInFlightModel NewPassenger = PassengersViewModel.AddNewPassenger(SelectedTrip, "newFlight");
            if (NewPassenger != null)
            {
                int n = Trips.IndexOf(SelectedTrip);
                Trips[n].PassengersList.Add(NewPassenger);
            }
        }

        private void DeletePassenger()
        {
            if (SelectedPassenger != null)
            {
                SelectedTrip.PassengersList.Remove(SelectedPassenger);
            }
            else MessageBox.Show("Choose passenger!");
        }

        private void EditPassengerData()
        {
            if (SelectedPassenger != null)
            {
                PassengersInFlightModel newData = PassengersViewModel.EditPassengerData(SelectedPassenger, SelectedTrip, "new flight");
                newData.Id = SelectedPassenger.Id;
                newData.Date = SelectedPassenger.Date;
                int n = Trips.IndexOf(SelectedTrip);
                int m = Trips[n].PassengersList.IndexOf(SelectedPassenger);
                Trips[n].PassengersList.RemoveAt(m);
                Trips[n].PassengersList.Insert(m, newData);
            }
        }

        private void SaveFlight()
        {
            if (Passengers.Count != 0)
            {
                ObservableCollection<PassengersInFlightModel> passList = new ObservableCollection<PassengersInFlightModel>();
                foreach (var p in Passengers)
                {
                    passList.Add(new PassengersInFlightModel { Id = p.Id, Date = SelectedDate.ToShortDateString(), PassengerName = p.PassengerName, SeatNumber = p.SeatNumber });
                }
               
                FlightsViewModel.ChangedFlight = new TripModel
                    (SelectedTrip.TripNumber, 
                    SelectedDate.ToShortDateString(), 
                    SelectedTrip.AirwayCompany, 
                    SelectedTrip.Plane, 
                    SelectedTrip.TownFrom, 
                    SelectedTrip.TownTo, 
                    SelectedTrip.DepTime, 
                    SelectedTrip.ArrTime, 
                    null,
                    passList);
            }
            else if (SelectedDate == null) MessageBox.Show("Date for trip is not selected!");
            else if (Passengers.Count == 0) MessageBox.Show("Passengers list is empty! Please,add passengers.");
            else MessageBox.Show("Check entered data! Something is wrong.");
        }

        private void SearchInTrips()
        {
            SearchExcludedItems.ForEach(t => { if (!Trips.Contains(t)) Trips.Add(t); });

            SearchResultItems.Clear();
            SearchExcludedItems.Clear();

            SearchResultItems = Trips.Where(t =>
            t.AirwayCompany.name.ToUpper().Contains(SearchText.ToUpper()) ||
            t.AirwayCompany.ID_comp.ToString().Contains(SearchText) ||
            t.DepTimeString.Contains(SearchText) ||
            t.ArrTimeString.Contains(SearchText) ||
            t.Plane.ToUpper().Contains(SearchText.ToUpper()) ||
            t.TownFrom.ToUpper().Contains(SearchText.ToUpper()) ||
            t.TownTo.ToUpper().Contains(SearchText.ToUpper()) ||
            t.TripNumber.ToString().Contains(SearchText)
            ).ToList();

            SearchExcludedItems = Trips.Where(t =>
            !t.AirwayCompany.name.ToUpper().Contains(SearchText.ToUpper()) ||
            !t.AirwayCompany.ID_comp.ToString().Contains(SearchText) ||
            !t.DepTimeString.Contains(SearchText) ||
            !t.ArrTimeString.Contains(SearchText) ||
            !t.Plane.ToUpper().Contains(SearchText.ToUpper()) ||
            !t.TownFrom.ToUpper().Contains(SearchText.ToUpper()) ||
            !t.TownTo.ToUpper().Contains(SearchText.ToUpper()) ||
            !t.TripNumber.ToString().Contains(SearchText)).ToList();
            Trips.Clear();

            SearchResultItems.ForEach(t => Trips.Add(t));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
