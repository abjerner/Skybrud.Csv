#if NETSTANDARD2_0_OR_GREATER

using System.Data;
using System.Text;

namespace Skybrud.Csv {

    public partial class CsvFile {

        /// <summary>
        /// Converts the CSV to a corresponding <see cref="DataTable"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="DataTable"/>.</returns>
        public DataTable ToDataTable() {
            return ToDataTable(Path == null ? "CSV" : System.IO.Path.GetFileName(Path));
        }

        /// <summary>
        /// Converts the CSV to a corresponding <see cref="DataTable"/>.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A new instance of <see cref="DataTable"/>.</returns>
        public DataTable ToDataTable(string tableName) {

            // Initialize a new data table
            DataTable table = new(string.IsNullOrWhiteSpace(tableName) ? "CSV" : tableName);

            // Append the columns to the data table
            foreach (var column in Columns) {
                table.Columns.Add(column.Name);
            }

            // Append each redirect row to the data table
            foreach (CsvRow row in Rows) {
                DataRow dr = table.Rows.Add();
                foreach (var column in Columns) {
                    dr[column.Name] = row.GetCellValue(column.Index);
                }
            }

            return table;

        }

        /// <summary>
        /// Converts the specified data <paramref name="table"/> to a new corresponding <see cref="CsvFile"/>.
        /// </summary>
        /// <param name="table">The data table to converted.</param>
        /// <returns>A new instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile FromDataTable(DataTable table) {
            return FromDataTable(table, DefaultSeparator, DefaultEncoding);
        }

        /// <summary>
        /// Converts the specified data <paramref name="table"/> to a new corresponding <see cref="CsvFile"/>.
        /// </summary>
        /// <param name="table">The data table to converted.</param>
        /// <param name="separator">The separator to be used for the CSV file.</param>
        /// <returns>A new instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile FromDataTable(DataTable table, CsvSeparator separator) {
            return FromDataTable(table, separator, DefaultEncoding);
        }

        /// <summary>
        /// Converts the specified data <paramref name="table"/> to a new corresponding <see cref="CsvFile"/>.
        /// </summary>
        /// <param name="table">The data table to converted.</param>
        /// <param name="separator">The separator to be used for the CSV file.</param>
        /// <param name="encoding">The encoding to be used for the CSV file.</param>
        /// <returns></returns>
        public static CsvFile FromDataTable(DataTable table, CsvSeparator separator, Encoding encoding) {

            // Initialize a new CSV file
            CsvFile file = new(separator, encoding);

            // Append all columns from the data table
            foreach (DataColumn column in table.Columns) {
                file.AddColumn(column.ColumnName);
            }

            // Append the rows and cells from the data table
            foreach (DataRow dr in table.Rows) {
                CsvRow row = file.AddRow();
                foreach (DataColumn column in table.Columns) {
                    row.AddCell((string) dr[column.ColumnName]);
                }
            }

            return file;

        }

    }

}

#endif