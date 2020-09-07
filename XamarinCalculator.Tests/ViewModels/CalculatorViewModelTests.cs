using System;
using Moq;
using NUnit.Framework;
using XamarinCalculator.Models;
using XamarinCalculator.Services;
using XamarinCalculator.ViewModels;

namespace XamarinCalculator.Tests.ViewModels
{
    public class CalculatorViewModelTests
    {
        private CalculatorViewModel subject;
        private Mock<IEditorService> mockEditor = new Mock<IEditorService>();

        [SetUp]
        public void SetUp()
        {
            subject = new CalculatorViewModel(mockEditor.Object);
        }

        [TearDown]
        public void TearDown()
        {
            mockEditor.Reset();
        }

        [Test]
        public void Clear_WhenWorkingValueHasText_BecomesEmpty()
        {
            subject.WorkingValue = "1";

            subject.Clear.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
        }

        [Test]
        public void Clear_WhenResultHasText_BecomesEmpty()
        {
            subject.Result = "1";

            subject.Clear.Execute(null);

            Assert.AreEqual(string.Empty, subject.Result);
        }

        [Test]
        public void Clear_WhenActiveOperator_BecomesNull()
        {
            subject.ActiveOperator = Operator.Add;

            subject.Clear.Execute(null);

            Assert.AreEqual(null, subject.ActiveOperator);
        }

        #region Addition Tests

