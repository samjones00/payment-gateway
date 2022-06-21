[![.NET](https://github.com/samjones00/payment-gateway/actions/workflows/dotnet.yml/badge.svg)](https://github.com/samjones00/payment-gateway/actions/workflows/dotnet.yml)


# Overview

The objective is to accept payments from the merchant, pass the request through different stages and return a response describing the outcome. Below is a high level flow of the process.

```mermaid
sequenceDiagram
    autonumber
    participant Merchant
    participant PG as Payment Gateway 
    participant DB as Database
    participant Bank as Mock CKO Bank
    Merchant->>PG: POST Payment Request
    PG-->>DB: Save to Database
    PG->>Merchant: Return Created Status (201)

    PG->>Bank: POST HTTP Request 
    Bank->>Bank: Retry If Unavailable     
    Bank->>PG: Return Response
    PG-->>DB: Update Database    
    PG->>Merchant: Return Details (200)
```

## Requirements

- Dotnet 6 SDK
- Docker
- Docker file sharing permissions allowing the `docker-compose` file in `.\MockBank` access to mount a volume. 

## Getting Started
-----------------

1. ### Start the mock bank

    ```
    cd .\MockBank
    docker-compose up
    ```
2. ### Start the application

    ```
    cd .\Api\PaymentGateway.Api
    dotnet run
    ```
3. ### Accessing the API

* Use Swagger UI from `https://localhost:7196/swagger`
* Use Postman, importing the url `https://localhost:7196/swagger/v1/swagger.json`

4. ### Authenticate using a bearer token
    Copy one the JWT's from the [JSON-Web-Tokens](#JSON-Web-Tokens) section


### Example bank responses

| Payment Reference | Response |
|-|-|
|ba1c9df4-001e-4922-9efa-488b59850bc4 | Unsuccessful
|Any other guid | Successful


## JSON Web Tokens
--------
Authentication and authorization is handled using JSON Web Tokens but due to time constraints and requirements I haven't provided a JWT issuer. However, below are some pre-generated bearer tokens which will authenticate and identify the request.

## Features
- A valid bearer token must be provided to perform any action
- Merchants can only access payment details submitted by themselves

#### Merchant: Apple

```
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFwcGxlIn0.TLGIHiqFuAbM7cVIJ3ZKVQ3dLi9YSzLE2BYVRqKqPhk
```

#### Merchant: Amazon
```
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFtYXpvbiJ9.3u77zp-pqHJdV79Lu92OxxzD6GaQ4gJK1YI_QKETA6g
```

# Testing

## Unit tests

Unit tests are best run through the command line, as running `dotnet test` from the command line would include integration tests as well.

## Integration Tests

Integration tests can be run from Visual Studio, whether they run against a hosted/running payment gateway application or an in-memory one is configured in `payment-gateway\Tests\PaymentGateway.Api.IntegrationTests\acceptance.runsettings`. 

> Ensure that the mock bank is running, see [Getting Started](#Getting-started):

To run the integration tests, you can either run against the running application:

1. Open a terminal window to run the payment gateway:

1. `CD payment-gateway\Api\PaymentGateway.Api`

1. `dotnet run`

1. Open another terminal window to run the acceptance tests:

1. `CD payment-gateway\Tests\PaymentGateway.Api.IntegrationTests`

1. Run `dotnet test` with a parameter, this will allow the tests to run over the localhost address:  
    **cmd**  
    `dotnet test  -- TestRunParameters.Parameter(name=\"UseInMemoryHttpClient\", value=\"false\")`

    **powershell**  
    `dotnet test --%  -- TestRunParameters.Parameter(name=\"UseInMemoryHttpClient\", value=\"false\") `

    **bash**  
    `dotnet test -- TestRunParameters.Parameter\(name=\"UseInMemoryHttpClient\",\ value=\"false\"\)`

Or run against the in-memory application

1. Open a terminal window

1. `CD payment-gateway\Tests\PaymentGateway.Api.IntegrationTests`

1. dotnet test

# Implementation Details

## API Routing

I first implemented routes using `MapPost` or `MapGet` using the minimal api format, but then I found this in the  [fluent validation documentation](https://docs.fluentvalidation.net/en/latest/aspnet.html).

> Note that Minimal APIs that are part of .NET 6 don’t support automatic validation.

I then re-wrote the routing in the Controller style.

## Connecting to the Acquiring Bank 

All code and configuration for calling the mock CKO bank is separated into its own `PaymentGateway.AcquiringBank.CKO` project, and is only referenced from the `PaymentGateway.Api` project using startup extensions.

Benefits are:
- Easy swapping out for other bank implementations
- Replacing the mock with a real world bank or (vice versa) with minimal changes
- A step closer for separating the acquiring banks into their own nuget packages for use in other applications.
- Self contained including the configuration

## The Mock Acquiring Bank

I'm using a pre-built docker image called [mockaco](https://github.com/natenho/Mockaco/) which allows you to describe the response you want to return given a specific request.

## Persistance

The payments are stored and updated using an in-memory cache, but could be swapped out for another implementation of `IRepository`.

## Data security

I thought about adding an IRepository decorator, encrypting on add/update and decrypting on read. I decided against this as it was "out of scope", but i'm not happy with storing card details in plain text NoSql database unless there were access restrictions or some other mechanism.

## Mapping

AutoMapper is used throughout, the command/query/domain model mapping is found in `PaymentGateway.Domain` and any acquiring bank mapping profiles are part of `PaymentGateway.AcquiringBank.CKO`.

```mermaid
sequenceDiagram
    autonumber
    participant Merchant as Merchant 
    participant Gateway as Payment Gateway
    participant Bank as Bank
    Merchant->>Gateway: Command mapped to Payment Domain Model
    Gateway->>Bank: Payment Domain Model mapped to Bank Request Model
    Bank->>Gateway: Bank Response Model mapped to 
    Gateway->>Merchant: Payment Domain Model mapped to Payment Response 

```

# Extra mile bonus points

- JWT Authentication and authorization
- Retry policy with delay when calling the bank endpoint
- Encapsulation of the bank connector, allowing easy replacement


# Next Steps

- Host the API behind an API management portal which would handle the authentication, rate limiting, etc
- Replace the In-Memory cache with a database, either using SQL Server with column-level encryption or NoSQL and access restrictions.
- Add logging, both local plain text logging and a hosted platform such as Application Insights