using System;

namespace Skybrud.Csv;

/// <summary>
/// Enum class representing the separator used in a CSV file.
/// </summary>
public enum CsvSeparator {

    /// <summary>
    /// For load operations, this will indicate that <see cref="CsvFile"/> should automatically try to detect the encoding - e.g. based on the BOM header of the file if present.
    ///
    /// This value shouldn't be used for save operations. If it is, <see cref="CsvFile.DefaultSeparator"/> will be used instead.
    /// </summary>
    Auto,

    /// <summary>
    /// Indicates that the separator is a comma.
    /// </summary>
    Comma,

    /// <summary>
    /// Indicates that the separator is a semicolon.
    /// </summary>
    [Obsolete("Use 'Semicolon' instead.")]
    SemiColon,

    /// <summary>
    /// Indicates that the separator is a semicolon.
    /// </summary>
    Semicolon,

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
    Tab,

    /// <summary>
    /// Indicates that the separator is a pipe (<c>|</c>).
    /// </summary>
    Pipe

}