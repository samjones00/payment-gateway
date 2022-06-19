[![.NET](https://github.com/samjones00/payment-gateway/actions/workflows/dotnet.yml/badge.svg)](https://github.com/samjones00/payment-gateway/actions/workflows/dotnet.yml)


## Overview
--------------
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
    PG->>Merchant: Return accepted  Status (202)

    PG->>Bank: POST HTTP Request 
    Bank->>Bank: Retry If Unavailable     
    Bank->>PG: Return Response
    PG-->>DB: Update Database    
    PG->>Merchant: Return Details (200)
```

## Getting started
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
    Copy one the JWT's from the [JSON-Web-Tokens](#JSON-Web-Tokens) section

* Use Swagger UI from https://localhost:[portnumber]/swagger
* Use Postman, importing the url https://localhost:[portnumber]/swagger/v1/swagger.json

4. ### Authenticate using a bearer token


### Example bank responses

| Payment Reference | Response |
|-|-|
|ba1c9df4-001e-4922-9efa-488b59850bc4 | Unsuccessful
|Any other guid | Successful


## JSON Web Tokens
------------------
Authentication and authorization is handled using JSON Web Tokens but due to time constraints and requirements I haven't provided a JWT issuer. However, below are some pre-generated bearer tokens which will authenticate and identify the request.

## Features
- A valid bearer token must be provided to perform any action
- Merchants can only access payment details submitted by themselves

#### Merchant: Netflix

```
eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsImlhdCI6MTY1NTU4MjkxMCwiZXhwIjoxNjg3MTE4OTEwLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MTM4LyIsInN1YiI6IkFwcGxlIn0.TLGIHiqFuAbM7cVIJ3ZKVQ3dLi9YSzLE2BYVRqKqPhk
```
#### Merchant: Disney Plus
```
token here
```

## API Routing
-------------
I first implemented routes using `MapPost` or `MapGet` using a, but found that automatic model validation isn't yet available for minimal API dotnet 6. 

> Note that Minimal APIs that are part of .NET 6 don’t support automatic 


See https://docs.fluentvalidation.net/en/latest/aspnet.html for details. I have left thr 
