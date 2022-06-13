using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using AutoFixture;
using PaymentGateway.Domain.Models;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Domain.Exceptions;

namespace PaymentGateway.Domain.UnitTests.Models
{
    public class PaymentReferenceTests : TestBase
    {
        [TestCaseSource(nameof(NullOrWhiteSpaceStrings))]
        public void Given_Invalid_Value_When_Creating_Should_Throw_Exception(string value)
        {
            // Arrange & Act
            Func<PaymentReference> action = () => PaymentReference.Create(value);

            // Assert
            action.Should().Throw<InvalidPaymentReferenceException>()
                .WithMessage($"'{nameof(PaymentReference)}' must not be empty.");
        }
        
        [Test]
        public void Given_Valid_Value_When_Creating_Should_Return_Value_Object()
        {
            // Arrange
            var value = _fixture.Create<string>();

            // Act
            var result = PaymentReference.Create(value);

            // Assert
            result.Value.Should().Be(value);
        }

        [Test]
        public void Given_BelowMinimumLengthCardNumber_When_Validated_Should_FailValidation([Range(1, PaymentReference.Length - 1)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<PaymentReference> action = () => PaymentReference.Create(value);

            // Then
            action.Should().Throw<InvalidPaymentReferenceException>()
                .WithMessage($"'{nameof(PaymentReference)}' must be {PaymentReference.Length} characters in length. You entered {length} characters.");
        }

        [Test]
        public void Given_AboveMaximumLengthCardNumber_When_Validated_Should_FailValidation([Range(PaymentReference.Length + 1, PaymentReference.Length + 5)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<PaymentReference> action = () => PaymentReference.Create(value);

            // Then
            action.Should().Throw<InvalidPaymentReferenceException>()
                .WithMessage($"'{nameof(PaymentReference)}' must be {PaymentReference.Length} characters in length. You entered {length} characters.");
        }
    }
}