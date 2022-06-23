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

## Getting Started
-----------------

### Running the applications

1. Start the mock bank

    ```
    > .\payment-gateway
    dotnet run --project .\MockBank\PaymentGateway.AcquiringBank.MockCKOBank
    ```
2. You can run the API from Visual Studio, ensuring that `PaymentGateway.Api` is set as the startup project.

    Alternatively, from the command line, in another terminal window:

    ```
    > .\payment-gateway
    dotnet run --project .\Api\PaymentGateway.Api\
    ```

### Accessing the API

* Use Swagger UI from `https://localhost:7196/swagger`
* Use Postman, importing the url `https://localhost:7196/swagger/v1/swagger.json`, requires setting the `baseUrl` variable.

Authenticate using a bearer token below, click Authorize in Swagger or add the `Bearer <tokenhere>` authentication header. Below are some pre-generated tokens, without these you will receive a **`401 Unauthorized`** response.

Merchant: Apple

```
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFwcGxlIn0.TLGIHiqFuAbM7cVIJ3ZKVQ3dLi9YSzLE2BYVRqKqPhk
```

Merchant: Amazon
```
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFtYXpvbiJ9.3u77zp-pqHJdV79Lu92OxxzD6GaQ4gJK1YI_QKETA6g
```

# Requests & Responses

**`400 Bad Request`**

When specifying `ba1c9df4-001e-4922-9efa-488b59850bc4` as the payment reference and returning an unsuccessful response.

Request
```json
{
  "paymentReference": "ba1c9df4-001e-4922-9efa-488b59850bc4",
  "amount": 12.34,
  "currency": "gbp",
  "cardNumber": "4111111111111111",
  "cvv": "123",
  "expiryDateYear": 2022,
  "expiryDateMonth": 12,
  "cardHolder": "s jones"
}
```
Response
```json
{
    "processedOn": "2022-06-23T16:55:18.5857683Z",
    "paymentStatus": "Unsuccessful",
    "paymentReference": "ba1c9df4-001e-4922-9efa-488b59850bc4"
}
```

**`201 Created`**

When specifying any other payment reference than above and returning a successful response.

Request
```json
{
  "paymentReference": "ba2c9df4-001e-4922-9efa-488b59856bd2",
  "amount": 12.34,
  "currency": "gbp",
  "cardNumber": "4111111111111111",
  "cvv": "123",
  "expiryDateYear": 2022,
  "expiryDateMonth": 12,
  "cardHolder": "s jones"
}
```
Response
```json
{
    "processedOn": "2022-06-23T16:53:29.6963308Z",
    "paymentStatus": "Successful",
    "paymentReference": "ba2c9df4-001e-4922-9efa-488b59856bd2"
}
```

**`409 Conflict`**

When submitting a payment that already exists.

Request
```json
Any request with a previously used payment reference.
```
Response
```json
"Payment already exists."
```

**`504 - Gateway Timeout`**

When the mock bank is unavailable after multiple retries.

Request
```
Any request
```
Response
```json
{
    "status": 504,
    "traceId": "00-c1c0d397f0ed8e835fdd39a3ea644ba2-5660b5e9909df082-00"
}
```

# Testing

## Unit tests

Unit tests can be run from Visual Studio, or using the command line:

```
> .\payment-gateway
dotnet test
```

## Integration Tests

The integration tests are configured to run in two ways. By default they run against a bank stub, however you can also run them against the mock bank application over a localhost port, ideal for testing against hosted instances. 

The payment gateway url used by acceptance tests is configured in `payment-gateway\Tests\PaymentGateway.Api.IntegrationTests\acceptance.runsettings`. 

**Running against the bank stub**

This method will run the acceptance tests against the HttpClient provided by `WebApplicationFactory`, which then uses a `BankConnectorServiceStub` and doesn't make any real calls to the `PaymentGateway.AcquiringBank.MockCKOBank`.

1. Run the payment gateway
```
> .\payment-gateway
dotnet run  --project .\Api\PaymentGateway.Api\
```
2. And in another terminal window, run the tests:
```
> .\payment-gateway
cd .\Tests\PaymentGateway.Api.IntegrationTests
dotnet test
```

