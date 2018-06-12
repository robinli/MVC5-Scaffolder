using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Happy.Scaffolding.MVC.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Happy.Scaffolding.MVC.Models.Tests
{
    [TestClass()]
    public class MetaColumnInfoTests
    {
        [TestMethod()]
        public void GetColumnTypeTest()
        {
            MetaColumnInfo mcol = new MetaColumnInfo();
            euColumnType expected = euColumnType.stringCT;
            euColumnType actual = mcol.GetColumnType("guid");
            Assert.AreEqual(expected, actual);
        }
    }
}
