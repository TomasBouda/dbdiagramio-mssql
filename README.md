# dbdiagram.io dbml generator for MSSQL

> Originally forked from [david-zoltan/dbdiagramio-mssql](https://github.com/david-zoltan/dbdiagramio-mssql)

This app allows you to generate dbml code for [dbdiagram.io](https://dbdiagram.io) from your MS SQL database.

## Installation

* Download `DbDiagramIo.CLI.zip` from [releases](https://github.com/TomasBouda/dbdiagramio-mssql/releases/latest) and extract it.
* Run `DbDiagramIo.CLI.exe` with at least `--connection-string` argument.

## Usage

```
Available arguments:

  -v, --verbosity            (Default: Information) Sets the serilog log level.

  -c, --connection-string    Required. The connection string to the database.

  --exclude-schemas          The schemas to exclude from the dbml output. You can use LIKE syntax here.

  --exclude-tables           The schemas to exclude from the dbml output. You can use LIKE syntax here e.g. 'AspNet%' to
                             exclude all tables starting with 'AspNet'.

  -o, --output-file          The file to write the dbml output to. If not provided, the output will be written to the
                             console.

  --help                     Display this help screen.

  --version                  Display version information.
```

### Example

```
.\DbDiagramIo.CLI.exe -c "Server=.;Database=MyDatabase;Integrated Security=true;" --exclude-schemas "Hangfire;serilog" --exclude-tables "AspNet%" -o "D:\Root\dbml.txt" -v 1
```

## The repository

* DbDiagramIo.CLI: Command line tool to export MS SQL database schema into a script that can be imported into [dbdiagram.io](https://dbdiagram.io)
* DbDiagramIo.MsSql: Library to export MS SQL database schema into a script that [dbdiagram.io](https://dbdiagram.io) understands.
* Example.DotNetCore: .NET Core example application that actually creates the [dbdiagram.io](https://dbdiagram.io)script by using the library.

## Try it

* `cd src` and open the `.sln` file with Visual Studio
* set the connection string in `Example.DotNetCore\Program.cs` to point to your database
* build the solution using Visual Studio
* run the application using `dotnet Example.DotNetCore.dll`
* copy&paste the application's output to https://dbdiagram.io/d
* see your database's diagram

## Create your own application

Use the DbDiagramIo.MsSql library in your code and create a better MS SQL => dbdiagram.io export tool.

### Add the nuget package


```
dotnet add package DbDiagramIo.MsSql
```

### Import the namespace

```
using DbDbiagramIo.MsSql;
```

### Generate dbdiagram.io script

```
(TableDto[] tables, ForeignKeyDto[] foreignKeys) = MsSqlSchemaReader.ReadTablesAndForeignKeysFromDb( "<YOUR-DB'S-CONNECTIONSTRING>" );

foreach (TableDto table in tables)
{
    Console.WriteLine( table.ToDbDbiagramCode() );
}

foreach (ForeignKeyDto fk in foreignKeys)
{
    Console.WriteLine( fk.ToDbDiagramDto() );
}
```

## Why

SQL Server Management Studio 18 no longer supports diagrams. [dbdiagram.io](https://dbdiagram.io) is a free tool to draw DB diagrams, however currently it does not have an "import from ms sql" feature. This project provided a quick & dirty way for me to generate DB diagrams for my MS SQL databases.

Edit 25.1.2024 (Tomas Bouda) - Curently there is mssql import option in [dbdiagram.io](https://dbdiagram.io) but it was not working for me + I needed to filter out some tables/schemas automatically.

## License

Copyright (c) 2019 David Zoltan, Tomas Bouda. Licensed under the [MIT license](LICENSE).
