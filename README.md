# Simple IOT Service with .Net Core 2 Web API 

This project is a simple IOT service that you can create your custom service with its fields and properties, define devices and authorize them to send data via URL or JSON.

# Features
* EF Code First & Migrations
* JWT with Role based Authentication
* AutoMapper
* FluentValidation
* DTO transfer model
* XUnit Tests

# Functionality overview
* CRUD Users (SignUp - Login - Create-Edit-Delete Subusers(devices) )
* CRUD Services
* CRUD Service properties
* CR** Device data (Get data with JSON & Url)
* Get Device Data with rest filters
* Assign properties to service
* Assign users(devices) to service

# How to run
.Net Core is cross-platform, I developed this project on Linux but steeps are same.
1. install `.Net Core 2 SDK` , `SQL Server` , and maybe `SQL Server Management Studio`.
2. Clone the project
3. Set connection string in appsetting.json
4. Run `Update-Database` in Package Manager Console for restoring Database(Test username & password : `BitBird`).
5. Run `dotnet run` in the project folder or open with Visual Studio and run it.

# Run test
1. Install [Postman](https://www.getpostman.com/)
2. Import test collection in `Test/IOT.postman_collection.json`.

# Clients
* My React.js client: [iot-react-client](https://github.com/MyBitBird/iot-react-client) 


# TODO
- [x] React Client 
- [x] Hash Passwords
- [x] Write Unit tests(25%)
- [ ] API Versioning


**Meisam Malekzadeh**
