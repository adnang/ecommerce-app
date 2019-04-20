# Shopping Cart App

An example shopping cart API and Client, written in ASP.Net Core 2.2.

Given extra time, I would
- map request DTOs to the commands
- introduce a command executor layer between the controllers and the domain 
  - the Mediatr library is a possible implementation of this
- implement logging
- return real error responses
- use Swagger Code gen to generate a client library instead of building a bespoke one
  - generating a client with swagger reduces overhead as endpoints are added/updated
- add Swagger documentation