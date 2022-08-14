using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.Csv;

namespace UnitTestProject1 {

    [TestClass]
    public class SeparatorTests {

        [TestMethod]
        public void ParseDetectColonAuto() {

            const string raw = "Hello:World\nHej:Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Colon, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectColonExplicit() {

            const string raw = "sep=:\nHello:World\nHej:Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Colon, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectCommaAuto() {

            const string raw = "Hello,World\nHej,Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Comma, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectCommaExplicit() {

            const string raw = "sep=,\nHello,World\nHej,Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Comma, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectSemiColonAuto() {

            const string raw = "Hello;World\nHej;Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.SemiColon, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectSemiColonExplicit() {

            const string raw = "sep=;\nHello;World\nHej;Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.SemiColon, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectSpaceAuto() {

            const string raw = "Hello World\nHej Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Space, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectSpaceExplicit() {

            const string raw = "sep= \nHello World\nHej Verden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Space, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectTabAuto() {

            const string raw = "Hello\tWorld\nHej\tVerden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Tab, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

        [TestMethod]
        public void ParseDetectTabExplicit() {

            const string raw = "sep=\t\nHello\tWorld\nHej\tVerden";

            CsvFile csv = CsvFile.Parse(raw);

            Assert.AreEqual(CsvSeparator.Tab, csv.Separator);

            Assert.AreEqual(2, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("Hello", csv.Columns[0].Name);
            Assert.AreEqual("World", csv.Columns[1].Name);

            Assert.AreEqual("Hej", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("Verden", csv.Rows[0].GetCellValue(1));

        }

    }

}