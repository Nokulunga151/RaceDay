# RaceDay

RaceDay is an event management system designed to manage road races and running events.
The system allows participants to register for events, select race categories and view their results, while organisers can create and manage events, categories, enrolments and race results


# Project overview
The RaceDay system provides functionality for:
 - User registration and login
 - Participant profile management
 - Event creation and management
 - Race category management
 - Participant enrolments
 - Race results

# User roles
Organiser

Organisers are responsible for managing RaceDay events. They can create, update and delete events and race categories, view event enrolments, and record or update race results.

Participant

Participants can create an account, manage their profile, view available events and categories, enrol in events, manage their enrolments, and view their race results.


# What Part 1 does
Part 1 establishes the planning and database foundation for the RaceDay system.

- **ERD** – Defines the database entities, relationships, primary keys, foreign keys and cardinalities.
- **API Endpoint Plan** – Defines the planned API endpoints, HTTP methods, roles, request bodies and expected responses.
- **SQL Database Script** – Creates and populates the RaceDay database tables, including the required constraints and sample data.

# Repository Structure
The `/docs` folder contains the main documentation and database files for the RaceDay project.

- `RaceDay_ERD.pdf` – Entity Relationship Diagram for the RaceDay database.
- `RaceDay_API_Endpoint_Plan.pdf` – Planned API endpoints and their requirements.
- `RaceDay_Database.sql` – SQL script used to create and populate the RaceDay database.

# How to execute SQL file

The RaceDay database is created using the `RaceDay_Database.sql` script located in the `docs` folder.

### Steps

1. Open **SQL Server Management Studio (SSMS)**.
2. Connect to the SQL Server instance.
3. Open `RaceDay_Database.sql`.
4. Click **Execute** or press **F5** to run the script.
5. The script creates the RaceDay database tables and inserts sample data.
6. Use **Object Explorer** and `SELECT` queries to verify that the tables and data were created successfully.

 # CI/CD

The GitHub Actions workflow automatically checks the RaceDay project whenever changes are pushed to the repository. It helps verify that the project builds successfully and identifies any errors that may need to be fixed.

# Technologies used
The RaceDay project used the following tools for Part 1:
 - drawio
 - SQL Server Management Studio (SSMS)
 - GitHub Actions

# YouTube Video Link
- https://youtu.be/UF6xu5px2Qc



   
