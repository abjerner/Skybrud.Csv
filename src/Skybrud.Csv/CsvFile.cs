using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using static Skybrud.Csv.CsvConstants;

namespace Skybrud.Csv {

    /// <summary>
    /// Class representing a CSV file parsed either from a file or a string.
    /// </summary>
    public partial class CsvFile {

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
        public bool HasPath => !string.IsNullOrWhiteSpace(Path);

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

            // Fallback to the default separator if set to "Auto"
            if (separator == CsvSeparator.Auto) separator = DefaultSeparator;

            StringBuilder sb = new();

            // Get the separator as a "char"
            char sep = GetCharFromSeparator(separator);

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
                    sb.Append(Escape(cell == null ? string.Empty : cell.Value, sep));
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

            if (value.Contains(DoubleQuote) || value.Contains(NewLine) || value.Contains(separator)) {

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
            if (string.IsNullOrWhiteSpace(Path)) throw new Exception("Property not set: Path");
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
            return Parse(text, CsvSeparator.Auto);
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
            CsvFile file = new(separator);

            // Parse the contents
            return ParseInternal(file, text);

        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path) {
            return Load(path, CsvSeparator.Auto, null);
        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <param name="separator">The separator used in the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path, CsvSeparator separator) {
            return Load(path, separator, null);
        }

        /// <summary>
        /// Loads the CSV file at the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path to the CSV file.</param>
        /// <param name="encoding">The encoding of the CSV file.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(string path, Encoding encoding) {
            return Load(path, CsvSeparator.Auto, encoding);
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
            byte[] bytes = File.ReadAllBytes(path);

            // Parse the CSV file from the bytes
            CsvFile file = Parse(bytes, separator, encoding);

            // Update the "Path" property so we'll have it for later
            file.Path = path;

            // Return the file
            return file;

        }

