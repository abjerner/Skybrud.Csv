namespace Skybrud.Csv {

    internal static class CsvConstants {

        public const char Colon = ':';

        public const char Comma = ',';

        public const char SemiColon = ';';

        public const char Space = ' ';

        public const char Tab = '\t';

        public const char DoubleQuote = '"';

        public const char NewLine = '\n';

        public const char CarriageReturn = '\r';

        public const char ZeroWidthNoBreakSpace = (char) 65279;

        public static readonly char[] WhiteSpaceCharacters = {
            Space, Tab, NewLine, CarriageReturn, ZeroWidthNoBreakSpace
        };

        public static readonly char[] Separators = {
            Colon, Comma, SemiColon, Space, Tab
        };

    }

}