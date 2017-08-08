# Simple.Migrations.ConsoleRunner
A console runner for https://github.com/canton7/Simple.Migrations

Changes can be rolled forwards or backwards, simply specify the version and the runner will decide based on the current version of the database.

# Limitations
This version currently only supports MS SQL Server
Doesn't support using a baseline version

# Pre-requisites
Requires a dll with migration classes. See the sample migrations project in this repo https://github.com/barker1889/Simple.Migrations.ConsoleRunner/tree/master/SampleMigrations.

Requires the database to be created. I'd suggest specifying the name of the database in the connection string rather than in the migrations themselves.

# Basic usage
*To be updated with more detail and examples, but for now...*

Either compile the runner from source or add it via nuget - https://www.nuget.org/packages/Simple.Migrations.ConsoleRunner/1.0.0

Run via a command line. The command line accepts the following parameters
* --connection-string: SQL Server connection string e.g "Server=.;Trusted_Connection=True;Database=MyTestDb;"
* --version: The migration number
* --mode: Either NoOp or Apply
  * *"noop"* will just display the changes. No migrations will be applied. A version table will be created if it doesn't exist
  * *"apply"* will actually run the migrations
* --migrations: This is the path to the dll that contains the migrations. It can be absolute or relative.
* --version-schema: (optional) The schema to place the version history table in
