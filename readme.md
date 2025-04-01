# Project Overview (DeveloperStore)

This system is designed to manage sales operations for a store, including handling customers, products, branches, and sales transactions. It provides functionality for creating, updating, and managing sales, applying discounts, and tracking domain events for business processes.

It includes features such as:
- Managing customers, products, and sales.
- Handling sales transactions with discounts and item management.
- Publishing and tracking domain events for system consistency.

## Key Technologies

- .NET 8.0
- Domain-Driven Design (DDD)
- Entity Framework Core
- AutoMapper
- FluentValidation
- MediatR
- Npgsql (PostgreSQL)
- xUnit (for testing)
- NSubstitute (for mocking in tests)
- Testcontainers (for integrations tests)


## How to Test

In the root folder, you can run the commands below to execute unit tests and integration tests.
>Note: Docker must be installed to run the integration tests. 
```
--Unit Tests
dotnet test tests/UnitTests/UnitTests.csproj

--Integrations Tests
dotnet test tests/IntegrationTests/IntegrationTests.csproj 
```

## Additional Information About the Project


### Architecture

This project uses a layered architecture to promote separation of concerns and facilitate system maintenance and evolution. The code structure is organized into four main layers:

1. **Api**: Responsible for exposing the application's endpoints and handling HTTP requests. It contains controllers, middleware, and API-specific configurations.
2. **Application**: Contains the application logic, including use cases, validations, and data manipulation before interacting with the lower layers.
3. **Domain**: Represents the core of the system, where business concepts, entities, domain events, exceptions, and business rules are defined.
4. **Infrastructure**: Implements technical details such as database access, integration with external services, and specific infrastructure configurations.

This approach ensures that each layer has a well-defined responsibility, reducing coupling and increasing cohesion. Additionally, it facilitates replacing or modifying a layer without directly impacting others, promoting a more flexible and sustainable architecture.

### Api

The **Api** layer is responsible for exposing the application's endpoints and handling HTTP requests. It acts as the entry point for external clients, such as web or mobile applications, to interact with the system. This layer includes controllers, middleware, and configurations specific to the API.

#### Why MediatR?

MediatR is used in the **Api** layer to promote a clean and decoupled architecture. It helps in implementing the mediator pattern, which centralizes the communication between different parts of the application. By using MediatR, the **Api** layer delegates the execution of business logic to the **Application** layer through commands and queries, ensuring that controllers remain thin and focused on handling HTTP requests and responses.

Additionally, MediatR simplifies the implementation of cross-cutting concerns, such as validation and logging, by leveraging pipeline behaviors. This approach enhances maintainability and testability by reducing dependencies and coupling between components.

#### Middleware - Error Handling

The `ErrorHandlingMiddleware` centralizes exception handling, ensuring consistent error responses and logging. It captures exceptions, logs them appropriately, and returns structured JSON responses with relevant HTTP status codes, improving maintainability and user experience.

## Application Layer Overview

The **Application** layer is responsible for orchestrating business use cases and coordinating interactions between the **Api** and **Domain** layers. It acts as a bridge, ensuring that the business logic defined in the **Domain** layer is executed correctly and that the results are returned to the **Api** layer.

This layer includes:
- **Commands and Queries**: Represent specific operations or requests, such as creating a customer or retrieving a list of products.
- **Handlers**: Implement the logic for processing commands and queries, often delegating tasks to the **Domain** layer.
- **Validation**: Ensures that input data meets the required criteria before processing, using tools like FluentValidation.
- **Mapping**: Transforms data between different representations, such as DTOs and domain entities, using AutoMapper.

By centralizing application logic in this layer, the system achieves better separation of concerns, making it easier to maintain, test, and extend.


## Domain Layer Overview

The **Domain** layer represents the core of the system, encapsulating the business logic, rules, and concepts. It is designed following the principles of Domain-Driven Design (DDD) to ensure that the software accurately reflects the business domain.

### Architecture and SOLID Principles

The **Domain** layer adheres to a clean architecture approach, ensuring that it remains independent of external frameworks and technologies. This independence allows the domain to evolve without being tightly coupled to infrastructure or application-specific concerns.

Key SOLID principles applied in this layer include:

