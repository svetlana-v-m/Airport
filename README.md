# Airport 
***RU*** Клиент для базы данных SQL, построенный на .NET Framework с испльзованием Entity framework, подход - DataBaseFirst.
В сборке два проекта: AirwaysWithEF и DAL.

***EN*** Simple client for simple airport database made with Entity Framework data-base first approach.
There are two projects in assembly: AirwaysWithEF and DAL.

## AirwaysWithEF project
***RU*** Этот проект содержит всю бизнес-логику, построен с применением паттерна MVVM и сдоержит традиционнно для этого подхода Models, ViewModels, Views.

***EN*** This project contains business logic and built with MVVM approach.
There are Models, Views and ViewModels.

## DAL project
***RU*** Этот проект является уровнем доступа к базе данных и содержит инструкции по взаимодействию с ней.

***EN*** This project contains instructions on interacting with data base.

## Data base
***RU*** База данных состоит из четырех таблиц:
- Company - список компаний-авиаперевозчиков.
- Trip - список полетов, связанный с таблицей Company.
- Passenger - список пассажиров.
- Pass_in_trip - данные о пассажирах и полетах, связанные с таблицами Passenger и Trip.

***EN*** Data base containts of 4 tables:
- Company - contains airway companies list. 
- Trip - contains trips list and connected with Company table by foreign key. 
- Passenger - contains passengers list. 
- Pass_in_trip - contains passenger data and trip data and connected with tables Passenger and Trip by foreign key.

## Functions
***RU***
1. Просмотр всех прилетов и отлетов в базе данных, либо для выбранного города/компании.
2. Добавление, редактирование списка пассажиров рейса.
3. Добавление нового рейса с использованием данных о существующих в базе данных рейсах.
4. Дбавление нового шаблона полета с последующим созданием рейса на основе этого шаблона.

***EN***
1. Observe all arrivals and departures in data base or for selected city or company.
2. Add new passenger, edit passenger data or delete passenger in existing flight.
3. Add new flight based on existing in data base trips.
4. Add new trip and then add new flight based on this trip.
