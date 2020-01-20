using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AirwaysWithEF.Models;
using AirwaysWithEF.ViewModels;
using AirwaysWithEF.Views;
using DAL;

namespace AirwaysWithEF
{
    class FlightsViewModel
    {
        private static List<Trip> Flights;
        private static List<TripModel> flightsModels;
        public static List<TripModel> NewTrip { get; set; } = new List<TripModel>();
        public static TripModel ChangedTrip { get; set; } 
        public static TripModel NewFlight { get; set; }
        public static TripModel ChangedFlight { get; set; }
        //private static List<string> Cities { get; set; } = new List<string>(GetAllDepCities());

        public static void GetFlights(string city,string company,ref List<TripModel> deps,ref List<TripModel> arrs)
        {
            List<Trip> flights = new List<Trip>(DAL.GetData.GetTripsInTripTable(city, company));
            flightsModels = new List<TripModel>();
            List<Company> companies = new List<Company>(GetAllCompanies());
            foreach (var t in flights)
            {
                ObservableCollection<PassengersInFlightModel> passList = new ObservableCollection<PassengersInFlightModel>(PassengersViewModel.GetPassengersInTrip(t.trip_no));
                List<string> tripDates = new List<string>();
                foreach (var p in passList) tripDates.Add(p.Date);
                tripDates.Distinct().ToList().ForEach(tripDate => 
                {
                    if ((city != null && company == null) || (city != null && company != null))
                    {
                        if (t.town_from.Trim() == city)
                        {
                           flightsModels.Add(new TripModel(t.trip_no, tripDate, new CompanyModel(GetAllCompanies().Where(c=>c.ID_comp==t.ID_comp).FirstOrDefault()), t.plane.Trim(), t.town_from.Trim(), t.town_to.Trim(), t.time_out, t.time_in, "departure", passList));
                        }
                        else if (t.town_to.Trim() == city)
                            flightsModels.Add(new TripModel(t.trip_no, tripDate, new CompanyModel(GetAllCompanies().Where(c => c.ID_comp == t.ID_comp).FirstOrDefault()), t.plane.Trim(), t.town_from.Trim(), t.town_to.Trim(), t.time_out, t.time_in, "arrival", passList));
                    }
                    else if (city == null && company != null|| city == null && company == null)
                    {
                        flightsModels.Add(new TripModel(t.trip_no, tripDate, new CompanyModel(GetAllCompanies().Where(c => c.ID_comp == t.ID_comp).FirstOrDefault()), t.plane.Trim(), t.town_from.Trim(), t.town_to.Trim(), t.time_out, t.time_in, null, passList));
                    }
                });
            }
  
            foreach(TripModel f in flightsModels)
            {
                if (f.DepOrArrFlag.Equals("departure")) deps.Add(f);
                else arrs.Add(f);
            }
        }

        public static List<TripModel> GetTrips()
        {
            Flights = new List<Trip>(DAL.GetData.GetTrips(null, null));
            flightsModels = new List<TripModel>();
            foreach (Trip t in Flights)
            {
                flightsModels.Add(new TripModel(t.trip_no,null, new CompanyModel(GetAllCompanies().Where(c => c.ID_comp == t.ID_comp).FirstOrDefault()), t.plane.Trim(), t.town_from.Trim(), t.town_to.Trim(), t.time_out, t.time_in, null, new ObservableCollection<PassengersInFlightModel>()));
            }
            return flightsModels;
        }

        public static List<TripModel> GetFlightsByTripNumber(int tripN)
        {
            List<MainView> flights = new List<MainView>(DAL.GetData.GetFlightsByTripNumber(tripN));
            List<TripModel> flightsModels = new List<TripModel>();
            flights.ForEach(f => { flightsModels.Add(new TripModel(f.Trip_number, f.Flight_date.ToShortDateString(), new CompanyModel(GetAllCompanies().Where(c => c.ID_comp == f.company.ID_comp).FirstOrDefault()), f.plane, f.town_from, f.town_to, f.time_out, f.time_in, null, null)); });
            return flightsModels.Distinct().ToList();
        }

        public static List<CompanyModel> GetAllCompanies()
        {
            List<CompanyModel> compList = new List<CompanyModel>();
            List<Company> list = new List<Company>(DAL.GetData.GetAllCompanies());
            list.ForEach(c => compList.Add(new CompanyModel(c)));
            return compList;
        }

        public static List<string> GetAllCities()
        {
            List<string> cities = new List<string>(GetData.GetAllCities());
            return cities;
        }

        public static List<int> GetExistingTripNumbers()
        {
            List<int> numbers = new List<int>(DAL.GetData.GetTripsNumbers());
            return numbers;
        }

        public static List<string> GetExistingPlanes()
        {
            List<string> planes = new List<string>(DAL.GetData.GetAllPlanes());
            return planes;
        }

        public static TripModel CreateNewFlight()
        {
            AddFlight NF = new AddFlight();
            NF.ShowDialog();
            if (NF.DialogResult == true)
            {
                SaveNewFlight(NewFlight);
                return NewFlight;
            }
            else return null;
        }

