# TimeSheetApp API
Model project to demonstrate all the "good implementations"

#### How to build the project

- To setup a database containder `docker compose -f .\docker-compose.yml up`
- Run the Api Project
- Go here in you browser `https://localhost:5001/swagger/`

#### Certical slicing

This project is useing a vertical slicing apporoach.
All Concerns are stored in the `Concerns/ConcernName` folders.
**!!Very important!!** Concern name must be plural, while domain object must be singular.
Ex.: Concerns/User**s** while the object inside must be `User.cs`, __to avoid Namespacing problems__


#### To Do List

- ✅ Dependency Injection
- ✅ Serilog Logger + Rotating File
- ✅ Unit tests configuration
- ✅ Database with docker
- ✅ GlobalExceptionHandler
- ✅ Usage of a 3d Party API `(Typicode https://jsonplaceholder.typicode.com/)`
- ✅ Vertical Sliced Concerns
- 🔲 Validations
- 🔲 MediatR
- 🔲 JWT authentication
- 🔲 React Frontend
- 🔲 Frontend with docker
- 🔲 Integration tests
- 🔲 Use Refit to calls other apis
- 🔲 Health check route returning App version build number and workstation information