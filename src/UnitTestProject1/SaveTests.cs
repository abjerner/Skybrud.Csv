using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.Csv;

namespace UnitTestProject1 {

    [TestClass]
    public class SaveTests {

        [TestMethod]
        public void SaveColon() {

            CsvFile csv = new CsvFile(CsvSeparator.Colon);
            csv.Columns.AddColumn("A");
            csv.Columns.AddColumn("B");

            CsvRow row = csv.AddRow();
            row.AddCell("Hello");
            row.AddCell("World");

            csv.Save("ResultColon.csv");

            CsvFile csv2 = CsvFile.Load("ResultColon.csv");

            Assert.AreEqual(csv.Separator, csv2.Separator);
            Assert.AreEqual(2, csv2.Columns.Length);

            Assert.AreEqual("A", csv2.Columns[0].Name);
            Assert.AreEqual("B", csv2.Columns[1].Name);

            Assert.AreEqual("Hello", csv2.Rows[0].GetCellValue(0));
            Assert.AreEqual("World", csv2.Rows[0].GetCellValue(1));

        }

    }

}