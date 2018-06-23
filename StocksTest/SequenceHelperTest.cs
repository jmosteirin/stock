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
            var sequence = new Sample[] { new Sample() { Value = 3.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new Sample() { Value = 2.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new Sample() { Value = 1.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new Sample() { Value = 4.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new Sample() { Value = 4.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new Sample() { Value = 4.0, Date = new DateTime(2016, 6, 1), Valid = true } };
            var result = sequence.Average(3).ToArray();
            Assert.AreEqual(result[0].Value, 3.0);
            Assert.AreEqual(result[0].Valid, false);
            Assert.AreEqual(result[1].Value, 2.5);
            Assert.AreEqual(result[1].Valid, false);
            Assert.AreEqual(result[2].Value, 2.0);
            Assert.AreEqual(result[2].Valid, true);
            Assert.AreEqual(result[3].Value, 7.0 / 3.0);
            Assert.AreEqual(result[3].Valid, true);
            Assert.AreEqual(result[4].Value, 3.0);
            Assert.AreEqual(result[4].Valid, true);
            Assert.AreEqual(result[5].Value, 4.0);
            Assert.AreEqual(result[5].Valid, true);
        }
        [TestMethod]
        public void Variance()
        {
            var sequence = new Sample[] { new Sample() { Value = 3.0, Date = new DateTime(2016, 6, 1), Valid = true },
                                          new Sample() { Value = 2.0, Date = new DateTime(2016, 6, 2), Valid = true },
                                          new Sample() { Value = 1.0, Date = new DateTime(2016, 6, 3), Valid = true },
                                          new Sample() { Value = 4.0, Date = new DateTime(2016, 6, 4), Valid = true },
                                          new Sample() { Value = 4.0, Date = new DateTime(2016, 6, 5), Valid = true },
                                          new Sample() { Value = 4.0, Date = new DateTime(2016, 6, 6), Valid = true } };
            var result = sequence.Variance(3).ToArray();
            Assert.AreEqual(result[0].Valid, false);
            Assert.AreEqual(result[1].Valid, false);
            Assert.AreEqual(result[2].Valid, false);
            Assert.AreEqual(result[3].Valid, false);
            Assert.AreEqual(result[4].Valid, true);
            Assert.IsTrue(result[5].Value > 1.2592 && result[5].Value < 1.2593);
            Assert.AreEqual(result[5].Valid, true);
        }
    }
}
