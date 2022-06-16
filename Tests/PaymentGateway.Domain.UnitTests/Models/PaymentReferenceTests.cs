using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Models;
using PaymentGateway.Tests.Shared;

namespace PaymentGateway.Domain.UnitTests.Models
{
    public class PaymentReferenceTests : TestBase
    {
        [TestCaseSource(nameof(NullOrWhiteSpaceStrings))]
        public void Given_Invalid_Value_When_Creating_Should_Throw_Exception(string value)
        {
            // Arrange & Act
            Func<ShopperReference> action = () => ShopperReference.Create(value);

            // Assert
            action.Should().Throw<InvalidPaymentReferenceException>()
                .WithMessage($"'{nameof(ShopperReference)}' must not be empty.");
        }

        [Test]
        public void Given_Valid_Value_When_Creating_Should_Return_Value_Object()
        {
            // Arrange
            var value = _fixture.Create<string>();

            // Act
            var result = ShopperReference.Create(value);

            // Assert
            result.Value.Should().Be(value);
        }

        [Test]
        public void Given_BelowMinimumLengthCardNumber_When_Validated_Should_FailValidation([Range(1, ShopperReference.Length - 1)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<ShopperReference> action = () => ShopperReference.Create(value);

            // Then
            action.Should().Throw<InvalidPaymentReferenceException>()
                .WithMessage($"'{nameof(ShopperReference)}' must be {ShopperReference.Length} characters in length. You entered {length} characters.");
        }

        [Test]
        public void Given_AboveMaximumLengthCardNumber_When_Validated_Should_FailValidation([Range(ShopperReference.Length + 1, ShopperReference.Length + 5)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<ShopperReference> action = () => ShopperReference.Create(value);

            // Then
            action.Should().Throw<InvalidPaymentReferenceException>()
                .WithMessage($"'{nameof(ShopperReference)}' must be {ShopperReference.Length} characters in length. You entered {length} characters.");
        }
    }
}