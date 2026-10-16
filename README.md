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

# Part 2: API Implementation
Part 2 implements the RaceDay API using ASP.NET Core and C#.

# Technologies Used
- C#
- ASP.NET Core Web API
- .NET 10
- Entity Framework Core
- Microsoft SQL Server
- SQL Server LocalDB for local development
- Swagger / OpenAPI
- xUnit for unit testing
- GitHub Actions for continuous integration


# Project Structure
The Part 2 solution is located in the RaceDay.Api/ directory.
- RaceDay.Api/ – ASP.NET Core API project and controllers.
- RaceDay.Contracts/ – Shared request and response contracts.
- RaceDay.Api.Tests/ – Unit test project.
- RaceDay.Api.slnx – Visual Studio solution file.
- .github/workflows/ – GitHub Actions workflow configuration.
- docs/ – Part 1 documentation and supporting project files.

# Project Structure
The Part 2 solution is located in the RaceDay.Api/ directory.
- RaceDay.Api/ – ASP.NET Core API project and controllers.
- RaceDay.Contracts/ – Shared request and response contracts.
- RaceDay.Api.Tests/ – Unit test project.
- RaceDay.Api.slnx – Visual Studio solution file.
- .github/workflows/ – GitHub Actions workflow configuration.
- docs/ – Part 1 documentation and supporting project files.

# Database Setup
Install SQL Server and SQL Server Management Studio (SSMS), with SQL Server LocalDB available for local development.

- Open the SQL script in the docs/ directory.
- Create the required database and tables.
- Configure the RaceDayConnection connection string in the API project's appsettings.json to match     your local SQL Server setup.

# How to Run the API
Prerequisites
- .NET 10 SDK
- Visual Studio or another .NET-compatible development environment
- SQL Server configured for the application

Steps
- Clone the repository.
- Open a terminal in the repository's root directory.
- Restore the solution's dependencies: dotnet restore RaceDay.Api/RaceDay.Api.slnx
- Build the solution: dotnet build RaceDay.Api/RaceDay.Api.slnx
- Run the API project: Open the solution in Visual Studio, set RaceDay.Api as the startup project,    and run it using the configured HTTPS profile.
- Ensure the database connection string is configured correctly before testing database-dependent     functionality.

# Authentication and Authorisation
The API provides registration and login functionality for supported user roles. Authentication is used to identify users, while role-based authorisation restricts access to protected functionality.

The API includes functionality for participant and organiser accounts. Protected endpoints should be tested using the appropriate authentication credentials and user role.

# API Endpoints
The API exposes endpoints for authentication, events, participant profiles, enrolments, results, and categories.

The main endpoint groups include:
- Authentication: Account registration and login.
- Events: Retrieve event information and access event-related operations.
- Profiles: Access participant profile functionality.
- Enrolments: Access participant enrolment information.
- Results: Access participant results.
- Categories: Retrieve event category information.

Refer to the API endpoint plan in docs/ and the implemented controllers for the complete list of routes, HTTP methods, request formats, and response codes.

# Swagger / OpenAPI
Swagger provides an interactive interface for exploring and testing the API endpoints.

When the API is running in the Development environment, open:

https://localhost:7257/swagger

The port may differ depending on the selected launch profile.

# Unit Testing
- The RaceDay.Api.Tests project contains automated unit tests written using xUnit.
- The tests cover scenarios involving authentication, event operations, access restrictions, participant functionality, and category retrieval.
- Run the tests from the repository root: dotnet test RaceDay.Api/RaceDay.Api.slnx --configuration Release
- Review the test output to confirm the number of passed, failed, and skipped tests.

# CI/CD: GitHub Actions
GitHub Actions is configured to run the RaceDay CI workflow when changes are pushed to the main branch or a pull request targets that branch.

The workflow performs the following steps:
- Checks out the repository.
- Installs the required .NET SDK.
- Restores project dependencies.
- Builds the solution.
- Runs the automated tests.

This workflow helps verify that the solution builds and its tests pass in the GitHub Actions environment.

# YouTube Video Link
- https://youtu.be/UF6xu5px2Qc



   
