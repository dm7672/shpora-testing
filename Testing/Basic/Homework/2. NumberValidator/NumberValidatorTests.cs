
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [Test]
    public void ThrowsArgumentException_WhenPrecisionNegative()
    {
        Action act = () => new NumberValidator(-1, 2);
        act.Should().Throw<ArgumentException>();
    }
    [Test]
    public void ThrowsArgumentException_WhenScaleMoreOrEqualToPrecision()
    {
        Action act = () => new NumberValidator(2, 2);
        act.Should().Throw<ArgumentException>();
    }
    [Test]
    public void Creates_NumberValidator()
    {
        Action act = () => new NumberValidator(1, 0, true);
        act.Should().NotThrow();
    }
    [Test]
    public void ZeroPointZero_IsValid()
    {
        var numberValidator = new NumberValidator(17, 2, true);
        numberValidator.IsValidNumber("0.0").Should().BeTrue();
    }
    [Test]
    public void Zero_IsValid()
    {
        var numberValidator = new NumberValidator(17, 2, true);
        numberValidator.IsValidNumber("0").Should().BeTrue();
    }
    [Test]
    public void ZeroZero_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, true);
        numberValidator.IsValidNumber("00.00").Should().BeFalse();
    }
    [Test]
    public void WhenOnlyPositive_NegativeInput_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, true);
        numberValidator.IsValidNumber("-0.00").Should().BeFalse();
    }
    [Test]
    public void PlusSignAffectsValidation_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, true);
        numberValidator.IsValidNumber("+1.23").Should().BeFalse();
    }
    [Test]
    public void PlusSignAffectsValidation_IsValid()
    {
        var numberValidator = new NumberValidator(4, 2, true);
        numberValidator.IsValidNumber("+1.23").Should().BeTrue();
    }
    [Test]
    public void FracPartIsMoreThanScale_IsNotValid()
    {
        var numberValidator = new NumberValidator(17, 2, true);
        numberValidator.IsValidNumber("+0.000").Should().BeFalse();
    }
    [Test]
    public void LettersAreNotAllowed_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, true);
        numberValidator.IsValidNumber("a.sd").Should().BeFalse();
    }
    // Новые тесты
    [Test]
    public void Null_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, false);
        numberValidator.IsValidNumber(null).Should().BeFalse();
    }
    [Test]
    public void EmptyString_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, false);
        numberValidator.IsValidNumber("").Should().BeFalse();
    }
    [Test]
    public void IntPartMoreThanPrecision_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, false);
        numberValidator.IsValidNumber("1234").Should().BeFalse();
    }
    [Test]
    public void NumberMoreThanPrecision_IsNotValid()
    {
        var numberValidator = new NumberValidator(3, 2, false);
        numberValidator.IsValidNumber("12.24").Should().BeFalse();
    }
    [Test]
    public void NegativeNumber_IsValid()
    {
        var numberValidator = new NumberValidator(10, 5, false);
        numberValidator.IsValidNumber("-12.24").Should().BeTrue();
    }
    [Test]
    public void ExponentNotation_IsNotValid()
    {
        var numberValidator = new NumberValidator(10, 5, false);
        numberValidator.IsValidNumber("1e9").Should().BeFalse();
    }
    [Test]
    public void Expression_IsNotValid()
    {
        var numberValidator = new NumberValidator(10, 5, false);
        numberValidator.IsValidNumber("1+7").Should().BeFalse();
    }
    [Test]
    public void Whitespace_IsNotValid()
    {
        var numberValidator = new NumberValidator(10, 5, false);
        numberValidator.IsValidNumber(" ").Should().BeFalse();
    }
    [Test]
    public void MultipleNumbers_IsNotValid()
    {
        var numberValidator = new NumberValidator(10, 5, false);
        numberValidator.IsValidNumber("1 2").Should().BeFalse();
    }

    [Test]
    public void DivisorComma_IsValid()
    {
        var numberValidator = new NumberValidator(10, 4, false);
        numberValidator.IsValidNumber("1,223").Should().BeTrue();
    }
    [Test]
    public void SingleComma_IsNotValid()
    {
        var numberValidator = new NumberValidator(10, 4, false);
        numberValidator.IsValidNumber(",").Should().BeFalse();
    }
    [Test]
    public void MultipleComma_IsNotValid()
    {
        var numberValidator = new NumberValidator(10, 4, false);
        numberValidator.IsValidNumber(",,,").Should().BeFalse();
    }
}