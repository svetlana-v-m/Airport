USE [Airlines]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[ID_comp] [int] NOT NULL,
	[name] [char](10) NOT NULL,
 CONSTRAINT [PK2] PRIMARY KEY CLUSTERED 
(
	[ID_comp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pass_in_trip]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pass_in_trip](
	[trip_no] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[ID_psg] [int] NOT NULL,
	[place] [char](10) NOT NULL,
 CONSTRAINT [PK_pt] PRIMARY KEY CLUSTERED 
(
	[trip_no] ASC,
	[date] ASC,
	[ID_psg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Passenger]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Passenger](
	[ID_psg] [int] NOT NULL,
	[name] [char](20) NOT NULL,
 CONSTRAINT [PK_psg] PRIMARY KEY CLUSTERED 
(
	[ID_psg] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trip]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trip](
	[trip_no] [int] NOT NULL,
	[ID_comp] [int] NOT NULL,
	[plane] [char](10) NOT NULL,
	[town_from] [char](25) NOT NULL,
	[town_to] [char](25) NOT NULL,
	[time_out] [datetime] NOT NULL,
	[time_in] [datetime] NOT NULL,
 CONSTRAINT [PK_t] PRIMARY KEY CLUSTERED 
(
	[trip_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[MainView]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[MainView]
AS
SELECT        dbo.Trip.trip_no AS [Trip number], dbo.Trip.plane, dbo.Trip.town_from, dbo.Trip.town_to, dbo.Trip.time_out, dbo.Trip.time_in, dbo.Passenger.name AS [Passenger name], dbo.Pass_in_trip.date AS [Flight date], 
                         dbo.Pass_in_trip.place AS Seat, dbo.Company.*
FROM            dbo.Trip INNER JOIN
                         dbo.Company ON dbo.Trip.ID_comp = dbo.Company.ID_comp INNER JOIN
                         dbo.Pass_in_trip ON dbo.Trip.trip_no = dbo.Pass_in_trip.trip_no INNER JOIN
                         dbo.Passenger ON dbo.Pass_in_trip.ID_psg = dbo.Passenger.ID_psg
GO
/****** Object:  View [dbo].[PassengersInTripView]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PassengersInTripView]
AS
SELECT        dbo.Pass_in_trip.trip_no, dbo.Pass_in_trip.place, dbo.Pass_in_trip.date, dbo.Passenger.name, dbo.Pass_in_trip.ID_psg
FROM            dbo.Pass_in_trip INNER JOIN
                         dbo.Passenger ON dbo.Pass_in_trip.ID_psg = dbo.Passenger.ID_psg
GO
/****** Object:  View [dbo].[TripsView]    Script Date: 20.01.2020 13:59:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TripsView]
AS
SELECT        dbo.Trip.trip_no, dbo.Trip.plane, dbo.Trip.town_from, dbo.Trip.town_to, dbo.Trip.time_out, dbo.Trip.time_in, dbo.Company.name
FROM            dbo.Trip INNER JOIN
                         dbo.Company ON dbo.Trip.ID_comp = dbo.Company.ID_comp
GO
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (1, N'Don_avia  ')
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (2, N'Aeroflot  ')
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (3, N'Dale_avia ')
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (4, N'air_France')
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (5, N'British_AW')
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (6, N'Qatar AW  ')
INSERT [dbo].[Company] ([ID_comp], [name]) VALUES (7, N'Lufthansa ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1100, CAST(N'2003-04-29T00:00:00.000' AS DateTime), 1, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1100, CAST(N'2003-04-29T00:00:00.000' AS DateTime), 7, N'5a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1123, CAST(N'2003-04-05T00:00:00.000' AS DateTime), 7, N'8a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1123, CAST(N'2003-04-08T00:00:00.000' AS DateTime), 15, N'1b        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1123, CAST(N'2003-04-09T00:00:00.000' AS DateTime), 1, N'4c        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1123, CAST(N'2003-04-09T00:00:00.000' AS DateTime), 5, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1123, CAST(N'2003-04-09T00:00:00.000' AS DateTime), 6, N'4b        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1123, CAST(N'2003-04-09T00:00:00.000' AS DateTime), 7, N'8a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1124, CAST(N'2003-04-02T00:00:00.000' AS DateTime), 0, N'10a       ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1124, CAST(N'2003-04-02T00:00:00.000' AS DateTime), 2, N'2d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1124, CAST(N'2003-04-02T00:00:00.000' AS DateTime), 6, N'1e        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1124, CAST(N'2003-04-02T00:00:00.000' AS DateTime), 38, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1145, CAST(N'2019-04-26T00:00:00.000' AS DateTime), 5, N'1d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1145, CAST(N'2019-04-26T00:00:00.000' AS DateTime), 9, N'2a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1181, CAST(N'2003-04-13T00:00:00.000' AS DateTime), 2, N'2e        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1181, CAST(N'2003-04-13T00:00:00.000' AS DateTime), 3, N'6a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1181, CAST(N'2003-04-13T00:00:00.000' AS DateTime), 5, N'2d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1182, CAST(N'2003-04-13T00:00:00.000' AS DateTime), 0, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1182, CAST(N'2003-04-13T00:00:00.000' AS DateTime), 5, N'4b        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1182, CAST(N'2003-04-13T00:00:00.000' AS DateTime), 9, N'6d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1188, CAST(N'2003-04-01T00:00:00.000' AS DateTime), 8, N'3a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (1188, CAST(N'2003-04-01T00:00:00.000' AS DateTime), 40, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7771, CAST(N'2005-11-04T00:00:00.000' AS DateTime), 11, N'4a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7771, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 17, N'7a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7771, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 37, N'1c        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7772, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 7, N'2a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7772, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 12, N'1d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7772, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 37, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7772, CAST(N'2005-11-29T00:00:00.000' AS DateTime), 10, N'3a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7772, CAST(N'2005-11-29T00:00:00.000' AS DateTime), 13, N'1b        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7773, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 8, N'2e        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7773, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 13, N'2d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7773, CAST(N'2005-11-07T00:00:00.000' AS DateTime), 52, N'5f        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7778, CAST(N'2005-11-05T00:00:00.000' AS DateTime), 10, N'2a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (7778, CAST(N'2005-11-05T00:00:00.000' AS DateTime), 52, N'1f        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (8881, CAST(N'2005-11-08T00:00:00.000' AS DateTime), 5, N'2b        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (8881, CAST(N'2005-11-08T00:00:00.000' AS DateTime), 8, N'8a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (8881, CAST(N'2005-11-08T00:00:00.000' AS DateTime), 37, N'1d        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (8881, CAST(N'2005-11-08T00:00:00.000' AS DateTime), 54, N'1a        ')
INSERT [dbo].[Pass_in_trip] ([trip_no], [date], [ID_psg], [place]) VALUES (8882, CAST(N'2005-11-06T00:00:00.000' AS DateTime), 37, N'1a        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (0, N'Alan Rickman        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (1, N'Bruce Willis        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (2, N'George Clooney      ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (3, N'Kevin Costner       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (4, N'Donald Sutherland   ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (5, N'Jennifer Lopez      ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (6, N'Ray Liotta          ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (7, N'Samuel L. Jackson   ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (8, N'Nikole Kidman       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (9, N'Alan Rickman        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (10, N'Kurt Russell        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (11, N'Harrison Ford       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (12, N'Russell Crowe       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (13, N'Steve Martin        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (14, N'Michael Caine       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (15, N'Angelina Jolie      ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (16, N'Mel Gibson          ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (17, N'Michael Douglas     ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (18, N'John Travolta       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (19, N'Sylvester Stallone  ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (20, N'Tommy Lee Jones     ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (21, N'Catherine Zeta-Jones')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (22, N'Antonio Banderas    ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (23, N'Kim Basinger        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (24, N'Sam Neill           ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (25, N'Gary Oldman         ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (26, N'Clint Eastwood      ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (27, N'Brad Pitt           ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (28, N'Johnny Depp         ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (29, N'Pierce Brosnan      ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (30, N'Sean Connery        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (37, N'Mullah Omar         ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (38, N'Viktoria Backham    ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (39, N'Kilie Minogue       ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (40, N'Marilin Manson      ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (52, N'Naomi Kambel        ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (53, N'Lady Gaga           ')
INSERT [dbo].[Passenger] ([ID_psg], [name]) VALUES (54, N'Mark Zuckerberg     ')
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1001, 6, N'Airbus    ', N'Dubai                    ', N'Moscow                   ', CAST(N'1900-01-01T10:30:00.000' AS DateTime), CAST(N'1900-01-01T15:30:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1002, 6, N'Airbus    ', N'Moscow                   ', N'Dubai                    ', CAST(N'1900-01-01T11:15:00.000' AS DateTime), CAST(N'1900-01-01T16:15:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1003, 6, N'Airbus    ', N'Abu-Dabi                 ', N'Moscow                   ', CAST(N'1900-01-01T16:05:00.000' AS DateTime), CAST(N'1900-01-01T20:44:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1004, 6, N'Airbus    ', N'Moscow                   ', N'Abu-Dabi                 ', CAST(N'1900-01-01T11:35:00.000' AS DateTime), CAST(N'1900-01-01T04:39:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1005, 7, N'Airbus    ', N'Munchen                  ', N'Moscow                   ', CAST(N'1900-01-01T15:45:00.000' AS DateTime), CAST(N'1900-01-01T19:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1006, 7, N'Airbus    ', N'Moscow                   ', N'Munchen                  ', CAST(N'1900-01-01T14:15:00.000' AS DateTime), CAST(N'1900-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1007, 3, N'TU-154    ', N'Khabarovsk               ', N'Moscow                   ', CAST(N'1900-01-01T12:00:00.000' AS DateTime), CAST(N'1900-01-01T10:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1009, 4, N'Boeing    ', N'Marcele                  ', N'Moscow                   ', CAST(N'1900-01-01T15:20:00.000' AS DateTime), CAST(N'1900-01-01T19:30:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1011, 2, N'TU-154    ', N'Anapa                    ', N'Krasnodar                ', CAST(N'1900-01-01T17:00:00.000' AS DateTime), CAST(N'1900-01-01T18:15:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1013, 7, N'Boeing    ', N'Berlin                   ', N'Moscow                   ', CAST(N'1900-01-01T18:00:00.000' AS DateTime), CAST(N'1900-01-01T22:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1100, 4, N'Boeing    ', N'Paris                    ', N'Rostov                   ', CAST(N'1900-01-01T14:30:00.000' AS DateTime), CAST(N'1900-01-01T17:50:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1101, 4, N'Boeing    ', N'Rostov                   ', N'Paris                    ', CAST(N'1900-01-01T08:12:00.000' AS DateTime), CAST(N'1900-01-01T11:45:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1123, 3, N'TU-154    ', N'Rostov                   ', N'Vladivostok              ', CAST(N'1900-01-01T16:20:00.000' AS DateTime), CAST(N'1900-01-01T03:40:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1124, 3, N'TU-154    ', N'Vladivostok              ', N'Rostov                   ', CAST(N'1900-01-01T09:00:00.000' AS DateTime), CAST(N'1900-01-01T19:50:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1145, 2, N'IL-86     ', N'Rostov                   ', N'Moscow                   ', CAST(N'1900-01-01T09:35:00.000' AS DateTime), CAST(N'1900-01-01T11:23:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1146, 2, N'IL-86     ', N'Moscow                   ', N'Rostov                   ', CAST(N'1900-01-01T17:55:00.000' AS DateTime), CAST(N'1900-01-01T20:01:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1181, 1, N'TU-134    ', N'Rostov                   ', N'Moscow                   ', CAST(N'1900-01-01T06:12:00.000' AS DateTime), CAST(N'1900-01-01T08:01:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1182, 1, N'TU-134    ', N'Moscow                   ', N'Rostov                   ', CAST(N'1900-01-01T12:35:00.000' AS DateTime), CAST(N'1900-01-01T14:30:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1187, 1, N'TU-134    ', N'Rostov                   ', N'Moscow                   ', CAST(N'1900-01-01T15:42:00.000' AS DateTime), CAST(N'1900-01-01T17:39:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1188, 1, N'TU-134    ', N'Moscow                   ', N'Rostov                   ', CAST(N'1900-01-01T22:50:00.000' AS DateTime), CAST(N'1900-01-01T00:48:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1195, 1, N'TU-154    ', N'Rostov                   ', N'Moscow                   ', CAST(N'1900-01-01T23:30:00.000' AS DateTime), CAST(N'1900-01-01T01:11:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (1196, 1, N'TU-154    ', N'Moscow                   ', N'Rostov                   ', CAST(N'1900-01-01T04:00:00.000' AS DateTime), CAST(N'1900-01-01T05:45:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7771, 5, N'Boeing    ', N'London                   ', N'Singapore                ', CAST(N'1900-01-01T01:00:00.000' AS DateTime), CAST(N'1900-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7772, 5, N'Boeing    ', N'Singapore                ', N'London                   ', CAST(N'1900-01-01T12:00:00.000' AS DateTime), CAST(N'1900-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7773, 5, N'Boeing    ', N'London                   ', N'Singapore                ', CAST(N'1900-01-01T03:00:00.000' AS DateTime), CAST(N'1900-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7774, 5, N'Boeing    ', N'Singapore                ', N'London                   ', CAST(N'1900-01-01T14:00:00.000' AS DateTime), CAST(N'1900-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7775, 5, N'Boeing    ', N'London                   ', N'Singapore                ', CAST(N'1900-01-01T09:00:00.000' AS DateTime), CAST(N'1900-01-01T20:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7776, 5, N'Boeing    ', N'Singapore                ', N'London                   ', CAST(N'1900-01-01T18:00:00.000' AS DateTime), CAST(N'1900-01-01T08:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7777, 5, N'Boeing    ', N'London                   ', N'Singapore                ', CAST(N'1900-01-01T18:00:00.000' AS DateTime), CAST(N'1900-01-01T05:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (7778, 5, N'Boeing    ', N'Singapore                ', N'London                   ', CAST(N'1900-01-01T22:00:00.000' AS DateTime), CAST(N'1900-01-01T12:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (8881, 5, N'Boeing    ', N'London                   ', N'Paris                    ', CAST(N'1900-01-01T03:00:00.000' AS DateTime), CAST(N'1900-01-01T04:00:00.000' AS DateTime))
INSERT [dbo].[Trip] ([trip_no], [ID_comp], [plane], [town_from], [town_to], [time_out], [time_in]) VALUES (8882, 5, N'Boeing    ', N'Paris                    ', N'London                   ', CAST(N'1900-01-01T22:00:00.000' AS DateTime), CAST(N'1900-01-01T23:00:00.000' AS DateTime))
ALTER TABLE [dbo].[Pass_in_trip]  WITH CHECK ADD  CONSTRAINT [FK_Pass_in_trip_Passenger] FOREIGN KEY([ID_psg])
REFERENCES [dbo].[Passenger] ([ID_psg])
GO
ALTER TABLE [dbo].[Pass_in_trip] CHECK CONSTRAINT [FK_Pass_in_trip_Passenger]
GO
ALTER TABLE [dbo].[Pass_in_trip]  WITH CHECK ADD  CONSTRAINT [FK_Pass_in_trip_Trip] FOREIGN KEY([trip_no])
REFERENCES [dbo].[Trip] ([trip_no])
GO
ALTER TABLE [dbo].[Pass_in_trip] CHECK CONSTRAINT [FK_Pass_in_trip_Trip]
GO
ALTER TABLE [dbo].[Trip]  WITH CHECK ADD  CONSTRAINT [FK_Trip_Company] FOREIGN KEY([ID_comp])
REFERENCES [dbo].[Company] ([ID_comp])
GO
ALTER TABLE [dbo].[Trip] CHECK CONSTRAINT [FK_Trip_Company]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[34] 4[40] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Trip"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Company"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 102
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Pass_in_trip"
            Begin Extent = 
               Top = 106
               Left = 460
               Bottom = 236
               Right = 634
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Passenger"
            Begin Extent = 
               Top = 6
               Left = 462
               Bottom = 102
               Right = 636
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 2250
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MainView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'MainView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[36] 2[5] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Pass_in_trip"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Passenger"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 102
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PassengersInTripView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'PassengersInTripView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Trip"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 212
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Company"
            Begin Extent = 
               Top = 6
               Left = 250
               Bottom = 102
               Right = 424
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TripsView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TripsView'
GO
