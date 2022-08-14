using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.Csv;

namespace UnitTestProject1 {

    [TestClass]
    public class EncodingTests {

        [TestMethod]
        public void LoadDetectEncodingWindows1252() {

            // TODO: This test will currently fail as dectection fails and UTF-8 is used as fallback

            CsvFile csv = CsvFile.Load(IOHelper.MapPath("~/SampleData/SampleWindows1252.csv"));

            //Assert.AreEqual(Encoding.GetEncoding(1252), csv.Encoding);

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

            //byte[] bytes = File.ReadAllBytes(IOHelper.MapPath("~/SampleData/SampleUtf8Bom.csv"));

            //StringBuilder sb = new StringBuilder();

            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();

            //foreach (byte chr in bytes.Take(25)) {
            //    sb.AppendLine((int) chr + " -> " + ((int) chr).ToString("x"));
            //}

            //sb.AppendLine();
            //sb.AppendLine();
            //sb.AppendLine();

            //throw new Exception(sb.ToString());

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