        /// <summary>
        /// Loads a new CSV file from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(Stream stream) {
            return Load(stream, DefaultSeparator, null);
        }

        /// <summary>
        /// Loads a new CSV file from the specified <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="separator">The separator to </param>
        /// <returns>An instance of <see cref="CsvFile"/>.</returns>
        public static CsvFile Load(Stream stream, CsvSeparator separator) {
            return Load(stream, separator, null);
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

            // Load the contents of the file/stream into a byte array
            byte[] bytes;
            using (BinaryReader reader = new(stream)) {
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

            return Parse(bytes, separator, encoding);

        }

        private static CsvFile Parse(byte[] bytes, CsvSeparator separator, Encoding encoding) {

            string contents = null;

            // Try to detect the encoding if none isn't specified
            // TODO: Should we follow the BOM even if another encoding is explicitly specified?
            encoding ??= GetEncodingByBom(bytes) ?? GetEncodingBySeek(bytes, out contents) ?? DefaultEncoding;

            // Convert the byte array to a string (using the "encoding")
            contents ??= encoding.GetString(bytes).Trim(WhiteSpaceCharacters);

            // Initialize a new CSV file
            CsvFile file = new(separator, encoding);

            // Parse the contents
            ParseInternal(file, contents);

            return file;

        }

        /// <summary>
        /// Internal helper method for parsing the contents of a CSV file.
        /// </summary>
        /// <param name="file">The CSV file.</param>
        /// <param name="contents">The contents of the CSV file, as a string.</param>
        /// <returns><paramref name="file"/>.</returns>
        private static CsvFile ParseInternal(CsvFile file, string contents) {

            // Normalize line endings
            // TODO: Might be a bit too much and affect performance badly
            contents = contents.Replace("\r\n", "\n");
            contents = contents.Replace("\r", "\n");

            if (file.Separator == CsvSeparator.Auto) {

                // TODO: Ideally we should only check the first X bytes of the file

                char separator = Separators
                    .ToDictionary(x => x, x => contents.IndexOf(x))
                    .Where(x => x.Value >= 0)
                    .OrderBy(x => x.Value)
                    .FirstOrDefault().Key;

                file.Separator = IsSeparator(separator, out CsvSeparator e) ? e : DefaultSeparator;

            }

            // Parse each line into a list of cell values
            List<List<string>> lines = ParseLines(file, contents);

            if (lines.Count == 0) throw new CsvException("Invalid CSV file");

            // If malformed, each line/row may not have the same amount of cells
            int maxColumns = lines.Max(x => x.Count);

            // Parse the columns (column headers)
            for (int c = 0; c < maxColumns; c++) {
                string name = lines[0].Skip(c).FirstOrDefault() ?? string.Empty;
                file.AddColumn(name);
            }

            // Parse the rows
            for (int r = 1; r < lines.Count; r++) {
                CsvRow row = file.AddRow();
                for (int c = 0; c < maxColumns; c++) {
                    CsvColumn column = file.Columns[c];
                    string value = lines[r].Skip(c).FirstOrDefault() ?? string.Empty;
                    row.AddCell(column, value);
                }
            }

            return file;

        }

        /// <summary>
        /// Internal helper method for parsing each line of the
        /// </summary>
        /// <param name="file">The CSV file we're parsing.</param>
        /// <param name="contents">The contents of the CSV file.</param>
        /// <returns>A list of <see cref="List{String}"/>.</returns>
        private static List<List<string>> ParseLines(CsvFile file, string contents) {

            List<List<string>> lines = new();

            string buffer = string.Empty;
            bool enclosed = false;
            bool escaped = false;

            // Get the separator as a "char"
            char separator = GetCharFromSeparator(file.Separator);

            List<string> line = new();

            int offset = 0;

            // Does the first line specify the seprartor?
            if (contents.Length > 4 && contents[0] == 's' && contents[1] == 'e' && contents[2] == 'p' && contents[3] == '=') {
                if (IsSeparator(contents[4])) {
                    separator = contents[4];
                    file.Separator = GetSeparatorFromChar(separator);
                    offset = 5;
                }
            }

            // Increment the offset if the next character is \n
            if (contents.Length > 5 && contents[5] is NewLine) offset++;

            // Parse each character in the input string
            for (int i = offset; i < contents.Length; i++) {

                char chr = contents[i];

                if (chr == DoubleQuote) {

                    // If the value is already enclosed, we handle further scenarios
                    if (enclosed) {

                        // Get the next character
                        char next = i < contents.Length - 1 ? contents[i + 1] : Space;

                        // A double quote may be used to escape another double quote if already in an enclosed value
                        if (next == DoubleQuote) {
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
                    buffer = string.Empty;
                } else if (chr == NewLine) {
                    line.Add(buffer);
                    lines.Add(line);
                    buffer = string.Empty;
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

        private static char GetCharFromSeparator(CsvSeparator separator) {
            return separator switch {
                CsvSeparator.Comma => Comma,
                CsvSeparator.Colon => Colon,
                CsvSeparator.SemiColon => SemiColon,
                CsvSeparator.Space => Space,
                CsvSeparator.Tab => Tab,
                _ => SemiColon
            };
        }

        private static CsvSeparator GetSeparatorFromChar(char character) {
            return character switch {
                Colon => CsvSeparator.Colon,
                Comma => CsvSeparator.Comma,
                SemiColon => CsvSeparator.SemiColon,
                Space => CsvSeparator.Space,
                Tab => CsvSeparator.Tab,
                _ => throw new CsvException($"Unknown separator: {character}")
            };
        }

        private static bool IsSeparator(char character) {
            return character is Colon or Comma or SemiColon or Space or Tab;
        }

        private static bool IsSeparator(char character, out CsvSeparator separator) {

            switch (character) {

                case Colon:
                    separator = CsvSeparator.Colon;
                    return true;

                case Comma:
                    separator = CsvSeparator.Comma;
                    return true;

                case SemiColon:
                    separator = CsvSeparator.SemiColon;
                    return true;

                case Space:
                    separator = CsvSeparator.Space;
                    return true;

                case Tab:
                    separator = CsvSeparator.Tab;
                    return true;

                default:
                    separator = default;
                    return false;

            }

        }

        /// <summary>
        /// Attempts to the find encoding of the specified <paramref name="bytes"/> by checking for the BOM header of various unicode encodings.
        /// </summary>
        /// <param name="bytes">The bytes to check.</param>
        /// <returns>An instance of <see cref="Encoding"/> if successful; otherwise, <c>null</c>.</returns>
        /// <see>
        ///     <cref>https://stackoverflow.com/a/19283954</cref>
        /// </see>
        private static Encoding GetEncodingByBom(byte[] bytes) {
            if (bytes == null || bytes.Length < 4) return null;
            if (bytes[0] == 0x2b && bytes[1] == 0x2f && bytes[2] == 0x76) return Encoding.UTF7;
            if (bytes[0] == 0xef && bytes[1] == 0xbb && bytes[2] == 0xbf) return Encoding.UTF8;
            if (bytes[0] == 0xff && bytes[1] == 0xfe && bytes[2] == 0 && bytes[3] == 0) return Encoding.UTF32;
            if (bytes[0] == 0xff && bytes[1] == 0xfe) return Encoding.Unicode;
            if (bytes[0] == 0xfe && bytes[1] == 0xff) return Encoding.BigEndianUnicode;
            if (bytes[0] == 0 && bytes[1] == 0 && bytes[2] == 0xfe && bytes[3] == 0xff) return new UTF32Encoding(true, true);
            return null;
        }

        /// <summary>
        /// Attemps to find the encoding of the specified <paramref name="bytes"/> by certain special characters are
        /// successfully converted using different encodings.
        /// </summary>
        /// <param name="bytes">The bytes to check.</param>
        /// <param name="plainText">When this method returns, holds the plain text string represented by <paramref name="bytes"/> if successful; otherwise, <c>null</c>.</param>
        /// <returns>An instance of <see cref="Encoding"/> if successful; otherwise, <c>null</c>.</returns>
        /// <remarks>This method may be expensive as worst case scenario is that the method will run through each byte
        /// in <paramref name="bytes"/> one time for each encoding the method is checking against. Using Big O notation,
        /// the performance can be described like O(N * M) where N is the length of <paramref name="bytes"/> and M is
        /// the encodings that the method is checking against (currently two). Based on this, we can determine that
        /// iterations increase linearly, which is not super bad, but it might still include a lot of iterations as
        /// <paramref name="bytes"/> grow.</remarks>
        private static Encoding GetEncodingBySeek(byte[] bytes, out string plainText) {

            plainText = Encoding.UTF8.GetString(bytes);
            if (plainText.Any(chr => _characters.Contains(chr))) return Encoding.UTF8;

            try {
                Encoding encoding = Encoding.GetEncoding(1252);
                plainText = encoding.GetString(bytes);
                if (plainText.Any(chr => _characters.Contains(chr))) return encoding;
            } catch (Exception) {
                // ignore
            }

            try {
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                plainText = encoding.GetString(bytes);
                if (plainText.Any(chr => _characters.Contains(chr))) return encoding;
            } catch (Exception) {
                // ignore
            }

            plainText = null;
            return null;

        }

        /// <summary>
        /// Defines a set of reference characters that we'll check against to determine the encoding.
        /// </summary>
        private static readonly HashSet<char> _characters = new() {
            'Ç', // Latin Capital Letter C With Cedilla
            'ü', // Latin Small Letter U With Diaeresis
            'é', // Latin Small Letter E With Acute
            'â', // Latin Small Letter A With Circumflex
            'ä', // Latin Small Letter A With Diaeresis
            'à', // Latin Small Letter A With Grave
            'å', // Latin Small Letter A With Ring Above
            'ç', // Latin Small Letter C With Cedilla
            'ê', // Latin Small Letter E With Circumflex
            'ë', // Latin Small Letter E With Diaeresis
            'è', // Latin Small Letter E With Grave
            'ï', // Latin Small Letter I With Diaeresis
            'î', // Latin Small Letter I With Circumflex
            'ì', // Latin Small Letter I With Grave
            'Ä', // Latin Capital Letter A With Diaeresis
            'Å', // Latin Capital Letter A With Ring Above
            'É', // Latin Capital Letter E With Acute
            'æ', // Latin Small Letter Ae
            'Æ', // Latin Capital Letter Ae
            'ô', // Latin Small Letter O With Circumflex
            'ö', // Latin Small Letter O With Diaeresis
            'ò', // Latin Small Letter O With Grave
            'û', // Latin Small Letter U With Circumflex
            'ù', // Latin Small Letter U With Grave
            'ÿ', // Latin Small Letter Y With Diaeresis
            'Ö', // Latin Capital Letter O With Diaeresis
            'Ü', // Latin Capital Letter U With Diaeresis
            'á', // Latin Small Letter A With Acute
            'í', // Latin Small Letter I With Acute
            'ó', // Latin Small Letter O With Acute
            'ú', // Latin Small Letter U With Acute
            'ñ', // Latin Small Letter N With Tilde, Small Letter Enye
            'Ñ', // Latin Capital Letter N With Tilde, Capital Letter Enye,
            'ø',
            'Ø',
            '¿'
        };

        #endregion

    }

}