## Payment Journey 

```mermaid
sequenceDiagram
    participant Merchant
    participant Gateway
    participant Bank
    Merchant->>Gateway: Submit Payment
    Gateway->>Bank: Send Request 
    Bank->>Gateway: Return Response 
    Gateway->>Merchant: Return Status
    Gateway->>Gateway: Retry If Unavailable 
```

## Details Lookup

```mermaid
sequenceDiagram
    participant Merchant
    participant Gateway
    Merchant->>Gateway: Submit Payment
    Gateway->>Merchant: Return Status
    Gateway->>Gateway: Retry If Unavailable 
```