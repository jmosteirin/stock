using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stocks;
using System.Linq;
using Stocks.Indicators;

namespace StocksTest
{
    [TestClass]
    public class SequenceHelperTest
    {
        [TestMethod]
        public void Average()
        {
            var sequence = new DoubleSample[] { new DoubleSample() { Value = 3.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new DoubleSample() { Value = 2.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new DoubleSample() { Value = 1.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new DoubleSample() { Value = 4.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new DoubleSample() { Value = 4.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new DoubleSample() { Value = 4.0, Date = new DateTime(2016, 6, 1), Valid = true } };
            var result = sequence.Average(3).ToArray();
            Assert.AreEqual(result[0].Value, 3.0);
            Assert.AreEqual(result[1].Value, 2.5);
            Assert.AreEqual(result[2].Value, 2.0);
            Assert.AreEqual(result[3].Value, 7.0/3.0);
            Assert.AreEqual(result[4].Value, 3.0);
            Assert.AreEqual(result[5].Value, 4.0);
        }
    }
}
