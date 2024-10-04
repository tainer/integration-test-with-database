@echo off

:: Iniciar o container SQL Server
docker run --name sqlserver-test -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest

:: Aguardar a inicialização do SQL Server
timeout /t 20

:: Aplicar o script de criação do banco
sqlcmd -S localhost,1433 -U sa -P YourStrong@Passw0rd -i "./assets/db_structure.sql"

:: Aplicar o script de dados de teste
sqlcmd -S localhost,1433 -U sa -P YourStrong@Passw0rd -i "./assets/db_data.sql"