        public static TripModel EditFlight(TripModel oldFlight)
        {
            EditFlight EF = new EditFlight(oldFlight);
            EF.ShowDialog();
            if (EF.DialogResult == true)
            {
                ChangeFlightData(oldFlight);
                return ChangedFlight;
            }
            else return null;
        }

        public static TripModel SaveNewFlight(TripModel newFlight)
        {
            foreach (var p in newFlight.PassengersList)
            {
                Pass_in_trip pass_In_Trip = new Pass_in_trip { trip_no = newFlight.TripNumber, place = p.SeatNumber, date = DateTime.Parse(newFlight.Date), ID_psg=p.Id};
                    DAL.EditDB.AddNewPassInFlight(pass_In_Trip);
                
            }
            return newFlight;
        }

        public static TripModel ChangeFlightData(TripModel oldFlightData)
        {
            if (ChangedFlight != null)
            {
                    foreach (var p in oldFlightData.PassengersList)
                    {
                        Pass_in_trip pass_In_Trip = new Pass_in_trip { trip_no = oldFlightData.TripNumber, place = p.SeatNumber, date = DateTime.Parse(oldFlightData.Date), ID_psg = p.Id };
                        DAL.EditDB.DeletePassengerFromFlight(pass_In_Trip);
                    }
                    foreach (var p in ChangedFlight.PassengersList)
                    {
                        Pass_in_trip pass_In_Trip = new Pass_in_trip { trip_no = ChangedFlight.TripNumber, place = p.SeatNumber, date = DateTime.Parse(ChangedFlight.Date), ID_psg = p.Id };
                        DAL.EditDB.AddNewPassengerToFlight(ChangedFlight.TripNumber,DateTime.Parse(ChangedFlight.Date),p.Id,p.SeatNumber);
                    }
                return ChangedFlight;
            }
            else return null;
        }

        public static void DeleteFlight(TripModel flight)
        {
            foreach(var p in flight.PassengersList) PassengersViewModel.DeletePassenger(p, flight);
        }

        public static List<TripModel> CreateNewTrip()
        {
            AddTrip addTrip = new AddTrip();
            addTrip.ShowDialog();
            if (addTrip.DialogResult == true)
            {
                if (NewTrip.Count == 1)
                {
                    SaveNewTrip(NewTrip[0]);
                    List<TripModel> trip = new List<TripModel>(NewTrip);
                    NewTrip.Clear();
                    return trip;
                }
                else if (NewTrip.Count == 2)
                {
                    NewTrip.ForEach(nt => SaveNewTrip(nt));
                    List<TripModel> trip = new List<TripModel>(NewTrip);
                    NewTrip.Clear();
                    return trip;
                }
                else return null;
            }
            else return null;
        }

        public static TripModel EditTrip(TripModel oldTripData)
        {
            EditTrip editTrip = new EditTrip(oldTripData);
            editTrip.ShowDialog();
            if (editTrip.DialogResult == true) return ChangedTrip;
            else return null;
        }

        public static void EditTrip(TripModel oldTripData,TripModel newTripData)
        {
            ChangedTrip = newTripData;
            TripModel trip = new TripModel(oldTripData.TripNumber, null, newTripData.AirwayCompany, newTripData.Plane, newTripData.TownFrom, newTripData.TownTo, newTripData.DepTime, newTripData.ArrTime, null, null);
            SaveChangedTrip(trip);
        }

        public static void SaveNewTrip(TripModel trip)
        {            
            if (trip.AirwayCompany.ID_comp == 0) trip.AirwayCompany=SaveNewCompany(trip.AirwayCompany.name);

            Trip newTrip = new Trip { trip_no = trip.TripNumber, town_to = trip.TownTo, town_from = trip.TownFrom, time_out = trip.DepTime, time_in = trip.ArrTime, plane = trip.Plane, Company=trip.AirwayCompany, ID_comp=trip.AirwayCompany.ID_comp};
            EditDB.AddNewTrip(newTrip);
        }

        public static void SaveChangedTrip(TripModel trip)
        {
            if (trip.AirwayCompany.ID_comp == 0) trip.AirwayCompany = SaveNewCompany(trip.AirwayCompany.name);

            Trip changedTrip = new Trip { trip_no = trip.TripNumber, town_from = trip.TownFrom, town_to = trip.TownTo, time_out = trip.DepTime, time_in = trip.ArrTime, plane = trip.Plane, ID_comp = trip.AirwayCompany.ID_comp, Company=trip.AirwayCompany };
            DAL.EditDB.EditTrip(changedTrip);
        }

        public static void DeleteTrip(TripModel trip)
        {
            DAL.EditDB.DeleteTrip(new Trip { trip_no = trip.TripNumber, town_to = trip.TownTo, town_from = trip.TownFrom, time_out = trip.DepTime, time_in = trip.ArrTime, plane = trip.Plane, Company=trip.AirwayCompany, ID_comp=trip.AirwayCompany.ID_comp });
        }

        private static CompanyModel SaveNewCompany(string compName)
        {
            List<int> Ids = new List<int>();
            foreach (var c in GetAllCompanies()) Ids.Add(c.ID_comp);
            CompanyModel newCompany = new CompanyModel { ID_comp = Ids.Max() + 1, name = compName };
            EditDB.AddNewCompany(newCompany);
            return newCompany;
        }
    }
}
