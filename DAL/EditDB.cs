using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DAL
{
    public static class EditDB
    {

        public static void AddNewPassenger(Passenger passenger)
        {
            AddPassengerToDB(passenger);
        }
        private static void AddPassengerToDB(Passenger pass)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Passenger.Add(pass);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void AddNewPassengerToFlight(int TripN,DateTime tripDate,int passID, string passSeat)
        {
            AddNewPassInTripToDB(TripN,tripDate,passID,passSeat);
        }
        private static void AddNewPassInTripToDB(int _TripN, DateTime _tripDate, int _passID, string _passSeat)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Pass_in_trip.Add(new Pass_in_trip { trip_no=_TripN, date=_tripDate, ID_psg=_passID, place=_passSeat});
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void DeletePassengerFromFlight(Pass_in_trip pass_In_Trip)
        {
            DeletePassengerFromPass_In_Flight(pass_In_Trip);
        }
        private static void DeletePassengerFromPass_In_Flight(Pass_in_trip pass)
        {
             using (var db = new AirlinesEntities())
            {
                Pass_in_trip pt = db.Pass_in_trip.Where(p => p.trip_no.Equals(pass.trip_no) && p.date.Equals(pass.date) && p.ID_psg.Equals(pass.ID_psg)).FirstOrDefault();
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Pass_in_trip.Remove(pt);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void EditPassengerData(Pass_in_trip pass_in_trip,string newPlace, string oldName,string newName)
        {
            EditPassengerDataInDB(newPlace,oldName,newName, pass_in_trip);
        }
        private static void EditPassengerDataInDB(string newPassPlace,string oldPassName,string newPassName, Pass_in_trip trip)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        Passenger passTochange = db.Passenger.Where(pass => pass.name == oldPassName).First();
                        passTochange.name = newPassName;
                        Pass_in_trip pass_in_tripToChange = db.Pass_in_trip.Where(pit => pit.trip_no.Equals(trip.trip_no) && pit.date.Equals(trip.date) && pit.ID_psg.Equals(trip.ID_psg)).First();
                        pass_in_tripToChange.place = newPassPlace;
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void AddNewTrip(Trip trip)
        {
            AddNewTripToDB(trip);
        }
        private static void AddNewTripToDB(Trip trip)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Trip.Add(trip);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception Ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void EditTrip(Trip trip)
        {
            EditTripInDB(trip);
        }
        private static void EditTripInDB(Trip trip)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var t in db.Trip)
                        {
                            if (t.trip_no.Equals(trip.trip_no))
                            {
                                t.ID_comp = trip.ID_comp;
                                t.plane = trip.plane;
                                t.time_in = trip.time_in;
                                t.time_out = trip.time_out;
                                t.town_from = trip.town_from;
                                t.town_to = trip.town_to;
                                break;
                            }
                        }
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex) { transaction.Rollback(); }
                }
            }
        }

        public static void DeleteTrip(Trip trip)
        {
            DeleteTripFromDB(trip);
        }
        private static void DeleteTripFromDB(Trip trip)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Trip.Remove(trip);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void AddNewPassInFlight(Pass_in_trip pass_In_Trip)
        {
            AddNewPassInTripToDB(pass_In_Trip);
        }
        private static void AddNewPassInTripToDB(Pass_in_trip pass_In_Trip)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Pass_in_trip.Add(pass_In_Trip);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public static void AddNewCompany(Company newCompany)
        {
            AddNewCompanyToDB(newCompany);
        }
        private static void AddNewCompanyToDB(Company company)
        {
            using (var db = new AirlinesEntities())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Company.Add(company);
                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
