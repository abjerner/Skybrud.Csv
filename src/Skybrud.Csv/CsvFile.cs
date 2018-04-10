using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Skybrud.Csv {

    /// <summary>
    /// Class representing a CSV file parsed either from a file or a string.
    /// </summary>
    public class CsvFile {

        #region Constants

        /// <summary>
        /// Gets the default CSV separator.
        /// </summary>
        public const CsvSeparator DefaultSeparator = CsvSeparator.SemiColon;

        /// <summary>
        /// Gets tghe default encoding.
        /// </summary>
        public static Encoding DefaultEncoding => Encoding.UTF8;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the seperator used in the CSV file.
        /// </summary>
        public CsvSeparator Separator { get; set; }

        /// <summary>
        /// Gets or sets the encoding used in the CSV file.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Gets the path from where the CSV file was loaded.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets whether this instance has a reference to the path the CSV file was loaded from.
        /// </summary>
        public bool HasPath => !String.IsNullOrWhiteSpace(Path);

        /// <summary>
        /// Gets a list of the columns of the file.
        /// </summary>
        public CsvColumnList Columns { get; }

        /// <summary>
        /// Gets a list of the rows of the file.
        /// </summary>
        public CsvRowList Rows { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new and empty CSV file.
        /// </summary>
        public CsvFile() {
            Separator = DefaultSeparator;
            Encoding = DefaultEncoding;
            Columns = new CsvColumnList(this);
            Rows = new CsvRowList(this);
        }

        /// <summary>
        /// Initializes an empty CSV file.
        /// </summary>
        /// <param name="separator">The separator to be used.</param>
        public CsvFile(CsvSeparator separator) {
            Separator = separator;
            Encoding = DefaultEncoding;
            Columns = new CsvColumnList(this);
            Rows = new CsvRowList(this);
        }

        /// <summary>
        /// Initializes an empty CSV file.
        /// </summary>
        /// <param name="encoding">The encoding to be used. If <c>null</c>, <see cref="DefaultEncoding"/> will be used instead.</param>
        public CsvFile(Encoding encoding) {
            Separator = DefaultSeparator;
            Encoding = encoding ?? DefaultEncoding;
            Columns = new CsvColumnList(this);
            Rows = new CsvRowList(this);
        }

        /// <summary>
        /// Initializes an empty CSV file.
        /// </summary>
        /// <param name="separator">The separator to be used.</param>
        /// <param name="encoding">The encoding to be used. If <c>null</c>, <see cref="DefaultEncoding"/> will be used instead.</param>
        public CsvFile(CsvSeparator separator, Encoding encoding) {
            Separator = separator;
            Encoding = encoding ?? DefaultEncoding;
            Columns = new CsvColumnList(this);
            Rows = new CsvRowList(this);
        }

        #endregion

        #region Member methods

        /// <summary>
        /// Adds a new column to the file.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <returns>The added column.</returns>
        public CsvColumn AddColumn(string name) {
            return Columns.AddColumn(name);
        }

        /// <summary>
        /// Adds a new row to the file.
        /// </summary>
        /// <returns>The added row.</returns>
        public CsvRow AddRow() {
            return Rows.AddRow();
        }

        /// <summary>
        /// Returns a string representation of the CSV file.
        /// </summary>
        /// <returns>A string representation of the CSV file.</returns>
        public override string ToString() {
            return ToString(Separator);
        }

        /// <summary>
        /// Returns a string representation of the CSV file, using the specified <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The separator to be used.</param>
        /// <returns>A string representation of the CSV file.</returns>
        public string ToString(CsvSeparator separator) {

            StringBuilder sb = new StringBuilder();
            
            // Get the separator as a "char"
            char sep;
            switch (separator) {
                case CsvSeparator.Comma: sep = ','; break;
                case CsvSeparator.Colon: sep = ':'; break;
                case CsvSeparator.SemiColon: sep = ';'; break;
                case CsvSeparator.Space: sep = ' '; break;
                case CsvSeparator.Tab: sep = '\t'; break;
                default: sep = ';'; break;
            }

            // Append the first line with the column headers
            for (int i = 0; i < Columns.Length; i++) {
                if (i > 0) sb.Append(sep);
                sb.Append(Escape(Columns[i].Name, sep));
            }

            foreach (CsvRow row in Rows) {
                sb.AppendLine();
                for (int i = 0; i < Columns.Length; i++) {
                    if (i > 0) sb.Append(sep);
                    CsvCell cell = i < row.Cells.Length ? row.Cells[i] : null;
                    sb.Append(Escape(cell == null ? "" : cell.Value, sep));
                }
            }
            
            return sb.ToString();

        }

        /// <summary>
        /// Helper method for escaping special characters (eg. double quotes and line breaks). If the value contains
        /// any characters that should be escaped, the value will be enclosed with double quotes. The value will not be
        /// modified if it doesn't contain invalid characters.
        /// </summary>
        /// <param name="value">The value to be escaped.</param>
        /// <param name="separator">The column separator.</param>
        /// <returns>The escaped string.</returns>
        private string Escape(string value, char separator) {

            if (value.Contains('"') || value.Contains('\n') || value.Contains(separator)) {
                
                // Double quotes are escaped by adding a new double quote for each existing double quote
                return "\"" + value.Replace("\"", "\"\"") + "\"";

            }

            return value;

        }

        /// <summary>
        /// Saves the CSV file to it's original path.
        /// </summary>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save() {
            if (String.IsNullOrWhiteSpace(Path)) throw new Exception("Property not set: Path");
            return Save(Path, Separator, Encoding);
        }

        /// <summary>
        /// Saves the CSV file to the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path) {
            return Save(path, Separator, Encoding);
        }

        /// <summary>
        /// Saves the CSV file at the specified <paramref name="path"/>, using the specified
        /// <paramref name="separator"/> and <see cref="Encoding"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <param name="separator">The separator to be used.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path, CsvSeparator separator) {
            return Save(path, separator, Encoding);
        }

        /// <summary>
        /// Saves the CSV file at the specified <paramref name="path"/>, using <see cref="Separator"/> and the
        /// specified <paramref name="encoding"/>.
        /// </summary>
        /// <param name="path">The path to where the CSV file should be saved.</param>
        /// <param name="encoding">The encoding to be used.</param>
        /// <returns>The original instance of <see cref="CsvFile"/>.</returns>
        public CsvFile Save(string path, Encoding encoding) {
            return Save(path, Separator, encoding);
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
            File.WriteAllText(path, ToString(separator), encoding);
            return this;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Parses the specified <paramref name="text"/> into an instance of <see cref="CsvFile"/>, using
        /// <see cref="DefaultSeparator"/> as a separator.
        /// </summary>
        /// <param name="text">The text representing the contents of the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Parse(string text) {
            return Parse(text, DefaultSeparator);
        }

        /// <summary>
        /// Parses the specified <paramref name="text"/> into an instance of <see cref="CsvFile"/>, using the specified
        /// <paramref name="separator"/>.
        /// </summary>
        /// <param name="text">The text representing the contents of the CSV file.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Parse(string text, CsvSeparator separator) {

            // Initialize a new CSV file
            CsvFile file = new CsvFile();

            // Parse the contents
            return ParseInternal(file, text, separator);

        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path) {
            return Load(path, DefaultSeparator, DefaultEncoding);
        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path, CsvSeparator separator) {
            return Load(path, separator, DefaultEncoding);
        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <param name="encoding">The encoding of the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path, Encoding encoding) {
            return Load(path, DefaultSeparator, encoding);
        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>. 
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <param name="encoding">The encoding of the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path, CsvSeparator separator, Encoding encoding) {

            // Load the contents of the CSV file
            string contents = File.ReadAllText(path, encoding ?? DefaultEncoding).Trim();

            // Initialize a new CSV file
            CsvFile file = new CsvFile { Separator = separator, Path = path };

            // Parse the contents
            ParseInternal(file, contents, separator);

            return file;

        }
        
        /// <summary>
        /// Loads a new CSV file from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(Stream stream) {
            return Load(stream, DefaultSeparator, DefaultEncoding);
        }

        /// <summary>
        /// Loads a new CSV file from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="separator">The separator to </param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(Stream stream, CsvSeparator separator) {
            return Load(stream, separator, DefaultEncoding);
        }

        /// <summary>
        /// Loads a new CSV file from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding of the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(Stream stream, Encoding encoding) {
            return Load(stream, DefaultSeparator, encoding);
        }

        /// <summary>
        /// Loads a new CSV file from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <param name="encoding">The encoding of the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(Stream stream, CsvSeparator separator, Encoding encoding) {

            // Make sure we have an encoding
            encoding = encoding ?? DefaultEncoding;

            // Load the contents of the file/stream into a byte array
            byte[] bytes;
            using (BinaryReader reader = new BinaryReader(stream)) {
                const int bufferSize = 4096;
                using (var ms = new MemoryStream()) {
                    byte[] buffer = new byte[bufferSize];
                    int count;
                    while ((count = reader.Read(buffer, 0, buffer.Length)) != 0) {
                        ms.Write(buffer, 0, count);
                    }
                    bytes = ms.ToArray();
                }
            }

            // Convert the byte array to a string (using the specified encoding)
            string contents = encoding.GetString(bytes);

            // Initialize a new CSV file
            CsvFile file = new CsvFile { Separator = separator };

            // Parse the contents
            ParseInternal(file, contents, separator);

            return file;

        }
        
        /// <summary>
        /// Internal helper method for parsing the contents of a CSV file.
        /// </summary>
        /// <param name="file">The CSV file.</param>
        /// <param name="contents">The contents of the CSV file, as a string.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <returns><paramref name="file"/>.</returns>
        private static CsvFile ParseInternal(CsvFile file, string contents, CsvSeparator separator = DefaultSeparator) {

            // Normalize line endings
            contents = contents.Replace("\r\n", "\n");
            contents = contents.Replace("\r", "\n");

            // Get the separator as a "char"
            char sep;
            switch (separator) {
                case CsvSeparator.Comma: sep = ','; break;
                case CsvSeparator.Colon: sep = ':'; break;
                case CsvSeparator.SemiColon: sep = ';'; break;
                case CsvSeparator.Space: sep = ' '; break;
                case CsvSeparator.Tab: sep = '\t'; break;
                default: sep = ';'; break;
            }

            // Parse each line into a list of cell values
            List<List<string>> lines = ParseLines(contents, sep);

            if (lines.Count == 0) throw new Exception("WTF?\r\r\nSeparator: " + separator + "\r\n" + contents);

            // If malformed, each line/row may not have the same amount of cells
            int maxColumns = lines.Max(x => x.Count);

            // Parse the columns (column headers)
            for (int c = 0; c < maxColumns; c++) {
                string name = lines[0].Skip(c).FirstOrDefault() ?? "";
                file.AddColumn(name);
            }

            // Parse the rows
            for (int r = 1; r < lines.Count; r++) {
                CsvRow row = file.AddRow();
                for (int c = 0; c < maxColumns; c++) {
                    CsvColumn column = file.Columns[c];
                    string value = lines[r].Skip(c).FirstOrDefault() ?? "";
                    row.AddCell(column, value);
                }
            }

            return file;

        }

        /// <summary>
        /// Internal helper method for parsing each line of the 
        /// </summary>
        /// <param name="contents">The contents of the CSV file.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <returns>A list of <see cref="List{String}"/>.</returns>
        private static List<List<string>> ParseLines(string contents, char separator) {

            List<List<string>> lines = new List<List<string>>();

            string buffer = String.Empty;
            bool enclosed = false;
            bool escaped = false;

            List<string> line = new List<string>();

            // Parse each character in the input string
            for (int i = 0; i < contents.Length; i++) {

                char chr = contents[i];

                if (chr == '"') {

                    // If the value is already enclosed, we handle further scenarios
                    if (enclosed) {

                        // Get the next character
                        char next = i < contents.Length - 1 ? contents[i + 1] : ' ';

                        // A double quote may be used to escape another double quote if already in an enclosed value
                        if (next == '"') {
                            if (escaped) {
                                buffer += chr;
                                escaped = false;
                                i++;
                            } else {
                                buffer += chr;
                                escaped = true;
                                i++;
                            }
                        } else {
                            enclosed = false;
                        }

                    } else {
                        enclosed = true;
                    }

                } else if (enclosed) {
                    buffer += chr;
                } else if (chr == separator) {
                    line.Add(buffer);
                    buffer = String.Empty;
                } else if (chr == '\n') {
                    line.Add(buffer);
                    lines.Add(line);
                    buffer = String.Empty;
                    line = new List<string>();
                } else {
                    buffer += chr;
                }


            }

            // Append the last line
            if (line.Count > 0) {
                line.Add(buffer);
                lines.Add(line);
            }

            return lines;

        }
        
        #endregion

    }

}