namespace Skybrud.Csv {

    /// <summary>
    /// Enum class representing the separator used in a CSV file.
    /// </summary>
    public enum CsvSeparator {

        /// <summary>
        /// For load operations, this will indicate that <see cref="CsvFile"/> should automatically try to detect the encoding - eg. based on the BOM header of the file if present.
        ///
        /// This value shouldn't be used for save operations. If it is, <see cref="CsvFile.DefaultSeparator"/> will be used instead.
        /// </summary>
        Auto,

        /// <summary>
        /// Indicates that the separator is a comma.
        /// </summary>
        Comma,

        /// <summary>
        /// Indicates that the separator is a semi colon.
        /// </summary>
        SemiColon,

        /// <summary>
        /// Indicates that the separator is a colon.
        /// </summary>
        Colon,

        /// <summary>
        /// Indicates that the separator is a space.
        /// </summary>
        Space,

        /// <summary>
        /// Indicates that the separator is a tab.
        /// </summary>
        Tab

    }

}