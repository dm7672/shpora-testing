
using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(-1, 0, false, TestName = "Precision is negative")]
    [TestCase(0, 0, false, TestName = "Precision is zero")]
    [TestCase(2, -1, false, TestName = "Scale is negative")]
    [TestCase(2, 2, false, TestName = "Scale is equal to precision")]
    [TestCase(2, 3, false, TestName = "Scale is greater than precision")]
    [TestCase(-1, 0, true, TestName = "Precision is negative and onlyPositive is True")]
    [TestCase(0, 0, true, TestName = "Precision is zero and onlyPositive is True")]
    [TestCase(2, -1, true, TestName = "Scale is negative and onlyPositive is True")]
    [TestCase(2, 2, true, TestName = "Scale equal precision and onlyPositive is True")]
    [TestCase(2, 3, true, TestName = "Scale greater than precision and onlyPositive is True")]
    public void Constructor_ShouldThrowArgumentException_WhenArgsInvalid(int precision, int scale, bool onlyPositive)
    {
        Action act = () => new NumberValidator(precision, scale, onlyPositive);
        act.Should().Throw<ArgumentException>();
    }

    [TestCase("0.0", 17, 2, true, TestName = "ZeroPointZero")]
    [TestCase("0", 1, 0, true, TestName = "Zero")]
    [TestCase("-12.24", 17, 5, false, TestName = "Negative number")]
    [TestCase("-12", 17, 5, false, TestName = "Negative integer")]
    [TestCase("12,24", 17, 5, true, TestName = "Comma divider")]
    [TestCase("+1.23", 4, 2, true, TestName = "Plus sign before number")]
    [TestCase("123", 3, 0, true, TestName = "Integer part length is equal to precision")]
    [TestCase("123.123", 9, 3, true, TestName = "Fraction part length is equal to scale")]
    [TestCase("123.123", 6, 3, true, TestName = "Integer part length plus fraction part length is equal to precision")]
    [TestCase("+123.123", 7, 3, true, TestName = "Sign plus integer part length plus fraction part length is equal to precision")]
    [TestCase("007", 3, 0, true, TestName = "Leading zeroes")]
    [TestCase("1234567890987654321", 19, 0, true, TestName = "Big integer")]
    [TestCase("1234567890987654321.1234567890987654321", 38, 19, true, TestName = "Big float number")]
    [TestCase("+1234567890987654321.1234567890987654321", 39, 19, true, TestName = "Big float number with sign")]
    [TestCase("-1234567890987654321.1234567890987654321", 39, 19, false, TestName = "Big negative float number")]
    public void IsValidNumber_ShouldReturnTrue_WhenValueIsValid(string value, int precision, int scale, bool onlyPositive)
    {
        var nV = new NumberValidator(precision, scale, onlyPositive);
        nV.IsValidNumber(value).Should().BeTrue();
    }

    [TestCase("00.00", 3, 2, false, TestName = "Integer part and fraction part is more than precision")]
    [TestCase("-0.00", 17, 10, true, TestName = "Negative sign is not allowed when onlyPositive is true")]
    [TestCase("+1.23", 3, 2, false, TestName = "Positive sign affects length calculation")]
    [TestCase("-1.23", 3, 2, false, TestName = "Negative sign affects length calculation")]
    [TestCase("+0.000", 17, 2, false, TestName = "Fraction part length is bigger than scale")]
    [TestCase("+a.sd", 3, 2, false, TestName = "Not digits are not allowed")]
    [TestCase(null, 3, 2, false, TestName = "Null")]
    [TestCase("", 3, 2, false, TestName = "Empty string")]
    [TestCase("1234", 3, 2, false, TestName = "Integer part length is more than precision")]
    [TestCase("1e9", 10, 2, false, TestName = "Exponential notation")]
    [TestCase("1+7", 10, 2, false, TestName = "Expression with plus")]
    [TestCase("1-7", 10, 2, false, TestName = "Expression with minus")]
    [TestCase("1*7", 10, 2, false, TestName = "Expression with *")]
    [TestCase("1/7", 10, 2, false, TestName = "Expression with /")]
    [TestCase(" ", 10, 2, false, TestName = "Single whitespace")]
    [TestCase(",", 10, 2, false, TestName = "Single comma")]
    [TestCase(".", 10, 2, false, TestName = "Single dot")]
    [TestCase("+", 10, 2, false, TestName = "Single plus sign")]
    [TestCase("-", 10, 2, false, TestName = "Single negative sign")]
    [TestCase("   ", 10, 2, false, TestName = "Multiple whitespaces")]
    [TestCase(",,", 10, 2, false, TestName = "Multiple commas")]
    [TestCase("..", 10, 2, false, TestName = "Multiple dots")]
    [TestCase("++10", 10, 2, false, TestName = "Multiple plus signs")]
    [TestCase("--21", 10, 2, false, TestName = "Multiple negative signs")]
    [TestCase("1 2", 10, 2, false, TestName = "Whitespace between numbers")]
    [TestCase("1,2,3", 10, 5, false, TestName = "Commas between three numbers")]
    [TestCase("1.2.3", 10, 5, false, TestName = "Dots between three numbers")]
    [TestCase("0007", 3, 0, false, TestName = "Leading zeroes affect length calculation")]
    [TestCase("+-12", 6, 2, false, TestName = "Plus and minus sign")]
    [TestCase("1,.2", 6, 2, false, TestName = "Comma and dot as dividers")]
    [TestCase(",1", 6, 2, false, TestName = "Comma in the beginning of the number")]
    [TestCase("1,", 6, 2, false, TestName = "Comma in the end of the number")]
    [TestCase("-0", 6, 2, true, TestName = "-0 is a negative number")]
    [TestCase("\n1214", 6, 2, false, TestName = "NL symbol in the beginning of the number")]
    [TestCase("\r1214", 6, 2, false, TestName = "CR symbol in the beginning of the number")]
    [TestCase("12\n14", 6, 2, false, TestName = "NL symbol in between the number")]
    [TestCase("12\r14", 6, 2, false, TestName = "CR symbol in between the number")] 
    [TestCase("1214\n", 6, 2, false, TestName = "NL symbol in the end of the number")] // не проходит, возвращает True
    [TestCase("1214\r", 6, 2, false, TestName = "CR symbol in the end of the number")]
    [TestCase("\r\n", 6, 2, false, TestName = "CRNL alone")]
    [TestCase("\n", 6, 2, false, TestName = "NL alone")]
    [TestCase("\r", 6, 2, false, TestName = "CR alone")]
    [TestCase("1_000_000_007", 20, 10, false, TestName = "_ as a divider")]
    [TestCase("0b1000100010001000", 20, 10, false, TestName = "16 bit binary number")]
    [TestCase("0xFFFFFF", 20, 10, false, TestName = "64 bit hexadecimal number")]

    public void IsValidNumber_ShouldReturnFalse_WhenValueIsNotValid(string value, int precision, int scale,
        bool onlyPositive)
    {
        var nV = new NumberValidator(precision, scale, onlyPositive);
        nV.IsValidNumber(value).Should().BeFalse();
    }
}