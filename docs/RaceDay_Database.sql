--Creates the database
CREATE DATABASE RaceDay;
GO

USE RaceDay;
GO

--Create organiser table
CREATE TABLE Organiser
(
    OrganiserID INT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(20)
);


--Create participants table
CREATE TABLE Participants
(

    ParticipantID INT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Surname VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Phone VARCHAR(20),
    Age INT NOT NULL
  
);


--Create event table
CREATE TABLE Event
(

    EventID VARCHAR(10) PRIMARY KEY,
    EventName VARCHAR(100) NOT NULL,
    EventDate DATE NOT NULL,
    Location VARCHAR(100) NOT NULL,
    Description VARCHAR(300),
    OrganiserID INT NOT NULL,

    FOREIGN KEY (OrganiserID)
        REFERENCES Organiser(OrganiserID)
  
);


--Creates category table
CREATE TABLE Category
(

   CategoryID VARCHAR(10) PRIMARY KEY,
    CategoryName VARCHAR(50) NOT NULL,
    Distance DECIMAL(5,2) NOT NULL,
    EntryFee DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    EventID VARCHAR(10) NOT NULL,

    FOREIGN KEY (EventID)
        REFERENCES Event(EventID),
		
	
	UNIQUE (EventID, CategoryID)
  
);

--Creates enrolment table
CREATE TABLE Enrolment
(

    EnrolmentID VARCHAR(15) PRIMARY KEY,
    ParticipantID INT NOT NULL,
    EventID VARCHAR(10) NOT NULL,
    CategoryID VARCHAR(10) NOT NULL,
    EnrolmentDate DATE NOT NULL,
    EnrolmentStatus VARCHAR(30) NOT NULL DEFAULT 'Active',

    FOREIGN KEY (ParticipantID)
        REFERENCES Participants(ParticipantID),

    FOREIGN KEY (EventID)
        REFERENCES Event(EventID),

    FOREIGN KEY (CategoryID)
        REFERENCES Category(CategoryID)
  
);


--Creates results table
CREATE TABLE Results
(

   ResultID VARCHAR(10) PRIMARY KEY,
    EnrolmentID VARCHAR(15) NOT NULL,
    FinishTime TIME,
    Position INT,
    ResultStatus VARCHAR(30) NOT NULL DEFAULT 'Pending',

    FOREIGN KEY (EnrolmentID)
        REFERENCES Enrolment(EnrolmentID)
  
);


--Inserts data into the organiser table
INSERT INTO Organiser (OrganiserID, FirstName, LastName, Email, Phone)
VALUES
   (1, 'Levi', 'Ackerman', 'levi@raceday.co.za', '0889456123'),
   (2, 'Eren', 'Jaeger', 'eren@raceday.co.za', '0812345678');
   
   
--Inserts data into the participants table
INSERT INTO Participants (ParticipantID, Name, Surname, Email, Phone, Age)
VALUES
    (1, 'Leo', 'Donna', 'ldonna@gmail.com', '0712345678', 27),
    (2, 'Gon', 'Matthews', 'gmatthews@gmail.com', '0723456789', 31);
	
	
--Inserts data into the event table	
INSERT INTO Event(EventID, EventName, EventDate, Location, Description, OrganiserID)
VALUES
    ('Ev-1', 'Johannesburg City Run', '2026-10-10', 'Johannesburg', 
     'Annual road running event through Johannesburg.', 1),

    ('Ev-2', 'Pretoria Spring Race', '2026-10-24', 'Pretoria', 
     'Community road race celebrating the spring season.', 2),

    ('Ev-3', 'Limpopo Marathon', '2026-11-07', 'Polokwane', 
     'Long-distance road race for runners of different levels.', 1);
	 
	 
	 
--Inserts data into category table	 
INSERT INTO Category (CategoryID, CategoryName, Distance, EntryFee, EventID)
VALUES
    -- Johannesburg City Run
    ('CAT1', '5 km Fun Run', 5.00, 80.00, 'Ev-1'),
    ('CAT2', '10 km Race', 10.00, 120.00, 'Ev-1'),
    ('CAT3', '21.1 km Half Marathon', 21.10, 180.00, 'Ev-1'),

    -- Pretoria Spring Race
    ('CAT4', '5 km Fun Run', 5.00, 70.00, 'Ev-2'),
    ('CAT5', '10 km Race', 10.00, 110.00, 'Ev-2'),
    ('CAT6', '21.1 km Half Marathon', 21.10, 170.00, 'Ev-2'),

    -- Limpopo Marathon
    ('CAT7', '10 km Race', 10.00, 100.00, 'Ev-3'),
    ('CAT8', '21.1 km Half Marathon', 21.10, 160.00, 'Ev-3'),
    ('CAT9', '42.2 km Marathon', 42.20, 250.00, 'Ev-3');
	
	
--Inserts data into enrolment table	
INSERT INTO Enrolment (EnrolmentID, ParticipantID, EventID, CategoryID, EnrolmentDate, EnrolmentStatus)
VALUES
    ('ENROL-1', 1, 'Ev-1', 'CAT2', '2026-09-01', 'Active'),
    ('ENROL-2', 2, 'Ev-1', 'CAT1', '2026-09-02', 'Active'),
    ('ENROL-3', 1, 'Ev-2', 'CAT5', '2026-09-03', 'Active'),
    ('ENROL-4', 2, 'Ev-3', 'CAT8', '2026-09-04', 'Active');
	

--Inserts data into results table
INSERT INTO Results (ResultID, EnrolmentID, FinishTime, Position, ResultStatus)
VALUES
    ('RS1', 'ENROL-1', '00:52:35', 14, 'Completed'),
    ('RS2', 'ENROL-2', '00:31:20', 8, 'Completed');
	


--Executes the tables with their inserted data	
SELECT * FROM Organiser;
SELECT * FROM Participants;
SELECT * FROM Event;
SELECT * FROM Category;
SELECT * FROM Enrolment;
SELECT * FROM Results;	