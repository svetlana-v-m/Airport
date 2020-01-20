using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirwaysWithEF.Models
{
    public class TripModel
    {
        public int TripNumber { get; set; }
        public bool ReturnTripBool { get; set; }
        public int ReturnTripN { get; set; }
        public TripModel ReturnTrip { get; set; }
        public string Date { get; set; }
        public CompanyModel AirwayCompany { get; set; }
        public string Plane { get; set; }
        public string TownFrom { get; set; }
        public string TownTo { get; set; }
        public DateTime DepTime { get; set; }
        public string DepTimeString { get; set; }
        public DateTime ArrTime { get; set; }
        public string ArrTimeString { get; set; }
        public string DepOrArrFlag { get; set; }
        public ObservableCollection<PassengersInFlightModel> PassengersList { get; set; }

        public TripModel(int tripNo,string date,CompanyModel airwayCompany,string plane,string townFrom,string townTo,DateTime depTime,DateTime arrTime, string depOrArrFlag,ObservableCollection<PassengersInFlightModel> passList)
        {
            TripNumber = tripNo;
            Date = date;
            AirwayCompany = airwayCompany;
            Plane = plane;
            TownFrom = townFrom;
            TownTo = townTo;
            DepTime = depTime;
            DepTimeString = DepTime.ToShortTimeString();
            ArrTime = arrTime;
            ArrTimeString = ArrTime.ToShortTimeString();
            if(depOrArrFlag==null)
            {
                if (tripNo % 2 == 0) DepOrArrFlag = "arrival";
                else DepOrArrFlag = "departure";
            }
            else DepOrArrFlag = depOrArrFlag;
            if(passList!=null) PassengersList = new ObservableCollection<PassengersInFlightModel>(passList);
        }

        public TripModel(TripModel trip)
        {
            TripNumber = trip.TripNumber;
            Date = trip.Date;
            AirwayCompany = trip.AirwayCompany;
            Plane = trip.Plane;
            TownFrom = trip.TownFrom;
            TownTo = trip.TownTo;
            DepTime = trip.DepTime;
            ArrTime = trip.ArrTime;
            if (trip.DepOrArrFlag == null)
            {
                if (trip.TripNumber % 2 == 0) DepOrArrFlag = "arrival";
                else DepOrArrFlag = "departure";
            }
            else DepOrArrFlag = trip.DepOrArrFlag;
            PassengersList = trip.PassengersList;
            ReturnTripBool = trip.ReturnTripBool;
            ReturnTripN = trip.ReturnTripN;
            ReturnTrip = trip.ReturnTrip;
        }
    }
}
