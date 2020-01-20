using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public static class GetData
    {
        public static List<Trip> GetTrips(string city,string company)
        {
            List<Trip> trips = new List<Trip>();
            using (var db = new AirlinesEntities())
            {
                if(city!=null&&company!=null)
                {
                    db.Trip.Where(c => c.Company.name.Equals(company) && c.town_from.Equals(city)).ToList().ForEach(t => trips.Add(t));
                    db.Trip.Where(c => c.Company.name.Equals(company) && c.town_to.Equals(city)).ToList().ForEach(t => trips.Add(t));
                }
                else if(city!=null&&company==null)
                {
                    db.Trip.Where(c => c.town_from.Equals(city)).ToList().ForEach(t => trips.Add(t));
                    db.Trip.Where(c => c.town_to.Equals(city)).ToList().ForEach(t => trips.Add(t));
                }
                else if(city==null&&company!=null)
                {
                    trips = db.Trip.Where(c => c.Company.name.Equals(company)).ToList();
                }
                else if(city==null&&company==null)
                {
                    db.Trip.ToList().ForEach(t => trips.Add(t));
                }
            }
            return trips;
        }

        public static List<Trip> GetTripsInTripTable(string city, string company)
        {
            List<Trip> trips = new List<Trip>();
            using (var db = new AirlinesEntities())
            {
                if (city != null && company != null)
                {
                    db.Trip.Where(c => c.Company.name.Equals(company) && c.town_from.Equals(city)).ToList().ForEach(t => trips.Add(t));
                    db.Trip.Where(c => c.Company.name.Equals(company) && c.town_to.Equals(city)).ToList().ForEach(t => trips.Add(t));
                }
                else if (city != null && company == null)
                {
                    db.Trip.Where(c => c.town_from.Equals(city)).ToList().ForEach(t => trips.Add(t));
                    db.Trip.Where(c => c.town_to.Equals(city)).ToList().ForEach(t => trips.Add(t));
                }
                else if (city == null && company != null)
                {
                    trips = db.Trip.Where(c => c.Company.name.Equals(company)).ToList();
                }
                else if (city == null && company == null)
                {
                    db.Trip.ToList().ForEach(t => trips.Add(t));
                }
            }
            return trips;
        }

        public static List<MainView> GetFlightsByTripNumber(int tripN)
        {
            List<MainView> list = new List<MainView>();
            using (var db = new AirlinesEntities())
            {
                db.MainView.Where(t => t.Trip_number == tripN).ToList().ForEach(f => list.Add(f));
            }
            return list;
        }

        public static List<string> GetAllCities()
        {
            List<string> data = new List<string>();
            List<Trip> deps = new List<Trip>();
            using (var db = new AirlinesEntities())
            {
                foreach (var c in db.Trip) data.Add(c.town_from.Trim());
                foreach (var c in db.Trip) data.Add(c.town_to.Trim());
            }

            return data.Distinct().ToList();
        }
        
        public static List<Company> GetAllCompanies()
        {
            List<Company> companies = new List<Company>();
            using (var db = new AirlinesEntities())
            {
                companies = db.Company.ToList();
            }

            return companies;
        }

        public static List<Pass_in_trip> GetPassengersInTrip(int TripNo)
        {
            List<Pass_in_trip> pass_In_Trips = new List<Pass_in_trip>();
            using (var db = new AirlinesEntities())
            {
                pass_In_Trips = db.Pass_in_trip.Where(tn => tn.trip_no == TripNo).ToList();
            }
            return pass_In_Trips;
        }

        public static List<PassengersInTripView> GetPassengersInTripView(int TripNo)
        {
            List<PassengersInTripView> pass_In_Trips = new List<PassengersInTripView>();
            using (var db = new AirlinesEntities())
            {
                pass_In_Trips = db.PassengersInTripView.Where(tn => tn.trip_no == TripNo).ToList();
            }
            return pass_In_Trips;
        }

        public static List<Passenger> GetAllPassengersData()
        {
            List<Passenger> passengers = new List<Passenger>();
            using (var db = new AirlinesEntities())
            {
                foreach (var p in db.Passenger) { p.name.Trim(); passengers.Add(p); }
            }
            return passengers;
        }

        public static int GetPassengerIDByName(string passName)
        {
            int idPass = 0;
            using (var db = new AirlinesEntities())
            {
                idPass = db.Passenger.Where(p => p.name == passName).Select(id => id.ID_psg).FirstOrDefault();
            }
            return idPass;
        }

        public static Pass_in_trip GetPass_In_Trip(int tripNo,DateTime TripDate,int PassId)
        {
            Pass_in_trip pass;
            using (var db = new AirlinesEntities())
            {
                pass = db.Pass_in_trip.Where(t => (t.trip_no == tripNo)&&(t.date==TripDate)&&(t.ID_psg==PassId)).FirstOrDefault();
            }
            return pass;
        }

        public static List<int> GetTripsNumbers()
        {
            List<int> numbers = new List<int>();
            using (var db = new AirlinesEntities())
            {
                db.Trip.ToList().ForEach(t => numbers.Add(t.trip_no));
            }
            return numbers.Distinct().ToList();
        }

        public static List<string> GetAllPlanes()
        {
            List<string> planes = new List<string>();
            using (var db = new AirlinesEntities())
            {
                db.Trip.ToList().ForEach(t => planes.Add(t.plane));
            }
            return planes.Distinct().ToList();
        }

    }
}
