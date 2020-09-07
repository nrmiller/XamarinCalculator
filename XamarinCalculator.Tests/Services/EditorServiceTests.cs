using NUnit.Framework;
using XamarinCalculator.Models;
using XamarinCalculator.Services;

namespace XamarinCalculator.Tests
{
    public class EditorServiceTests
    {
        private EditorService subject;

        [SetUp]
        public void Setup()
        {
            subject = new EditorService();
        }

        [Test]
        public void Clear()
        {
            subject.Input = "1.0";

            subject.Clear();

            Assert.AreEqual(string.Empty, subject.Input);
        }

        [Test]
        public void ProcessKey_PositiveZero_StaysZero()
        {
            Assert.AreEqual("0", subject.ProcessKey(Key.Num0));
            Assert.AreEqual("0", subject.ProcessKey(Key.Num0));
        }

        [Test]
        public void ProcessKey_DuplicateDecimals_DoesNotAddExtraDecimals()
        {
            Assert.AreEqual("0", subject.ProcessKey(Key.Num0));
            Assert.AreEqual("0.", subject.ProcessKey(Key.Decimal));
            Assert.AreEqual("0.", subject.ProcessKey(Key.Decimal));
        }

        [Test]
        public void ProcessKey_InitialKeyIsDecimal_PrependsWithZero()
        {
            Assert.AreEqual("0.", subject.ProcessKey(Key.Decimal));
        }

        [Test]
        public void ProcessKey_BuildDecimal_ReturnsCorrectValue()
        {
            Assert.AreEqual("0.", subject.ProcessKey(Key.Decimal));
            Assert.AreEqual("0.0", subject.ProcessKey(Key.Num0));
            Assert.AreEqual("0.01", subject.ProcessKey(Key.Num1));
            Assert.AreEqual("0.012", subject.ProcessKey(Key.Num2));
            Assert.AreEqual("0.0123", subject.ProcessKey(Key.Num3));
            Assert.AreEqual("0.01234", subject.ProcessKey(Key.Num4));
            Assert.AreEqual("0.012345", subject.ProcessKey(Key.Num5));
            Assert.AreEqual("0.0123456", subject.ProcessKey(Key.Num6));
            Assert.AreEqual("0.01234567", subject.ProcessKey(Key.Num7));
            Assert.AreEqual("0.012345678", subject.ProcessKey(Key.Num8));
            Assert.AreEqual("0.0123456789", subject.ProcessKey(Key.Num9));
        }
    }
}