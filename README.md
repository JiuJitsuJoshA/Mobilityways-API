# Mobilityways-API - Coding Test

This is my solution for the Mobilityways coding test to create a minimal API REST API that has the following:

1. Create user
    - a. This endpoint should require name, email and password and create a new user and store in-memory.  

2. Get JWT token 
    - a. This endpoint should require an email and password and generate a JSON Web Token (JWT) for the matching user if found in in-memory store. 

3. List users
    - a. This endpoint should list all users in the in-memory store. This endpoint should require bearer authentication using a valid JWT (obtained from above endpoint).

## Support

- This project runs on .NET 7
- Swagger is enabled in development mode and has been configured to use Authorization
- **Please Note: The JWT Key is used from appsettings.json, this is used here only for the purposes of this test. In production this would typically be retrieved from an App Configuration Store which points to a secret in Azure Key Vault.**

## Description

This project adopts a clean architecture approach to provide clear separation and minimise tight coupling. I chose to implement clean architecture because one of its primary advantages is enhancing maintainability. While this project may currently be small in scale, it's common for projects to start small and gradually expand, eventually becoming challenging to manage. I firmly believe that the minor upfront effort required for setting up the project structure in this way will pay off in the long run should the application grow in size.

The project is structured as followed:

- **API** – Responsible for all endpoints, sets up JWT settings, authorization, Swagger and registers dependencies. Also includes a service class that is responsible for sending Mediator requests.
- **Application Layer** – Uses CQRS pattern with Mediator to separate read and write operations. Also Includes abstractions for services which are injected via constructor injection. 
- **Domain Layer** – Houses the entities used for the business domain. Only the User entity is used in this project which inherits the BaseEntity which is setup to use a Domain Drive Design approach if needed. Also includes the repository interface.
- **Infrastructure Layer** – This includes concrete classes for implementations of a generic repository, services and setup of EF Core.

### Explanations

-	I decided to use EF Core with the selection of using a in memory database (as per spec) as the infrastructure is already in place should the application need to grow. There would be little refactoring needed to be undertaken should there be a need to use a more permanent storage solution, e.g. SQL Database, Cosmos DB etc
-	CQRS pattern used to provide separation for read and write operations
-	Mediator pattern used to decouple code and provide better testing for unit tests

### Thoughts
 
With more time I would of liked to implement some of the following:

-	Integration tests for the API to test the whole process from start to finish (happy path mainly)
-	Add more validation for User with fluent assertations and setting up mediatr pipeline behaviour
-	Implement a cleaner way for handling the return of status codes
-	Separate the repository into a readonly for queries and write for commands