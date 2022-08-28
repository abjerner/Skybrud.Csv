using System.Text;
using Skybrud.Csv;

namespace TestProject1 {

    [TestClass]
    public class EncodingTests {

        [TestMethod]
        public void LoadDetectEncodingIso8859() {

            // In .NET Framework, the file's encoding will be detected to be "Windows 1252", while in .NET, the same
            // file's encoding wil be detected to be "ISO 8859-1".

            CsvFile csv = CsvFile.Load(IOHelper.MapPath("~/SampleData/SampleWindows1252.csv"));

            Assert.AreEqual(Encoding.GetEncoding("iso-8859-1"), csv.Encoding);

            Assert.AreEqual(4, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("A", csv.Columns[0].Name);
            Assert.AreEqual("B", csv.Columns[1].Name);
            Assert.AreEqual("C", csv.Columns[2].Name);
            Assert.AreEqual("D", csv.Columns[3].Name);

            Assert.AreEqual("Rød", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("grød", csv.Rows[0].GetCellValue(1));
            Assert.AreEqual("med", csv.Rows[0].GetCellValue(2));
            Assert.AreEqual("fløde", csv.Rows[0].GetCellValue(3));

        }

        [TestMethod]
        public void LoadDetectEncodingUtf8() {

            CsvFile csv = CsvFile.Load(IOHelper.MapPath("~/SampleData/SampleUtf8.csv"));

            Assert.AreEqual(Encoding.UTF8, csv.Encoding);

            Assert.AreEqual(4, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("A", csv.Columns[0].Name);
            Assert.AreEqual("B", csv.Columns[1].Name);
            Assert.AreEqual("C", csv.Columns[2].Name);
            Assert.AreEqual("D", csv.Columns[3].Name);

            Assert.AreEqual("Rød", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("grød", csv.Rows[0].GetCellValue(1));
            Assert.AreEqual("med", csv.Rows[0].GetCellValue(2));
            Assert.AreEqual("fløde", csv.Rows[0].GetCellValue(3));

        }

        [TestMethod]
        public void LoadDetectEncodingUtf8Bom() {

            CsvFile csv = CsvFile.Load(IOHelper.MapPath("~/SampleData/SampleUtf8Bom.csv"));

            Assert.AreEqual(Encoding.UTF8, csv.Encoding);

            Assert.AreEqual(4, csv.Columns.Length);
            Assert.AreEqual(1, csv.Rows.Length);

            Assert.AreEqual("A", csv.Columns[0].Name);
            Assert.AreEqual("B", csv.Columns[1].Name);
            Assert.AreEqual("C", csv.Columns[2].Name);
            Assert.AreEqual("D", csv.Columns[3].Name);

            Assert.AreEqual("Rød", csv.Rows[0].GetCellValue(0));
            Assert.AreEqual("grød", csv.Rows[0].GetCellValue(1));
            Assert.AreEqual("med", csv.Rows[0].GetCellValue(2));
            Assert.AreEqual("fløde", csv.Rows[0].GetCellValue(3));

        }

    }

}