# .NET Core Web API for API Development Challenge for HCA 

    - .NET Core 5.0.100
    - Added Docker support for both SQL Server and In Memory Database
    - For SQl Server, run the docker-compose project with modified code in Startup.CS file
    - For in memory DB, run LabTests project (Docker, or Regular)  

## Assumptions Made
    - This Application acts as central API for several client applications

## Design Choice
    - Used MVVM architecture
    - Application designed with Code-First approach for easy migrations
    - All dates are rendered in the default format

## Packages used
    - Entity Framework core
    - SQlite
    - SqlServer
    - Swagger
    
# any complexities can be expected ?
If no docker is installed on the machine, please run in Debug mode.



