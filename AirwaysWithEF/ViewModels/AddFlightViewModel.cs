using AirwaysWithEF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirwaysWithEF.ViewModels
{
    class AddFlightViewModel : INotifyPropertyChanged
    {
        #region Properties
        #region Collections
        public ObservableCollection<TripModel> Trips { get; set; } 
        public ObservableCollection<PassengersInFlightModel> Passengers { get; set; }
        #endregion

        #region Selections

        private TripModel selectedTrip;
        public TripModel SelectedTrip
        {
            get { return selectedTrip; }
            set { selectedTrip = value;
                if (SelectedTrip != null) SelectedTripLabel = "Selected trip: " + SelectedTrip.TripNumber + " " + SelectedTrip.TownFrom + "-" + SelectedTrip.TownTo + " " + SelectedTrip.DepTime + "-" + SelectedTrip.ArrTime;
                else SelectedTripLabel = "Trip is notselected.";
                OnPropertyChanged("SelectedTrip"); }
        }
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value;OnPropertyChanged("SelectedDate"); }
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

        private RelayCommand editTripCommand;
        public RelayCommand EditTripCommand
        {
            get {
                return editTripCommand ?? (editTripCommand = new RelayCommand(obj =>
              {
                  EditTrip();
              }));
            }
        }

        private RelayCommand deleteTripCommand;
        public RelayCommand DeleteTripCommand
        {
            get {
                return deleteTripCommand ?? (deleteTripCommand = new RelayCommand(obj =>
              {
                  DeleteTrip();
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
                }, (obj)=>SelectedPassenger != null));
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
                    },(obj)=> SelectedTrip != null && Passengers.Count>0 && SelectedDate != null));
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

        public AddFlightViewModel()
        {
            InitializeCollections();
            FillInTripCollection();
            SelectedDate = DateTime.Now;
            SearchText = "Поиск...";
        }

        private void InitializeCollections()
        {
            Trips = new ObservableCollection<TripModel>();
            Passengers = new ObservableCollection<PassengersInFlightModel>();
            SearchResultItems = new List<TripModel>();
            SearchExcludedItems = new List<TripModel>();
        }

        private void FillInTripCollection()
        {
            FlightsViewModel.GetTrips().ForEach(t => Trips.Add(t));
            for (int i = 0; i < Trips.Count; i++)
            {
                for (int j = 0; j < Trips.Count; j++)
                {
                    if (
                        Trips[i].TownFrom.Equals(Trips[j].TownTo) &&
                        Trips[i].TownTo.Equals(Trips[j].TownFrom) &&
                        Trips[i].TripNumber == Trips[j].TripNumber - 1)
                    {
                        Trips[i].ReturnTripBool = true;
                        Trips[i].ReturnTripN = Trips[j].TripNumber;
                        Trips[j].ReturnTripBool = true;
                        Trips[j].ReturnTripN = Trips[i].TripNumber;
                        Trips[i].ReturnTrip = Trips[j];
                        Trips[j].ReturnTrip = Trips[i];
                    }
                }
            }
        }

        private void CreateNewTrip()
        {
            List<TripModel> newTrip = FlightsViewModel.CreateNewTrip();
            if(newTrip!=null)
            {
                if (newTrip.Count == 1)
                {
                    Trips.Add(newTrip[0]);
                    SelectedTrip = Trips.Last();
                }
                else if(newTrip.Count==2)
                {
                    newTrip.ForEach(t => Trips.Add(t));
                    SelectedTrip = Trips[Trips.IndexOf(Trips.Last()) - 1];
                }
            }
        }

        private void EditTrip()
        {
            TripModel changedTrip = FlightsViewModel.EditTrip(SelectedTrip);
            if (changedTrip!=null&& SelectedTrip.ReturnTripBool)
            {
                foreach (var t in Trips)
                {
                    if (t.TripNumber.Equals(SelectedTrip.ReturnTripN))
                    {
                        FlightsViewModel.EditTrip(t, new TripModel(t) { AirwayCompany = changedTrip.AirwayCompany, Plane = changedTrip.Plane, TownFrom = changedTrip.TownTo, TownTo = changedTrip.TownFrom });
                         break;
                    }
                }
            }
            Trips.Clear();
            FillInTripCollection();
        }

        private void DeleteTrip()
        {
            if (SelectedTrip != null)
            {
                List<TripModel> flights = new List<TripModel>(FlightsViewModel.GetFlightsByTripNumber(SelectedTrip.TripNumber));
                List<TripModel> returnFlights = new List<TripModel>();
                if (SelectedTrip.ReturnTripBool) returnFlights = FlightsViewModel.GetFlightsByTripNumber(SelectedTrip.ReturnTripN);
                if (flights.Count > 0)
                {
                    StringBuilder st = new StringBuilder();
                    flights.ForEach(f => { st.Append(" " + f.Date); });
                    string message;
                    if (flights.Count==1) message = "This trip can not be deleted because there is " + flights.Count + " flight in the schedule based on this trip with dates:" + st.ToString() + "." + " You should remove all flights based on this trip.";
                    else message = "This trip can not be deleted because there are " + flights.Count + " flights in the schedule based on this trip with dates:" + st.ToString() + "." + " You should remove all flights based on this trip.";
                    MessageBox.Show(message, "Delete trip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else 
                {
                     string message = "Delete trip " + SelectedTrip.TripNumber + " " + SelectedTrip.TownFrom + " - " + SelectedTrip.TownTo + " by " + SelectedTrip.AirwayCompany + " " + SelectedTrip.DepTime.ToShortTimeString() + " - " + SelectedTrip.ArrTime.ToShortTimeString() + " on " + SelectedTrip.Plane + "?";
                    DialogResult result= System.Windows.Forms.MessageBox.Show(message, "Delete trip", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(SelectedTrip.ReturnTripBool)
                    {
                        string message1 = "Trip " + SelectedTrip.TripNumber + " " + SelectedTrip.TownFrom + " - " + SelectedTrip.TownTo + " has return trip number " + SelectedTrip.ReturnTripN + ". Delete return trip as well?";
                        DialogResult result1 = MessageBox.Show(message1, "Delete return trip", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result1 == DialogResult.Yes)
                        {
                            if(returnFlights.Count>0)
                            {
                                StringBuilder st = new StringBuilder();
                                returnFlights.ForEach(f => { st.Append(" " + f.Date); });
                                string message2 = "This trip can not be deleted because there are " + returnFlights.Count + " flights in the schedule based on this trip with dates:" + st.ToString() + "." + " You should remove all flights based on this trip.";
                                MessageBox.Show(message2, "Delete return trip", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            { FlightsViewModel.DeleteTrip(SelectedTrip.ReturnTrip); Trips.Remove(SelectedTrip.ReturnTrip); }
                        }
                    }
                    if (result == DialogResult.Yes)
                    {
                        FlightsViewModel.DeleteTrip(SelectedTrip);
                        Trips.Remove(SelectedTrip);
                    }

                }
            }
        }

        private void AddPassenger()
        {
            PassengersInFlightModel NewPassenger = PassengersViewModel.AddNewPassenger(SelectedTrip,"newFlight");
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
            if(SelectedPassenger!=null)
            {
                PassengersInFlightModel newData = PassengersViewModel.EditPassengerData(SelectedPassenger, SelectedTrip,"new flight");
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
            if (SelectedTrip != null && SelectedDate != null && Passengers.Count != 0)
            {
                List<PassengersInFlightModel> passList = new List<PassengersInFlightModel>(Passengers);
                Passengers.Clear();
                foreach (var p in passList)
                {
                    p.Date = SelectedDate.ToShortDateString();
                    PassengersInFlightModel pass = PassengersViewModel.AddNewPassenger(p);
                    Passengers.Add(pass);
                }
   
                FlightsViewModel.NewFlight = new TripModel(SelectedTrip.TripNumber, SelectedDate.ToShortDateString(), SelectedTrip.AirwayCompany, SelectedTrip.Plane, SelectedTrip.TownFrom, SelectedTrip.TownTo, SelectedTrip.DepTime, SelectedTrip.ArrTime, null, Passengers);
            }
            else if (SelectedTrip == null) MessageBox.Show("Trip is not selected!");
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
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