1. **Single Responsibility Principle (SRP)**: Each class in the domain has a single responsibility. For example:
    - Entities like `Sale`, `Customer`, and `Product` encapsulate only the business logic relevant to their respective concepts.
    - Value objects like `Money` and `Discount` represent immutable domain concepts with well-defined behaviors.

2. **Open/Closed Principle (OCP)**: The domain is designed to be open for extension but closed for modification. For instance:
    - New domain events or value objects can be added without altering existing classes.
    - Business rules are encapsulated in methods, allowing for extension without modifying the core logic.

3. **Liskov Substitution Principle (LSP)**: Abstract base classes like `Entity` and `ValueObject` ensure that derived classes can be used interchangeably without breaking the application.

4. **Interface Segregation Principle (ISP)**: Interfaces like `IAggregateRoot` and `IDomainEvent` are small and focused, ensuring that implementing classes are not burdened with unnecessary methods.

5. **Dependency Inversion Principle (DIP)**: The domain depends on abstractions rather than concrete implementations. For example:
    - Repositories like `IBranchRepository` and `IProductRepository` define contracts for data access, leaving the implementation to the infrastructure layer.
    - The `IUnitOfWork` interface abstracts transaction management, ensuring that the domain logic remains decoupled from persistence concerns.

By adhering to these principles, the **Domain** layer achieves high cohesion, low coupling, and a clear separation of concerns, making it robust, maintainable, and adaptable to future changes.

## Infrastructure Layer Overview

The **Infrastructure** layer is responsible for implementing technical details such as database access, integration with external services, and specific configurations. It serves as the foundation for the system's operational needs, ensuring that the application can interact with external dependencies effectively.

### Key Technologies and Practices

1. **Entity Framework Core**: Used as the ORM for database interactions, enabling efficient data access and manipulation.
2. **Npgsql**: Provides PostgreSQL database connectivity, ensuring robust and scalable data storage.
3. **Repository Pattern**: Implements repositories like `BranchRepository`, `ProductRepository`, and `CustomerRepository` to encapsulate data access logic and promote separation of concerns.
4. **Unit of Work**: Ensures transactional consistency by coordinating changes across multiple repositories.
5. **Migrations**: Manages database schema evolution using Entity Framework migrations, with seed data for initial setup.
6. **Extension Methods**: Includes reusable utilities like `QueryableExtensions` to enhance LINQ queries.
7. **Layered Architecture**: Adheres to a clean separation of concerns, ensuring that the **Infrastructure** layer remains focused on technical implementations without leaking into business logic.

By leveraging these technologies and practices, the **Infrastructure** layer ensures maintainability, scalability, and a clear boundary between the application and its external dependencies.


## UnitTests

The **UnitTests** project is designed to validate the behavior of individual components in isolation, ensuring that each unit of the system performs as expected. By focusing on small, isolated pieces of functionality, unit tests provide fast feedback and help maintain code quality throughout the development lifecycle.

### Best Practices for Unit Testing

1. **Arrange-Act-Assert (AAA) Pattern**: Tests are structured into three clear sections:
    - **Arrange**: Set up the necessary objects and state.
    - **Act**: Perform the action being tested.
    - **Assert**: Verify the expected outcome.

2. **Single Responsibility**: Each test should focus on a single behavior or scenario, making it easier to identify issues when a test fails.

3. **Readable and Descriptive Names**: Test method names should clearly describe the scenario being tested, e.g., `Should_ApplyDiscount_When_ValidCouponIsProvided`.

4. **Mocking Dependencies**: Use mocking frameworks like **NSubstitute** to isolate the unit under test by replacing dependencies with controlled mock objects.

5. **Avoid Testing Implementation Details**: Focus on testing the behavior and outcomes rather than internal implementation, ensuring tests remain resilient to refactoring.

### NSubstitute for Mocking

**NSubstitute** is used to create mock objects for dependencies, enabling the isolation of the unit under test. It simplifies the creation of substitutes for interfaces and abstract classes, allowing developers to:

- Simulate specific behaviors or return values for methods.
- Verify that certain methods were called with expected arguments.
- Handle edge cases by simulating exceptions or unexpected inputs.

