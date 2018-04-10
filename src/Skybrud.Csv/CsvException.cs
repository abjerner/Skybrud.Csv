using System;

namespace Skybrud.Csv {

    /// <summary>
    /// Exception class used throughout this package.
    /// </summary>
    public class CsvException : Exception {

        /// <summary>
        /// Initializes a new exception with the specified <paramref name="message"/>.
        /// </summary>
        /// <param name="message">The error message.</param>
        public CsvException(string message) : base(message) { }

    }

}