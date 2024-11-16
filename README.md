# User Details
User1@CodePulse.com
User1@123

admin@CodePulse.com
Admin@123

# LaunchSetting.json?
Application launch profile related details will be there like https and http and IIS config

# Program.cs?
Its an entry point of an application, we register our dependency here, request handling pipeline


# Why we use Seperate DB context for Application and Authentication?
We use AuthDB context to interact with authentication Tables

# Why we Use DTO?
we can avoid Unnecessary Data by mapping domain model to DTO
Uses: Performance, security,versioning


# Dependency Injection :
Instead of creating instance of class within a controller we will pass object as a parameter within the controller through constructor

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