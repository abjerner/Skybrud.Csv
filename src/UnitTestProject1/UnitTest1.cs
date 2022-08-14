using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skybrud.Csv;

namespace UnitTestProject1 {

    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void TestMethod1() {

            CsvFile csv1 = new CsvFile()
                    .AddColumn("Id")
                    .AddColumn("Name")
                    .AddColumn("Description")
                    .AddRow()
                    .AddCell("1234")
                    .AddCell("Hej med\ndig")
                    .AddCell("hello \"world\"")
                    .AddRow()
                    .AddCell("5678")
                    .AddCell("rød grød med fløde")
                    .File
                ;

            Assert.AreEqual(3, csv1.Columns.Length);
            Assert.AreEqual(2, csv1.Rows.Length);

            Assert.AreEqual("Id", csv1.Columns[0].Name);
            Assert.AreEqual("Name", csv1.Columns[1].Name);
            Assert.AreEqual("Description", csv1.Columns[2].Name);

            Assert.AreEqual("Id", csv1.Rows[0].Cells[0].Column.Name);
            Assert.AreEqual("Name", csv1.Rows[0].Cells[1].Column.Name);
            Assert.AreEqual("Description", csv1.Rows[0].Cells[2].Column.Name);

            Assert.AreEqual("1234", csv1.Rows[0].Cells[0].Value);
            Assert.AreEqual("Hej med\ndig", csv1.Rows[0].Cells[1].Value);
            Assert.AreEqual("hello \"world\"", csv1.Rows[0].Cells[2].Value);

            Assert.AreEqual("Id", csv1.Rows[1].Cells[0].Column.Name);
            Assert.AreEqual("Name", csv1.Rows[1].Cells[1].Column.Name);
            Assert.AreEqual("Description", csv1.Rows[1].Cells[2].Column.Name);

            Assert.AreEqual("5678", csv1.Rows[1].Cells[0].Value);
            Assert.AreEqual("rød grød med fløde", csv1.Rows[1].Cells[1].Value);
            Assert.AreEqual("", csv1.Rows[1].Cells[2].Value);

            CsvFile csv2 = CsvFile.Parse(csv1.ToString(CsvSeparator.SemiColon), CsvSeparator.SemiColon);

            Assert.AreEqual(3, csv2.Columns.Length);
            Assert.AreEqual(2, csv2.Rows.Length);

            Assert.AreEqual("Id", csv2.Columns[0].Name);
            Assert.AreEqual("Name", csv2.Columns[1].Name);
            Assert.AreEqual("Description", csv2.Columns[2].Name);

            Assert.AreEqual("Id", csv2.Rows[0].Cells[0].Column.Name);
            Assert.AreEqual("Name", csv2.Rows[0].Cells[1].Column.Name);
            Assert.AreEqual("Description", csv2.Rows[0].Cells[2].Column.Name);

            Assert.AreEqual("1234", csv2.Rows[0].Cells[0].Value);
            Assert.AreEqual("Hej med\ndig", csv2.Rows[0].Cells[1].Value);
            Assert.AreEqual("hello \"world\"", csv2.Rows[0].Cells[2].Value);

            Assert.AreEqual("Id", csv2.Rows[1].Cells[0].Column.Name);
            Assert.AreEqual("Name", csv2.Rows[1].Cells[1].Column.Name);
            Assert.AreEqual("Description", csv2.Rows[1].Cells[2].Column.Name);

            Assert.AreEqual("5678", csv2.Rows[1].Cells[0].Value);
            Assert.AreEqual("rød grød med fløde", csv2.Rows[1].Cells[1].Value);
            Assert.AreEqual("", csv2.Rows[1].Cells[2].Value);

        }

    }

}