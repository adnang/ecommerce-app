# Shopping Cart App

An example shopping cart API and Client, written in ASP.Net Core 2.2.

### Design Decisions
- idempotency at the domain layer: 
  - since a request could be retried by the client, the last response should succeed for a given operation
  - the update method throws is the product isn't already in the basket, since the idempotency key is on the sku and quantity, not the description

### Given extra time, I would
- implement HATEOS to add links to the other available operations
- map request DTOs to the commands
- introduce a command executor layer between the controllers and the domain 
  - the Mediatr library is a possible implementation of this
- implement logging
- return real error responses
- use Swagger Code gen to generate a client library instead of building a bespoke one
  - generating a client with swagger reduces overhead as endpoints are added/updated
- add Swagger documentation