For example:
```csharp
var mockRepository = Substitute.For<IProductRepository>();
mockRepository.GetById(Arg.Any<Guid>()).Returns(new Product { Id = Guid.NewGuid(), Name = "Test Product" });
```

This approach ensures that tests remain focused on the unit's logic without being affected by external dependencies.

### Coverage and Assertions

The unit tests leverage **xUnit** for test execution and assertions. Key features include:
- **Parameterized Tests**: Simplify testing multiple scenarios with different inputs and expected outcomes.
- **Custom Assertions**: Enhance readability and reusability by encapsulating common validation logic.

### Benefits of Unit Testing

- **Early Bug Detection**: Catch issues early in the development process, reducing the cost of fixing defects.
- **Refactoring Confidence**: Ensure that changes to the codebase do not introduce regressions.
- **Documentation**: Serve as living documentation for the expected behavior of the system.

By adhering to these practices and leveraging tools like **NSubstitute** and **xUnit**, the unit tests in this project ensure a robust and maintainable codebase, fostering confidence in the system's reliability.


## Integration Tests

The **IntegrationTests** project is designed to validate the interaction between multiple components of the system, ensuring that they work together as expected. These tests go beyond unit tests by verifying the behavior of the system in a more realistic environment, including database interactions, API endpoints, and external dependencies.

### Best Practices for Integration Testing

1. **Realistic Environment**: Use a setup that closely resembles the production environment, including the same database type, configurations, and external services.
2. **Isolated Tests**: Ensure that each test is independent and does not rely on the state left by other tests. Use tools like Testcontainers to create isolated environments for each test run.
3. **Clear Test Scenarios**: Focus on testing end-to-end workflows and critical integration points, such as API endpoints, database queries, and external service calls.
4. **Data Cleanup**: Ensure that test data is cleaned up after each test to maintain a consistent environment.
5. **Error Handling**: Test edge cases and error scenarios to validate the system's robustness and resilience.

### Testcontainers for Integration Testing

The **IntegrationTests** project leverages **Testcontainers** to create lightweight, disposable containers for testing. This approach ensures that tests are executed in a consistent and isolated environment, reducing the risk of flaky tests caused by external dependencies.

#### Why Testcontainers?

1. **Isolation**: Each test runs in its own containerized environment, ensuring that tests do not interfere with each other.
2. **Consistency**: Containers provide a consistent environment across different machines, eliminating issues caused by differences in local setups.
3. **Ease of Use**: Testcontainers simplifies the setup and teardown of containers, allowing developers to focus on writing tests rather than managing infrastructure.
4. **Realistic Testing**: By using the same database and services as production, Testcontainers ensures that tests are more reliable and reflective of real-world scenarios.

#### Example: PostgreSQL Container

In this project, Testcontainers is used to spin up a PostgreSQL container for database integration tests. The `TestFixture` class initializes the container, configures the connection string, and ensures that the database is ready for testing.

```csharp
PostgreSqlContainer = new PostgreSqlBuilder()
    .WithDatabase("developerstore")
    .WithUsername("user")
    .WithPassword("password")
    .Build();

PostgreSqlContainer.StartAsync().GetAwaiter().GetResult();
ConnectionString = PostgreSqlContainer.GetConnectionString();
```

This setup ensures that the database is isolated and reset for each test run, providing a clean slate for testing.

### Coverage and Assertions

The integration tests use **xUnit** for test execution and **FluentAssertions** for assertions. Key features include:
- **Readable Assertions**: FluentAssertions provides a natural language syntax for expressing expectations, making tests easier to read and understand.
- **End-to-End Scenarios**: Tests cover complete workflows, such as creating a product, updating it, and verifying the changes through API endpoints.

### Benefits of Integration Testing

- **Confidence in System Behavior**: Validate that all components work together as expected, reducing the risk of integration issues in production.
- **Early Detection of Issues**: Catch problems related to database queries, API contracts, and external dependencies early in the development process.
- **Improved Quality**: Ensure that the system meets functional and non-functional requirements, such as performance and reliability.

By following these best practices and leveraging tools like **Testcontainers**, the **IntegrationTests** project ensures that the system is robust, reliable, and ready for production use.
