# Football For All

Football For All is an ASP.NET Core MVC web application for providing information about football matches, different championships, clubs, footballers and more.


On home page of the web app you can find information about upcoming matches:
![Home page](/Screenshots/home_page.PNG)


Users with Admin rights have access to the Admin Panel, where they can create, edit and delete data from the main tables in DB:
![Admin panel](/Screenshots/admin_panel.PNG)

## Technologies

Football For All is an ASP.NET Core MVC monolith web application. It uses a default Identity. Also it uses EntityFramework Core for comunication with MS SQL Server DB, that has following main tables:
![DB Diagram](/Screenshots/db_diagram.png)

Also the project includes stuff like Services, Migrations and Unit tests.

## Usage

Just clone the repo and open it in Visual Studio 2019. Apply migrations - they will create a DB for you (see Program.cs file of FootballForAll.Web project), and run the project. Enjoy!

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## Authors and Maintainers

* Dmitriy Tuma
