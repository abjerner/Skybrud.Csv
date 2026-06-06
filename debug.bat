@echo off
dotnet build src/Skybrud.Csv --configuration Debug /t:rebuild /t:pack -p:PackageOutputPath=c:/nuget