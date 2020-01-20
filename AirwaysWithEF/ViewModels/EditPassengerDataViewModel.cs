
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AirwaysWithEF.ViewModels
{
    public class EditPassengerDataViewModel:INotifyPropertyChanged
    {
        public ObservableCollection<string> Passengers { get; set; }
        public ObservableCollection<string> AvailiableSeats { get; set; }
        private List<PassengersInFlightModel> PassengersInFlight;
        public static PassengersInFlightModel NewPassengerData;
        private List<string> OccupiedSeats;
        private string passengerName;
        public string PassengerName
        {
            get { return passengerName; }
            set { passengerName = value;
                
                OnPropertyChanged("PassengerName"); }
        }
        private string selectedPassenger;
        public string SelectedPassenger
        {
            get { return selectedPassenger; }
            set { selectedPassenger = value; if (SelectedPassenger != null) PassengerName = SelectedPassenger; OnPropertyChanged("SelectedPassenger"); }
        }
        private string passengerSeat;
        public string PassengerSeat
        {
            get { return passengerSeat; }
            set { passengerSeat = value;OnPropertyChanged("PassengerSeat"); }
        }
        private string selectedSeat;
        public string SelectedSeat
        {
            get { return selectedSeat; }
            set { selectedSeat = value; if (SelectedSeat != null) PassengerSeat = SelectedSeat; OnPropertyChanged("SelectedSeat"); }
        }
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ?? (saveCommand = new RelayCommand(obj =>
                    {
                        NewPassengerData = new PassengersInFlightModel { PassengerName = PassengerName, SeatNumber = PassengerSeat };
                        PassengersViewModel.NewPassengerData = NewPassengerData;
                    }));
            }
        }

        public EditPassengerDataViewModel(List<PassengersInFlightModel> list,PassengersInFlightModel pass)
        {
            InitialiseCollections(list);
            FillInCollections();
            if(pass!=null)
            {
                PassengerName = pass.PassengerName;
                PassengerSeat = pass.SeatNumber;
            }
        }
        
        private void InitialiseCollections(List<PassengersInFlightModel> list)
        {
            PassengersInFlight = new List<PassengersInFlightModel>(list);
            Passengers = new ObservableCollection<string>();
            AvailiableSeats = new ObservableCollection<string>();
        }

        private void FillInCollections()
        {
            FillInPassengersCollection();
            FillInSeatsCollection();
        }

        private void FillInPassengersCollection()
        {
            Passengers = PassengersViewModel.GetAllPassengers();
            if(PassengersInFlight!=null)
            {
                PassengersInFlight.ForEach(p =>
                {
                    if (Passengers.Contains(p.PassengerName)) Passengers.Remove(p.PassengerName);
                });
            }
        }

        private void FillInSeatsCollection()
        {
            OccupiedSeats = new List<string>();
            if (PassengersInFlight != null) PassengersInFlight.ForEach(p => OccupiedSeats.Add(p.SeatNumber.Trim()));

            for(int i=0;i<10;i++)
            {
                
                for (int j=0;j<6;j++)
                {
                    StringBuilder seat = new StringBuilder();
                    seat.Append(i + 1);
                    switch (j)
                    {
                        case 0: seat.Append("a"); break;
                        case 1: seat.Append("b"); break;
                        case 2: seat.Append("c"); break;
                        case 3: seat.Append("d"); break;
                        case 4: seat.Append("e"); break;
                        case 5: seat.Append("f"); break;
                    }
                    if (!OccupiedSeats.Contains(seat.ToString())) AvailiableSeats.Add(seat.ToString());
                }
                
            }
        }
  
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
