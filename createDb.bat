Title Create empty database
SQLCMD -E -S localhost,1433 -Q "ALTER DATABASE HUCIDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
SQLCMD -S localhost,1433 -Q "DROP DATABASE HUCIDB"
SQLCMD -S localhost,1433 -Q "CREATE DATABASE HUCIDB"
pause
