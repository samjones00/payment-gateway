using FluentValidation.TestHelper;
using NUnit.Framework;
using PaymentGateway.Domain.Models.Card;
using PaymentGateway.Tests.Shared;
using DomainValidators = PaymentGateway.Domain.Validators;

namespace PaymentCardValidatorTests;

public class CardNumberTests : TestBase
{
    private readonly Random random = new();

    [Test, Repeat(10)]
    public void Given_ValidLengthCardNumber_When_Validated_Should_PassValidation()
    {
        // Given
        var cardNumberValue = new string('0', random.Next(CardNumber.MinimumLength, CardNumber.MaximumLength));

        var cardNumber = new PaymentCard
        {
            CardNumber = new CardNumber(cardNumberValue)
        };

        var validator = new DomainValidators.PaymentCardValidator();

        // When
        var results = validator.TestValidate(cardNumber);

        // Then
        results.ShouldNotHaveValidationErrorFor(x => x.CardNumber.Value);
    }

    [TestCase("")]
    [TestCase("000000000000")]
    [TestCase("0000000000000")]
    public void Given_BelowMinimumLengthCardNumber_When_Validated_Should_FailValidation(string cardNumberValue)
    {
        // Given
        var paymentCard = new PaymentCard
        {
            CardNumber = new CardNumber(cardNumberValue)
        };

        var validator = new DomainValidators.PaymentCardValidator();

        // When
        var results = validator.TestValidate(paymentCard);

        // Then
        results.ShouldHaveValidationErrorFor(x => x.CardNumber.Value)
            .WithErrorMessage($"Card number must be at least {CardNumber.MinimumLength} digits.");
    }

    [TestCase("00000000000000000")]
    [TestCase("0000000000000000000")]
    public void Given_AboveMaximumLengthCardNumber_When_Validated_Should_FailValidation(string cardNumberValue)
    {
        // Given
        var paymentCard = new PaymentCard
        {
            CardNumber = new CardNumber(cardNumberValue)
        };

        var validator = new DomainValidators.PaymentCardValidator();

        // When
        var results = validator.TestValidate(paymentCard);

        // Then
        results.ShouldHaveValidationErrorFor(x => x.CardNumber.Value)
            .WithErrorMessage($"Card number must be a maximum of {CardNumber.MaximumLength} digits.");
    }
}