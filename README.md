# Airport
Simple client for simple airport database made with Entity Framework data-base first approach.
There are two projects in assembly: AirwaysWithEF and DAL.
# AirwaysWithEF project
This project contains business logic and built with MVVM approach.
There are Models, Views and ViewModels.
#DAL project
This project contains instructions on interacting with data base.
# Data base
Data base containts of 4 tables:Company, Trip, Passenger, Pass_in_trip.
Table Company contains airway companies list. 
Table Trip contains trips list and connected with Company table by foreign key.
Table Passenger contains passengers list.
Table Pass_in_trip contains passenger data and trip data and connected with tables Passenger and Trip by foreign key.
# Functions
1.Observe all arrivals and departures in data base or for selected city or company. 
2.Add new passenger, edit passenger data or delete passenger in existing flight.
3.Add new flight based on existing in data base trips.
4.Add new trip and then add new flight based on this trip.
