# Skybrud.Csv

**Skybrud.Csv** is a small library for parsing and generating CSV files. It's written in .NET, and supports both .NET Standard 1.3 and .NET Framework 4.5 (and newer).

## Installation

To install the package, pick one of the two methods below:

1. [**NuGet Package**][NuGetPackage]  
   Install this NuGet package in your Visual Studio project. Makes updating easy.

2. [**ZIP file**][GitHubRelease]  
   Grab a ZIP file of the latest release; unzip and move the `Skybrud.Csv.dll` for your target framework to the bin directory of your project.
   
## Usage

**Generate and save a new CSV file using method chaining**  
The packages supports method chaining, so you can generate a new CSV file as shown below. Notice that you can specify both the separator and encoding when saving to disk.

```csharp
CsvFile csv1 = new CsvFile()
    .AddColumn("Id")
    .AddColumn("Name")
    .AddColumn("Description")
    .AddRow()
        .AddCell("1234")
        .AddCell("Hej med\ndig")
        .AddCell("hello \"world\"")
    .AddRow()
        .AddCell("5678")
        .AddCell("rød grød med fløde")
        .AddCell("hello")
    .File
    .Save("C:/Example.csv", CsvSeparator.Tab, Encoding.UTF8);
```

**Load a CSV file from disk**  
When loading a CSV file from disk, you specify the path as well as the separator and encoding.

If the separator and encoding is left out, they will default to <code field="Skybrud.Csv.CsvSeparator.Tab">CsvSeparator.Tab</code> and <code property="System.Text.Encoding.UTF8">Encoding.UTF8</code> respectively. You can see the <code type="Skybrud.Csv.CsvFile">CsvFile</code> class for more about this.

```csharp
// Load the file from disk (assuming tabular as separator and UTF-8 as encoding)
CsvFile csv2 = CsvFile.Load("C:/Example.csv", CsvSeparator.Tab, Encoding.UTF8);

// Iterate through the columns
foreach (CsvColumn column in csv2.Columns)
{
    Console.WriteLine(column.Index + ". " + column.Name);
}

// Iterate through the rows
foreach (CsvRow row in csv2.Rows)
{
    
    Console.WriteLine("Row #" + (row.Index + 1));
    
    // Iterate throug the cells of the row
    foreach (CsvCell cell in row.Cells)
    {
        Console.WriteLine(cell.Column.Name + " => " + cell.Value);
    }

}
```



[NuGetPackage]: https://www.nuget.org/packages/Skybrud.Csv
[GitHubRelease]: https://github.com/abjerner/Skybrud.Csv/releases/latest
[Issues]: https://github.com/abjerner/Skybrud.Csv/issues
