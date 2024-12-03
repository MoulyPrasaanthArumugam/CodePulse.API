# User Details
admin@CodePulse.com
Admin@123

# LaunchSetting.json?
Application launch profile related details will be there like https and http and IIS config

# Program.cs?
Its an entry point of an application, we register our dependency here, request handling pipeline


# Why we use Seperate DB context for Application and Authentication?
We use AuthDB context to interact with authentication Tables

# Why we Use DTO?
we can avoid Unnecessary Data by mapping domain model to DTO (View Models)
Uses: Performance, security,versioning

#AutoMapper
AutoMapper can be used for Object to Object Mapping (DTO - Domain Model Mapping). Avoid Code Reduntancy.

# Dependency Injection :
Instead of creating instance of class within a controller we will pass object as a parameter within the controller through constructor

Dependency Injection Options
AddScoped: The current usage is for creating services with a per-request lifecycle (the same instance is used within one HTTP request).
AddSingleton: Use for services that need a single instance throughout the application's lifetime (e.g., configuration or cache).
AddTransient: Use for lightweight, stateless services that are created each time they are requested.

services.AddScoped<IWatchlistRepository, WatchlistRepository>();
services.AddSingleton<ICacheService, MemoryCacheService>();
services.AddTransient<IEmailService, EmailService>();


# Repository pattern
we won't expose the DBcontext to controller we will use Repository to insert , update delete
Design Pattern to seperate data access layer from application
it's a layer of Abstraction
Uses :
Decoupling the data access layer from application
Consistency
Performance
Multiple Data sources

# Migrating and Updating DB
Add-Migration "Name of Migration"
Update-Database

ex)
EntityFrameworkCore\Add-Migration Initial -Context ApplicationDBContext
EntityFrameworkCore\Update-Database -Context ApplicationDBContext
EntityFrameworkCore\Remove-Migration -Context ApplicationDBContext



# Modifying FK Constraints:

ALTER TABLE [dbo].[Like] 
DROP CONSTRAINT [FK_Like_IdentityUser_UserId];

ALTER TABLE [dbo].[Like]
WITH CHECK ADD CONSTRAINT [FK_Like_IdentityUser_UserId]
FOREIGN KEY ([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE;


# ModelState
ModelState is primarily used to track and handle validation errors during model binding. It ensures that any issues—such as missing required fields,
invalid data types, or violations of validation attributes—are captured. By leveraging ModelState, we can provide customized and user-friendly error
responses when errors occur during model binding or other validation scenarios.

# Logging
*primarily we can use default ILogger to Log but to get more flexibility we can use 3rd party like SeriLog/Log4Net

*Serilog is a popular logging framework for ASP.NET Core that provides a flexible and powerful way to capture and store log data.
*It is a third party library that can be easily integrated with an ASP.NET Core Web API.

# Serilog Provides more flexibility like managing diferent log levels in both console and File.
new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
    .WriteTo.File("logs/log.txt", restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateLogger();


# Log levels
Level		Priority	Description
Trace		1			Very fine-grained details, typically not enabled in production.
Debug		2			Used during development for diagnostic purposes.
Information	3			General application flow and operational information.
Warning		4			Potential issues or unexpected events that don't stop the application.
Error		5			Recoverable failures or issues requiring immediate investigation.
Critical	6			Catastrophic failures requiring urgent attention.

# We can use Log levels to limit the type of logs 
for ex) if we set loglevel to Debug (2) , we can get all levels of logs from 2 - 6 skippin log level 1


# Global Exception Handling
Global Exception Handling is been used to centralize handling of any Unhandled Exceptions to create consistent Error Response 
and to Avoid Repetition like wrapping every action method within try catch blocks.

Here we have configured Custom Middleware Named "ExceptionHandlerMiddleware" to handle Internal server errors.