**Running against the mock bank application**

This method makes the acceptance test call the application gateway over http, which then calls the mock bank application.

1. Run the mock bank
```
> .\payment-gateway
dotnet run  --project .\MockBank\PaymentGateway.AcquiringBank.MockCKOBank
```
2. In another terminal window, run the payment gateway:
```
> .\payment-gateway
dotnet run  --project .\Api\PaymentGateway.Api\
```
3. And in another terminal window, run the tests:
```
> .\payment-gateway
cd .\Tests\PaymentGateway.Api.IntegrationTests
dotnet test --%  -- TestRunParameters.Parameter(name=\"UseInMemoryHttpClient\", value=\"false\")
```

# Implementation Details

## JSON Web Tokens

Authentication is handled using JSON Web Tokens but due to time constraints and requirements I haven't provided a JWT issuer. However, pre-generated bearer tokens which will authenticate and identify the request can be found under [Accessing the API](#Accessing-the-API).

The JWT identifer is used to identify the merchant reference.

Features:
- A valid bearer token must be provided to perform any action
- Merchants can only access payment details submitted by themselves

## API Routing

I first implemented routes using `MapPost` and `MapGet` using the Minimal API format, but then I found this in the [fluent validation documentation](https://docs.fluentvalidation.net/en/latest/aspnet.html).

> Note that Minimal APIs that are part of .NET 6 don’t support automatic validation.

I then re-wrote the routing in the Controller style as I wanted the request validation to be handled via the middleware.

All routes are defined in `PaymentGateway.Domain.Constants.ApiRoutes` for maintainability.

## Acquiring Bank Connector

All code and configuration for calling the `PaymentGateway.AcquiringBank.MockCKOBank` is separated into its own `PaymentGateway.AcquiringBank.CKO` project, and is only referenced from the `PaymentGateway.Api` project using startup extensions.

Benefits are:
- Easy swapping out for other bank implementations
- Replacing the mock with a real world bank or (vice versa) with minimal changes
- A step closer for separating the acquiring banks into their own nuget packages for use in other applications.
- Self contained including the configuration

## Mediator

I've used mediator to handle the command and query handling, in this case it's mainly used for decoupling as i'm not taking advantage of the notification benefits. I could have built on this using CQRS, but I didn't think the benefits were justifiable.

## The Mock Acquiring Bank

The mock bank (`.\MockBank\PaymentGateway.AcquiringBank.MockCKOBank`) is a very simple aspnet core 6 Web API project which returns either successful or unsuccessful responses depending on the payment reference. 

Given more time I would write this to be more configurable to use json files a request/response templates.

## Persistance

The payments are stored and updated using an in-memory cache, but could be swapped out for another implementation of `IRepository`.

## Data Security

I thought about adding an IRepository decorator, encrypting on add/update and decrypting on read. I decided against this as it was "out of scope", but i'm not happy with storing card details in plain text NoSql database unless there were access restrictions or some other mechanism in place.

## Mapping

AutoMapper is used throughout, the command/query/domain model mapping is found in `PaymentGateway.Domain` and the acquiring bank mapping profiles are part of `PaymentGateway.AcquiringBank.CKO`.

```mermaid
sequenceDiagram
    autonumber
    participant Merchant as Merchant 
    participant Gateway as Payment Gateway
    participant Bank as Bank
    Merchant->>Gateway: Command mapped to Payment Domain Model
    Gateway->>Bank: Payment Domain Model mapped to Bank Request Model
    Bank->>Gateway: Bank Response returned and Payment model updated 
    Gateway->>Merchant: Payment Domain Model mapped to Payment Response 

```

# Extra mile bonus points

- JWT Authentication
- Retry policy with delay when calling the bank endpoint
- Encapsulation of the bank connector, allowing easy replacement

# Next Steps

- Host the API behind an API management portal which would handle the authentication, rate limiting, other transient faults, etc
- Replace the In-Memory cache with a database, either using SQL Server with column-level encryption or NoSQL and access restrictions.
- Add logging, both local plain text logging using serilog and a hosted platform such as Application Insights
- Add telemetry to track request/response times and performance