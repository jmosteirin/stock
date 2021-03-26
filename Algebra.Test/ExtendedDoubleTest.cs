using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Algebra.Test
{
    [TestClass]
    public class ExtendedDoubleTest
    {
        [TestMethod]
        public void NormalOperations()
        {
            ExtendedDouble p1 = 4.0;
            ExtendedDouble p2 = 2.0;
            ExtendedDouble p3 = 4.0;
            ExtendedDouble p4 = 5.0;
            Assert.AreEqual(p1 + p2, 6.0);
            Assert.AreEqual(p1 - p2, 2.0);
            Assert.AreEqual(p1 * p2, 8.0);
            Assert.AreEqual(p1 / p2, 2.0);
            //==
            Assert.IsTrue(p1 == p3);
            Assert.IsFalse(p1 == p2);
            //!=
            Assert.IsFalse(p1 != p3);
            Assert.IsTrue(p1 != p2);
            //>=
            Assert.IsTrue(p1 >= p2);
            Assert.IsTrue(p1 >= p3);
            Assert.IsFalse(p1 >= p4);
            //>
            Assert.IsTrue(p1 > p2);
            Assert.IsFalse(p1 > p3);
            Assert.IsFalse(p1 > p4);
            //<=
            Assert.IsTrue(p1 <= p4);
            Assert.IsTrue(p1 <= p3);
            Assert.IsFalse(p1 <= p2);
            //<
            Assert.IsTrue(p1 < p4);
            Assert.IsFalse(p1 < p3);
            Assert.IsFalse(p1 < p2);
        }
        [TestMethod]
        public void InfiniteOperations()
        {
            ExtendedDouble positiveInfinite = ExtendedDouble.PositiveInfinite();
            ExtendedDouble negativeInfinite = ExtendedDouble.NegativeInfinite();
            ExtendedDouble positive = 2.0;
            ExtendedDouble negative = -2.0;
            ExtendedDouble zero = 0.0;
            ExtendedDouble invalid = ExtendedDouble.BuildInvalid();

            Assert.AreEqual(positive + positiveInfinite, positiveInfinite);
            Assert.AreEqual(positiveInfinite + positive, positiveInfinite);
            Assert.AreEqual(positive + negativeInfinite, negativeInfinite);
            Assert.AreEqual(negativeInfinite + positive, negativeInfinite);
            Assert.IsFalse((positiveInfinite + negativeInfinite).Valid);
            Assert.AreEqual(positiveInfinite + positiveInfinite, positiveInfinite);
            Assert.AreEqual(negativeInfinite + negativeInfinite, negativeInfinite);

            Assert.AreEqual(positive - positiveInfinite, negativeInfinite);
            Assert.AreEqual(positiveInfinite - positive, positiveInfinite);
            Assert.AreEqual(positive - negativeInfinite, positiveInfinite);
            Assert.AreEqual(negativeInfinite - positive, negativeInfinite);
            Assert.AreEqual(positiveInfinite - negativeInfinite, positiveInfinite);
            Assert.AreEqual(negativeInfinite - positiveInfinite, negativeInfinite);
            Assert.IsFalse((positiveInfinite - positiveInfinite).Valid);
            Assert.IsFalse((negativeInfinite - negativeInfinite).Valid);

            Assert.AreEqual(positive * positiveInfinite, positiveInfinite);
            Assert.AreEqual(positiveInfinite * positive, positiveInfinite);
            Assert.AreEqual(negative * positiveInfinite, negativeInfinite);
            Assert.AreEqual(positiveInfinite * negative, negativeInfinite);
            Assert.AreEqual(positive * negativeInfinite, negativeInfinite);
            Assert.AreEqual(negativeInfinite * positive, negativeInfinite);
            Assert.AreEqual(negative * negativeInfinite, positiveInfinite);
            Assert.AreEqual(negativeInfinite * negative, positiveInfinite);
            Assert.AreEqual(positiveInfinite * negativeInfinite, negativeInfinite);
            Assert.AreEqual(negativeInfinite * negativeInfinite, positiveInfinite);
            Assert.AreEqual(negativeInfinite * positiveInfinite, negativeInfinite);
            Assert.AreEqual(positiveInfinite * positiveInfinite, positiveInfinite);
            Assert.IsFalse((positiveInfinite * zero).Valid);
            Assert.IsFalse((negativeInfinite * zero).Valid);
            Assert.IsFalse((zero * positiveInfinite).Valid);
            Assert.IsFalse((zero * negativeInfinite).Valid);

            Assert.AreEqual(positive / positiveInfinite, 0);
            Assert.AreEqual(positiveInfinite / positive, positiveInfinite);
            Assert.AreEqual(negative / positiveInfinite, 0);
            Assert.AreEqual(positiveInfinite / negative, negativeInfinite);
            Assert.AreEqual(positive / negativeInfinite, 0);
            Assert.AreEqual(negativeInfinite / positive, negativeInfinite);
            Assert.AreEqual(negative / negativeInfinite, 0);
            Assert.AreEqual(negativeInfinite / negative, positiveInfinite);
            Assert.IsFalse((positiveInfinite / negativeInfinite).Valid);
            Assert.IsFalse((negativeInfinite / negativeInfinite).Valid);
            Assert.IsFalse((negativeInfinite / positiveInfinite).Valid);
            Assert.IsFalse((positiveInfinite / positiveInfinite).Valid);
            Assert.AreEqual(positiveInfinite / zero, positiveInfinite);
            Assert.AreEqual(negativeInfinite / zero, negativeInfinite);
            Assert.AreEqual(zero / positiveInfinite, 0);
            Assert.AreEqual(zero / negativeInfinite, 0);
            Assert.AreEqual(positive / zero, positiveInfinite);
            Assert.AreEqual(negative / zero, negativeInfinite);
            Assert.IsFalse((zero / zero).Valid);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(positiveInfinite >= positiveInfinite);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(positiveInfinite >= positive);
            Assert.IsTrue(positiveInfinite >= negativeInfinite);

            Assert.IsFalse(negativeInfinite >= positiveInfinite);
            Assert.IsFalse(negativeInfinite >= positive);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(negativeInfinite >= negativeInfinite);
#pragma warning restore CS1718 // Comparison made to same variable

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(positiveInfinite > positiveInfinite);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(positiveInfinite > positive);
            Assert.IsTrue(positiveInfinite > negativeInfinite);

            Assert.IsFalse(negativeInfinite > positiveInfinite);
            Assert.IsFalse(negativeInfinite > positive);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(negativeInfinite > negativeInfinite);
#pragma warning restore CS1718 // Comparison made to same variable


#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(positiveInfinite <= positiveInfinite);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsFalse(positiveInfinite <= positive);
            Assert.IsFalse(positiveInfinite <= negativeInfinite);

            Assert.IsTrue(negativeInfinite <= positiveInfinite);
            Assert.IsTrue(negativeInfinite <= positive);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(negativeInfinite <= negativeInfinite);
#pragma warning restore CS1718 // Comparison made to same variable

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(positiveInfinite < positiveInfinite);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsFalse(positiveInfinite < positive);
            Assert.IsFalse(positiveInfinite < negativeInfinite);

            Assert.IsTrue(negativeInfinite < positiveInfinite);
            Assert.IsTrue(negativeInfinite < positive);
#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(negativeInfinite < negativeInfinite);
#pragma warning restore CS1718 // Comparison made to same variable

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(negativeInfinite == negativeInfinite);
            Assert.IsTrue(positiveInfinite == positiveInfinite);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsFalse(negativeInfinite == positiveInfinite);
            Assert.IsFalse(positiveInfinite == negativeInfinite);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsFalse(negativeInfinite != negativeInfinite);
            Assert.IsFalse(positiveInfinite != positiveInfinite);
#pragma warning restore CS1718 // Comparison made to same variable
            Assert.IsTrue(negativeInfinite != positiveInfinite);
            Assert.IsTrue(positiveInfinite != negativeInfinite);
        }
    }
}
