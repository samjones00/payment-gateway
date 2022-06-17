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