        [Test]
        public void Add_WhenNoResultAndNoWorkingValue_DoesNothing()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = string.Empty;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual(string.Empty, subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenNoResultAndHasWorkingValue_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = string.Empty;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenHasResultAndNoWorkingValue_UpdateActiveOperator()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = "1";

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenHasResultAndHasWorkingValueAndNoActiveOperator_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = null;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenHasResultAndHasWorkingValueAndAdding_ResultBecomesSumAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Add;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("3", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenHasResultAndHasWorkingValueAndSubtracting_ResultBecomesDifferenceAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Subtract;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenHasResultAndHasWorkingValueAndMultiplying_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "4";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Multiply;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("8", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenHasResultAndHasWorkingValueAndDividing_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "2";
            subject.Result = "8";
            subject.ActiveOperator = Operator.Divide;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("4", subject.Result);
            Assert.AreEqual(Operator.Add, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenWillOverflowFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Add;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenWillOverflowFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MinValue.ToString();
            subject.ActiveOperator = Operator.Subtract;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenWillOverflowFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Multiply;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenResultExceeds21CharsFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Add;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenResultExceeds21CharsFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "-99999999999999999999";
            subject.ActiveOperator = Operator.Subtract;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Add_WhenResultExceeds21CharsFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Multiply;

            subject.Add.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }
        #endregion

        #region Subtraction Tests

        [Test]
        public void Subtract_WhenNoResultAndNoWorkingValue_DoesNothing()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = string.Empty;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual(string.Empty, subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenNoResultAndHasWorkingValue_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = string.Empty;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenHasResultAndNoWorkingValue_UpdateActiveOperator()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = "1";

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenHasResultAndHasWorkingValueAndNoActiveOperator_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = null;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenHasResultAndHasWorkingValueAndAdding_ResultBecomesSumAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Add;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("3", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenHasResultAndHasWorkingValueAndSubtracting_ResultBecomesDifferenceAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Subtract;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenHasResultAndHasWorkingValueAndMultiplying_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "4";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Multiply;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("8", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenHasResultAndHasWorkingValueAndDividing_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "2";
            subject.Result = "8";
            subject.ActiveOperator = Operator.Divide;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("4", subject.Result);
            Assert.AreEqual(Operator.Subtract, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenWillOverflowFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Add;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenWillOverflowFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MinValue.ToString();
            subject.ActiveOperator = Operator.Subtract;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenWillOverflowFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Multiply;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenResultExceeds21CharsFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Add;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenResultExceeds21CharsFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "-99999999999999999999";
            subject.ActiveOperator = Operator.Subtract;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Subtract_WhenResultExceeds21CharsFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Multiply;

            subject.Subtract.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }
        #endregion

        #region Multiplication Tests

        [Test]
        public void Multiply_WhenNoResultAndNoWorkingValue_DoesNothing()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = string.Empty;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual(string.Empty, subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenNoResultAndHasWorkingValue_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = string.Empty;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenHasResultAndNoWorkingValue_UpdateActiveOperator()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = "1";

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenHasResultAndHasWorkingValueAndNoActiveOperator_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = null;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenHasResultAndHasWorkingValueAndAdding_ResultBecomesSumAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Add;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("3", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenHasResultAndHasWorkingValueAndSubtracting_ResultBecomesDifferenceAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Subtract;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenHasResultAndHasWorkingValueAndMultiplying_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "4";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Multiply;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("8", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenHasResultAndHasWorkingValueAndDividing_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "2";
            subject.Result = "8";
            subject.ActiveOperator = Operator.Divide;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("4", subject.Result);
            Assert.AreEqual(Operator.Multiply, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenWillOverflowFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Add;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenWillOverflowFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MinValue.ToString();
            subject.ActiveOperator = Operator.Subtract;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenWillOverflowFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Multiply;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenResultExceeds21CharsFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Add;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenResultExceeds21CharsFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "-99999999999999999999";
            subject.ActiveOperator = Operator.Subtract;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Multiply_WhenResultExceeds21CharsFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Multiply;

            subject.Multiply.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }
        #endregion

        #region Division Tests

        [Test]
        public void Divide_WhenNoResultAndNoWorkingValue_DoesNothing()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = string.Empty;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual(string.Empty, subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenNoResultAndHasWorkingValue_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = string.Empty;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenHasResultAndNoWorkingValue_UpdateActiveOperator()
        {
            subject.WorkingValue = string.Empty;
            subject.Result = "1";

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenHasResultAndHasWorkingValueAndNoActiveOperator_ResultBecomesWorkingValueAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = null;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenHasResultAndHasWorkingValueAndAdding_ResultBecomesSumAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Add;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("3", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenHasResultAndHasWorkingValueAndSubtracting_ResultBecomesDifferenceAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Subtract;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenHasResultAndHasWorkingValueAndMultiplying_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "4";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Multiply;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("8", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenHasResultAndHasWorkingValueAndDividing_ResultBecomesProductAndUpdateActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "2";
            subject.Result = "8";
            subject.ActiveOperator = Operator.Divide;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("4", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenWillOverflowFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Add;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenWillOverflowFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MinValue.ToString();
            subject.ActiveOperator = Operator.Subtract;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenWillOverflowFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Multiply;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenResultExceeds21CharsFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Add;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenResultExceeds21CharsFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "-99999999999999999999";
            subject.ActiveOperator = Operator.Subtract;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenResultExceeds21CharsFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Multiply;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Divide_WhenResultExceeds21CharsFromDivide_ResultIsTruncated()
        {
            subject.WorkingValue = "9";
            subject.Result = "1";
            subject.ActiveOperator = Operator.Divide;

            subject.Divide.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("0.1111111111111111111", subject.Result);
            Assert.AreEqual(Operator.Divide, subject.ActiveOperator);
        }
        #endregion

        #region Equals Tests

        [Test]
        public void Evaluate_WhenHasResultAndHasWorkingValueAndNoActiveOperator_RetainWorkingValueAndResult()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = null;

            subject.Evaluate.Execute(null);

            Assert.AreEqual("1", subject.WorkingValue);
            Assert.AreEqual("2", subject.Result);
            mockEditor.Verify(m => m.Clear(), Times.Never());
        }

        [Test]
        public void Evaluate_WhenNoResultAndHasWorkingValueAndHasOperator_ResultBecomesWorkingValueAndClearActiveOperatorAndClearWorkingValue([Values(Operator.Add, Operator.Subtract, Operator.Multiply, Operator.Divide)] Operator op)
        {
            subject.WorkingValue = "1";
            subject.Result = string.Empty;
            subject.ActiveOperator = op;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }
      
        [Test]
        public void Evaluate_WhenHasResultAndHasWorkingValueAndAdding_ResultBecomesSumAndClearActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Add;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("3", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenHasResultAndHasWorkingValueAndSubtracting_ResultBecomesDifferenceAndClearActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "1";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Subtract;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("1", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenHasResultAndHasWorkingValueAndMultiplying_ResultBecomesProductAndClearActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "4";
            subject.Result = "2";
            subject.ActiveOperator = Operator.Multiply;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("8", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenHasResultAndHasWorkingValueAndDividing_ResultBecomesProductAndClearActiveOperatorAndClearWorkingValue()
        {
            subject.WorkingValue = "2";
            subject.Result = "8";
            subject.ActiveOperator = Operator.Divide;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("4", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenWillOverflowFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Add;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenWillOverflowFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = decimal.MinValue.ToString();
            subject.ActiveOperator = Operator.Subtract;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenWillOverflowFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = decimal.MaxValue.ToString();
            subject.ActiveOperator = Operator.Multiply;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenResultExceeds21CharsFromAdd_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Add;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenResultExceeds21CharsFromSubtract_ResultIsError()
        {
            subject.WorkingValue = "1";
            subject.Result = "-99999999999999999999";
            subject.ActiveOperator = Operator.Subtract;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenResultExceeds21CharsFromMultiply_ResultIsError()
        {
            subject.WorkingValue = "2";
            subject.Result = "999999999999999999999";
            subject.ActiveOperator = Operator.Multiply;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("Error", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }

        [Test]
        public void Evaluate_WhenResultExceeds21CharsFromDivide_ResultIsTruncated()
        {
            subject.WorkingValue = "9";
            subject.Result = "1";
            subject.ActiveOperator = Operator.Divide;

            subject.Evaluate.Execute(null);

            Assert.AreEqual(string.Empty, subject.WorkingValue);
            mockEditor.Verify(m => m.Clear(), Times.Once());
            Assert.AreEqual("0.1111111111111111111", subject.Result);
            Assert.AreEqual(null, subject.ActiveOperator);
        }
        #endregion

        #region Negate Tests
        [Test]
        public void Negate_WhenCanNegate_NegatesWorkingValue()
        {
            subject.WorkingValue = "1.0";

            subject.Negate.Execute(null);

            Assert.AreEqual("-1.0", subject.WorkingValue);
        }

        [Test]
        public void CanNegate_WhenNoWorkingValue_ReturnsFalse()
        {
            subject.WorkingValue = string.Empty;

            Assert.False(subject.Negate.CanExecute(null));
        }

        [Test]
        public void CanNegate_WhenWorkingValueIs21Characters_ReturnsFalse()
        {
            subject.WorkingValue = "999999999999999999999";

            Assert.False(subject.Negate.CanExecute(null));
        }

        [Test]
        public void CanNegate_WhenHasWorkingValueLessThan21Characters_ReturnsTrue()
        {
            subject.WorkingValue = "99999999999999999999";

            Assert.True(subject.Negate.CanExecute(null));
        }

        #endregion
    }
}
