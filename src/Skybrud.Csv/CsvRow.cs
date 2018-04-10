using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Skybrud.Csv {

    /// <summary>
    /// Class representing a row in an instance of <see cref="CsvFile"/>.
    /// </summary>
    public class CsvRow {

        #region Properties

        /// <summary>
        /// Gets a reference back to the parent <see cref="CsvFile"/>.
        /// </summary>
        public CsvFile File { get; }

        /// <summary>
        /// Gets the index of the row.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the value of the first cell matching the specified <paramref name="columnName"/>, or <c>null</c>
        /// if not found.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>An instance of <see cref="CsvCell"/>, or <c>null</c> if not found.</returns>
        public CsvCell this[string columnName] {
            get { return Cells.FirstOrDefault(x => x.Column.Name == columnName); }
        }

        /// <summary>
        /// Gets a reference to the cells of the row.
        /// </summary>
        public CsvCellList Cells { get; }

        #endregion

        #region Constructors

        internal CsvRow(int index, CsvFile file) {
            Index = index;
            File = file;
            Cells = new CsvCellList(this);
        }

        #endregion

        #region Member methods

        internal CsvCell AddCell(CsvColumn column, string value) {
            return Cells.AddCell(column, value);
        }

        /// <summary>
        /// Adds a new cell with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value of the cell.</param>
        /// <returns>The added cell.</returns>
        public CsvCell AddCell(string value) {
            CsvColumn column = File.Columns[Cells.Length];
            return Cells.AddCell(column, value);
        }
    
        /// <summary>
        /// Gets the string value of the cell at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        /// <returns>The string value of the cell.</returns>
        public string GetCellValue(int index) {
            CsvCell cell = Cells[index];
            return cell?.Value;
        }

        /// <summary>
        /// Gets the string value of the cell at the specified <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="index">The index of the cell.</param>
        /// <returns>The string value of the cell.</returns>
        public T GetCellValue<T>(int index) {
            CsvCell cell = Cells[index];
            return cell == null ? default(T) : (T) Convert.ChangeType(cell.Value, typeof(T));
        }

        /// <summary>
        /// Gets the string value of the cell at the specified <paramref name="index"/>, and converts it using
        /// <paramref name="callback"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="index">The index of the cell.</param>
        /// <param name="callback">The callback function to be used for converting the value.</param>
        /// <returns>An instance of <typeparamref name="T"/> representing the value of the cell.</returns>
        public T GetCellValue<T>(int index, Func<string, T> callback) {
            CsvCell cell = Cells[index];
            return cell == null ? default(T) : callback(cell.Value);
        }

        /// <summary>
        /// Gets the string value of the cell with the specified <paramref name="columnName"/>. If multiple columns
        /// match <paramref name="columnName"/>, only the value of the first cell will be returned.
        /// </summary>
        /// <param name="columnName">The name of the cell.</param>
        /// <returns>The string value of the cell.</returns>
        public string GetCellValue(string columnName) {
            CsvCell cell = this[columnName];
            return cell?.Value;
        }

        /// <summary>
        /// Gets the string value of the cell with the specified <paramref name="columnName"/>, and converts it to the
        /// type of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="columnName">The name of the cell.</param>
        /// <returns>An instance of <typeparamref name="T"/> representing the value of the cell.</returns>
        public T GetCellValue<T>(string columnName) {
            CsvCell cell = this[columnName];
            return cell == null ? default(T) : (T) Convert.ChangeType(cell.Value, typeof(T));
        }

        /// <summary>
        /// Gets the string value of the cell with the specified <paramref name="columnName"/>, and converts it using
        /// <paramref name="callback"/>.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="columnName">The name of the cell.</param>
        /// <param name="callback">The callback function to be used for converting the value.</param>
        /// <returns>An instance of <typeparamref name="T"/> representing the value of the cell.</returns>
        public T GetCellValue<T>(string columnName, Func<string, T> callback) {
            CsvCell cell = this[columnName];
            return cell == null ? default(T) : callback(cell.Value);
        }

        /// <summary>
        /// Sets the value of the cell at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the cell.</param>
        /// <param name="value">The new value of the cell.</param>
        /// <returns>The original instance of <see cref="CsvRow"/>.</returns>
        public CsvRow SetCellValue(int index, object value) {
            CsvCell cell = Cells[index];
            cell.Value = String.Format(CultureInfo.InvariantCulture, "{0}", value);
            return this;
        }

        /// <summary>
        /// Set the value of the cell with the specified <paramref name="columnName"/>.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="value">The new value of the cell.</param>
        /// <returns>The original instance of <see cref="CsvRow"/>.</returns>
        public CsvRow SetCellValue(string columnName, object value) {
            CsvCell cell = this[columnName];
            cell.Value = String.Format(CultureInfo.InvariantCulture, "{0}", value);
            return this;
        }
        
        /// <summary>
        /// Saves the CSV file to it's original path.
        /// </summary>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save() {
            return File.Save();
        }

        /// <summary>
        /// Saves the CSV file to the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path) {
            return File.Save(path);
        }

        /// <summary>
        /// Saves the CSV file at the specified <paramref name="path"/>, using the specified
        /// <paramref name="separator"/> and <see cref="CsvFile.Encoding"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <param name="separator">The separator to be used.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path, CsvSeparator separator) {
            return File.Save(path, separator);
        }

        /// <summary>
        /// Saves the CSV file at the specified <paramref name="path"/>, using <see cref="CsvFile.Separator"/> and the
        /// specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <param name="encoding">The encoding to be used.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path, Encoding encoding) {
            return File.Save(path, encoding);
        }

        /// <summary>
        /// Saves the CSV file at the specified <paramref name="path"/>, using the specified
        /// <paramref name="separator"/> and <paramref name="encoding"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <param name="separator">The separator to be used.</param>
        /// <param name="encoding">The encoding to be used.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path, CsvSeparator separator, Encoding encoding) {
            return File.Save(path, separator, encoding);
        }

        #endregion

    }

}