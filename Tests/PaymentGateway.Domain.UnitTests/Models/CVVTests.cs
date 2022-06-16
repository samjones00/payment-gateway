using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Models.Card;
using PaymentGateway.Tests.Shared;

namespace PaymentGateway.Domain.UnitTests.Models
{
    public class CVVTests : TestBase
    {
        [TestCaseSource(nameof(NullOrWhiteSpaceStrings))]
        public void Given_Invalid_Value_When_Creating_Should_Throw_Exception(string value)
        {
            // Given, When
            Func<CVV> action = () => CVV.Create(value);

            // Then
            action.Should().Throw<InvalidCVVException>()
                .WithMessage($"'{nameof(CVV)}' must not be empty.");
        }

        [Test]
        public void Given_Valid_Length_Value_When_Creating_Should_Return_Value_Object([Range(CVV.MinimumLength, CVV.MaximumLength)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            var result = CVV.Create(value);

            // Then
            result.Value.Should().Be(value);
        }

        [Test]
        public void Given_AboveMaximumLengthCardNumber_When_Validated_Should_FailValidation([Range(CVV.MaximumLength + 1, CVV.MaximumLength + 5)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<CVV> action = () => CVV.Create(value);

            // Then
            action.Should().Throw<InvalidCVVException>()
                .WithMessage($"'{nameof(CVV)}' must be between {CVV.MinimumLength} and {CVV.MaximumLength} characters. You entered {length} characters.");
        }

        [Test]
        public void Given_BelowMinimumLengthCardNumber_When_Validated_Should_FailValidation([Range(1, CVV.MinimumLength - 1)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<CVV> action = () => CVV.Create(value);

            // Then
            action.Should().Throw<InvalidCVVException>()
                .WithMessage($"'{nameof(CVV)}' must be between {CVV.MinimumLength} and {CVV.MaximumLength} characters. You entered {length} characters.");
        }
    }
}