using DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirwaysWithEF.Views;
using AirwaysWithEF.Models;

namespace AirwaysWithEF.ViewModels
{
    class PassengersViewModel
    {
        public static PassengersInFlightModel NewPassenger;
        public static PassengersInFlightModel NewPassengerData;
        public static ObservableCollection<string> GetAllPassengers()
        {
            ObservableCollection<string> passengers = new ObservableCollection<string>();
            List<Passenger> list = new List<Passenger>(GetData.GetAllPassengersData());
            list.ForEach(p => passengers.Add(p.name));
            return passengers;
        }
        public static List<PassengersInFlightModel> GetPassengersInTrip(int tripNo)
        {
            List<PassengersInTripView> list = new List<PassengersInTripView>(DAL.GetData.GetPassengersInTripView(tripNo));
            List<PassengersInFlightModel> passList = new List<PassengersInFlightModel>();
            list.ForEach(p => passList.Add(new PassengersInFlightModel { Id = p.ID_psg, Date = p.date.ToShortDateString(), PassengerName = p.name, SeatNumber = p.place }));
            return passList;
        }
        public static Passenger ConvertToPassengerType(PassengersInFlightModel passenger)
        {
            Passenger p = new Passenger() { ID_psg=passenger.Id, name=passenger.PassengerName};
            return p;
        }
        public static PassengersInFlightModel AddNewPassenger(TripModel trip,string param)
        {
            List<PassengersInFlightModel> passList = new List<PassengersInFlightModel>();
            if (trip.PassengersList == null) trip.PassengersList = new ObservableCollection<PassengersInFlightModel>();
            else if (trip.PassengersList.Count > 0) passList = trip.PassengersList.ToList();
            AddPassenger AP = new AddPassenger(passList, param);
            AP.ShowDialog();
            if(AP.DialogResult == true)
            {
                
                if (param.Equals("newFlight"))
                {
                     return NewPassenger;
                }
                else
                {
                    bool flag = false;
                    List<Passenger> list = new List<Passenger>(GetData.GetAllPassengersData());
                    list.ForEach(p => { if (p.name.Trim().Equals(NewPassenger.PassengerName.Trim())) { NewPassenger.Id = p.ID_psg; flag = true; } });
                    if (NewPassenger.Id == 0 && list.Count > 0) NewPassenger.Id = list.Max(p => p.ID_psg) + 1;
                    Passenger newPass = ConvertToPassengerType(NewPassenger);
                    if (flag)
                    {
                        DAL.EditDB.AddNewPassengerToFlight(trip.TripNumber, DateTime.Parse(trip.Date), NewPassenger.Id, NewPassenger.SeatNumber);
                    }
                    else
                    {
                        DAL.EditDB.AddNewPassenger(newPass);
                        DAL.EditDB.AddNewPassengerToFlight(trip.TripNumber, DateTime.Parse(trip.Date), NewPassenger.Id, NewPassenger.SeatNumber);
                    }
                    return NewPassenger;
                }
            }
            else return null;
        }
        public static PassengersInFlightModel AddNewPassenger(PassengersInFlightModel pass)
        {
            List<Passenger> list = new List<Passenger>(GetData.GetAllPassengersData());
            list.ForEach(p => { if (p.name.Trim().Equals(pass.PassengerName.Trim())) pass.Id = p.ID_psg; });
            if (pass.Id == 0 && list.Count > 0)
            {
                pass.Id = list.Max(p => p.ID_psg) + 1;
                Passenger newPass = ConvertToPassengerType(pass);
                DAL.EditDB.AddNewPassenger(newPass);
            }
            return pass;
        }
        private static Pass_in_trip ConvertToPassInTripType(PassengersInFlightModel pass, TripModel trip)
        {
            Pass_in_trip pass_In_Trip = new Pass_in_trip
            {
                trip_no = trip.TripNumber,
                date = DateTime.Parse(trip.Date),
                ID_psg = pass.Id,
                place = pass.SeatNumber
            };
            return pass_In_Trip;
        }
        public static void DeletePassenger(PassengersInFlightModel pass, TripModel trip)
        {
            Pass_in_trip passToDelete = ConvertToPassInTripType(pass, trip);
            DAL.EditDB.DeletePassengerFromFlight(passToDelete);
        }
        public static PassengersInFlightModel EditPassengerData(PassengersInFlightModel pass, TripModel trip,string param)
        {
            EditPassengerData EPD = new EditPassengerData(trip.PassengersList.ToList(), pass);
            EPD.ShowDialog();
            if (EPD.DialogResult == true)
            {
                if(param.Equals("new flight"))
                {
                    return NewPassengerData;
                }
                else
                {
                    DAL.EditDB.EditPassengerData(ConvertToPassInTripType(pass, trip), NewPassengerData.SeatNumber, pass.PassengerName, NewPassengerData.PassengerName);
                }
            }
            return NewPassengerData;
        }
    }
}
