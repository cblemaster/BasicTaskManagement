# BasicTaskManagement

## About:
- A .NET/C# application that provides task management functionality, nothing fancy, just the basics.

### Built with:
- .NET 8 / C# 12
- SQL Server database
- API: ASP.NET Core, minimal API, Entity Framework Core 8
- UI: .NET MAUI (targeting Windows only)
- Programming techniques:
	- Expression bodied members
	- Pattern matching
	- Asynchronous programming
	- Dependency injection
	- MVVM pattern

## Features:
- View task items by task group
- View all task items marked important
- View all task items due today
- View all completed task items
- Filter views of task items to show or hide completed task items
- Create, rename, and delete (if no task items) task groups
- Create, update (including changing task group), and delete task items

## Business rules:
- Task items must belong to a task group, but task groups do not have to contain task items
- Completed task items can be deleted, but they cannot be updated, they cannot be made incomplete
- Task items can be due in the past
- Task items do not require a due date

## UI conventions:
- Grouped task items are displayed with the task group names sorted ascending (favorite task groups then sorting to top), then the task items in each task group sorted by due date descending
- Collections of task items are displayed sorted by due date descending
- Important task items have their name displayed in a bold font
- Completed task items have their name displayed in a strikethrough font

## Instructions for running the application:
- Note that SQL server and Visual Studio are required for running the application
- Clone or download the repo
- Browse to \BasicTaskManagement\Database
- Run the database script 'BasicTaskManagement-Create-DB-And-Initialize-Sample-Data.sql'
	- This script will drop (if exists) and re-create the database and tables
	- Note that there is optional sample data that can be inserted into the database as well; it can be found at the end of the database script and it is commented out
- Browse to \BasicTaskManagement\BasicTaskManagement.API\appsettings.json
	- There is a database connection string in this config file that needs to point to your database
- Run the solution in Visual Studio

## Improvement opportunities:
- Keep this up to date with the latest .NET LTS releases
