@echo off
dotnet build src/Skybrud.Csv --configuration Debug /t:rebuild /t:pack -p:BuildTools=1 -p:PackageOutputPath=c:/nuget