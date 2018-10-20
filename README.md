# Simple IOT Service with .Net Core 2 Web API - Best Practice

This project is a simple IOT service that you can create your custom service with its fields and properties, define devices and authorize them to send data via URL or JSON.

# Features
* EF with PostgreSql DB (Although it may seem crazy but it was cool!)
* JWT with Role based Authentication
* AutoMapper
* FluentValidation
* DTO transfer model

# How to run
.Net Core is cross-platform, I developed this project on Linux but steeps are same.
1. install `.Net Core 2 SDK` , `PostgreSQL` , and maybe PostgreSQL GUI like `pgAdmin` or `DBeaver` in your system.
2. Restore PostgreSQL DB backup(`DB/IOT_db.backup`)
3. Clone the project
4. Set connection string in appsetting.json
5. Run `dotnet run` in the project folder or open with Visual Studio and run it.

# Run test
1. Install [Postman](https://www.getpostman.com/)
2. Import test collection in `Test/IOT.postman_collection.json`.

# TODO
- [x] Hash Passwords
- [ ] React Client
- [ ] API Versioning
- [ ] Write Unit tests

**Meisam Malekzadeh**
