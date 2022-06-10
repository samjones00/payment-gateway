using FluentAssertions;
using FluentValidation.TestHelper;
using PaymentGateway.Domain.Models.Card;

using DomainValidators = PaymentGateway.Domain.Validators;

namespace PaymentCardValidatorTests;

public class CVVTests
{
    [TestCase("000")]
    [TestCase("0000")]
    public void Given_ValidLengthCVV_When_Validated_Should_PassValidation(string cvvValue)
    {
        // Given
        var cardNumber = new PaymentCard
        {
            CVV = new CVV(cvvValue)
        };

        var validator = new DomainValidators.PaymentCardValidator();

        // When
        var results = validator.TestValidate(cardNumber);

        // Then
        results.ShouldNotHaveValidationErrorFor(x => x.CVV.Value);
    }

    [TestCase("")]
    [TestCase("00")]
    [TestCase("0")]
    public void Given_BelowMinimumLengthCVV_When_Validated_Should_FailValidation(string cvvValue)
    {
        // Given
        var cardNumber = new PaymentCard
        {
            CVV = new CVV(cvvValue)
        };

        var validator = new DomainValidators.PaymentCardValidator();

        // When
        var results = validator.TestValidate(cardNumber);

        // Then
        results.ShouldHaveValidationErrorFor(x => x.CVV.Value)
            .WithErrorMessage($"CVV number must be at least {CVV.MinimumLength} digits.");
    }

    [TestCase("00000")]
    [TestCase("000000")]
    public void Given_AboveMaximumLengthCVV_When_Validated_Should_FailValidation(string cvvValue)
    {
        // Given
        var cardNumber = new PaymentCard
        {
            CVV = new CVV(cvvValue)
        };

        var validator = new DomainValidators.PaymentCardValidator();

        // When
        var results = validator.TestValidate(cardNumber);

        // Then
        results.ShouldHaveValidationErrorFor(x => x.CVV.Value)
            .WithErrorMessage($"CVV number must be a maximum of {CVV.MaximumLength} digits.");
    }
}