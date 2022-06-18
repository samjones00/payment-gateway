## Payment Journey 

```mermaid
sequenceDiagram
    autonumber
    participant Merchant
    participant PG as Payment Gateway 
    participant DB as Database
    participant Bank
    Merchant->>PG: Submit Payment
    PG-->>DB: Save to database
    PG->>Merchant: Return accepted  Status (202)

    PG->>Bank: POST HTTP Request 
    Bank->>Bank: Retry If Unavailable     
    Bank->>PG: Return Response
    PG-->>DB: Update database    
    PG->>Merchant: Return Response (200)
```

## Details Lookup

```mermaid
sequenceDiagram
    autonumber
    participant Merchant
    participant PG as Payment Gateway 
    participant DB as Database
    Merchant->>PG: GET Request
    PG-->>DB:Query database
    DB-->>PG:Return database
    PG->>Merchant: Return Details (200)
```


```mermaid
stateDiagram
    direction LR
    [*] --> A
    A --> B
    B --> C
    state B {
      direction LR
      a --> b
    }
    B --> D
```