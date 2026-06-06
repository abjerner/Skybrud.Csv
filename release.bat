@echo off
dotnet build src/Skybrud.Csv --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget