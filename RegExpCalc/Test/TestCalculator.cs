using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegExpCalc.Test
{
    [TestFixture]
    public class RegExpCalcTest
    {
        Calculator equation;

        [Description("Check correctness result of expression")]
        [TestCase("2+2*2", ExpectedResult = "6")]
        [TestCase("8/2*(9-4)", ExpectedResult = "20")]
        [TestCase("(5.6+(-2*-3))^2.2", ExpectedResult = "219.688627843835")]
        [TestCase("(1.55+1.4*3.2)*(3.9-5.1+5.0)", ExpectedResult = "22.914")]
        [TestCase("14+7*(2^3)-(1*2-3+4^(2-4*7*(3*(1.123+2.33))))", ExpectedResult = "71")]
        [TestCase("2^3/4+2/(2+2)+3^1/(6^(14-7*(1+1))+5)", ExpectedResult = "3")]
        [TestCase("-(3.2-2*2)^2", ExpectedResult = "-0.64")]
        [TestCase("1-(-(-(-0.5)))", ExpectedResult = "1.5")]
        [TestCase("-(-(-(-(1+(-2)^(-(12+(-11)))))))", ExpectedResult = "0.5")]
        public string Result(string input)
        {
            equation = input;

            return equation.ToString();
        }

        [Description("Check on throwing exception with incorrectly placed brackets")]
        [TestCase("2+(2+2))")]
        [TestCase("7-)3")]
        [TestCase("-(((-((2))))")]
        [TestCase("(2+2")]
        [TestCase("123+(1234)))")]
        public void CheckBrackets(string input)
        {
            equation = input;

            Assert.AreEqual(false, equation.CheckBracketsNotation());
        }

        [Description("Check on throwing exception with incorrect expression")]
        [TestCase("(3#2)")]
        [TestCase("2a+4")]
        [TestCase("5+6-7{")]
        [TestCase("3+2>2")]
        public void CheckIncorrectSymb(string input)
        {
            equation = input;

            Assert.AreEqual(false, equation.CheckIncorrectCharacters());
        }

        [Description("Check on throwing exception if operations between members in equation aren't unambiguous")]
        [TestCase("2-+2")]
        [TestCase("2-*2")]
        [TestCase("2+*2")]
        [TestCase("2-/2")]
        [TestCase("2+/2")]
        [TestCase("2-^2")]
        [TestCase("2+^2")]
        [TestCase("2*/2")]
        [TestCase("2*^2")]
        [TestCase("2**2")]
        [TestCase("2/^2")]
        [TestCase("2//2")]
        [TestCase("2^^2")]
        [TestCase("2**2")]
        public void CheckOperations(string input)
        {
            equation = input;

            Assert.AreEqual(false, equation.CheckOperationsNotation());
        }

        [Description("Check on throwing exception with incorrectly placed operation in brackets")]
        [TestCase("2+(*2+2))")]
        [TestCase("(^2-2)")]
        [TestCase("(2+7-)*3")]
        [TestCase("(2+2+)")]
        [TestCase("1+(+2)")]
        public void CheckOperationInBrackets(string input)
        {
            equation = input;

            Assert.AreEqual(false, equation.CheckOperationInBracketsNotation());
        }

        [Description("Check on throwing exception with incorrect float numbers notation")]
        [TestCase("2.2.3+2))")]
        [TestCase("2.-2")]
        [TestCase("(2+7.)")]
        [TestCase("3.(2+2)")]
        public void CheckFloatNumbers(string input)
        {
            equation = input;

            Assert.AreEqual(false, equation.CheckCorrectFloatNumbers());
        }
    }
}
