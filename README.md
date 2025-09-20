# ClinicApp

ClinicApp is a microservices-based application designed to manage appointments in clinical environments. Built primarily with C#, it leverages modern web technologies and best practices to provide a robust, scalable, and secure appointment management solution for hospitals and clinics.

## Features

- **Microservices Architecture**: Modular design for scalability, maintainability, and independent deployment.
- **Appointment Scheduling**: Efficiently manage, create, and track patient appointments.
- **Identity Management**: Integrated identity services for secure user authentication and role management.
- **JWT Authentication**: Secure API endpoints with JSON Web Token (JWT) authentication.
- **Extensible Design**: Easily add new services or extend existing ones to fit clinic-specific needs.

## Technologies Used

- **C#**
- **ASP.NET Core**
- **Razor Pages**
- **JWT (JSON Web Tokens)**
- **Entity Framework Core**

## Getting Started

### Prerequisites

- [.NET 6 SDK or later](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) or another supported database
- 
### Setup

1. **Clone the repository**
    ```bash
    git clone https://github.com/SamuelLeiva/ClinicApp.git
    cd ClinicApp
    ```

2. **Restore dependencies**
    ```bash
    dotnet restore
    ```

3. **Configure environment variables**

    Create a `.env` file or use `appsettings.json` to configure connection strings and JWT settings.

4. **Apply database migrations**
    ```bash
    dotnet ef database update
    ```

5. **Run the services**
    ```bash
    dotnet run --project <MicroserviceProjectName>
    ```


### Usage

- Access the API documentation (e.g., Swagger) at `https://localhost:<port>/swagger` after running the service.
- Use the provided endpoints to manage appointments, authenticate users, and access clinic resources.

## Project Structure

- `AppointmentService/` — Handles appointment-related logic.
- `IdentityService/` — Manages authentication and user identity.
- `Shared/` — Common models and utilities.
- `Web/` — Frontend interface (if included).

## Security

- All endpoints are protected with JWT authentication.
- Role-based access control for sensitive operations.
- Secure storage and transmission of sensitive data.


## License

This project is licensed under the MIT License.

## Contact

For questions or support, please contact [SamuelLeiva](https://github.com/SamuelLeiva).
