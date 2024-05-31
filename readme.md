#  API Usage Guide For Domain Driven 

This guide provides instructions on how to set up and interact with the GreenFlux API, which is running in a Docker container alongside a PostgreSQL database.

## Prerequisites

- Docker and Docker Compose installed on your machine.
- .NET Core SDK (appropriate version).
- Any API client like Postman or a browser for API calls.

## Setup

1. **Start the Services:**
   Ensure that your Docker containers for both the API and the PostgreSQL database are up and running. You can start both services using Docker Compose:

   ```bash
   docker-compose up -d
	 ```

This command will start all services defined in your `docker-compose.yml` file in detached mode.

2.  **Database Configuration:** Ensure that your application's connection strings in the configuration file (`appsettings.json`) match the credentials used in Docker. Typically, it would look something like this:

  ```bash
    "ConnectionStrings": {
    "GreenFluxDb": "Host=db;Port=5432;Database=greenfluxdb;Username=greenfluxuser;Password=greenflux123;SslMode=Disable;"
  }
  ```
  
3. **Swagger**
IIS : 
https://localhost:44341/swagger/index.html
Docker : 
https://localhost:32780/swagger/index.html
Docker-Compose: 
http://localhost:8080/swagger/index.html