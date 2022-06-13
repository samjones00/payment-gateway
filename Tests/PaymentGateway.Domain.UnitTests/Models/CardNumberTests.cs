﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using AutoFixture;
using PaymentGateway.Tests.Shared;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.UnitTests.Models
{
    public class CardNumberTests : TestBase
    {
        [TestCaseSource(nameof(NullOrWhiteSpaceStrings))]
        public void Given_Invalid_Value_When_Creating_Should_Throw_Exception(string value)
        {
            // Given, When
            Func<CardNumber> action = () => CardNumber.Create(value);

            // Then
            action.Should().Throw<InvalidCardNumberException>()
                .WithMessage($"'{nameof(CardNumber)}' must not be empty.");
        }

        [Test]
        public void Given_Valid_Length_Value_When_Creating_Should_Return_Value_Object([Range(CardNumber.MinimumLength, CardNumber.MaximumLength)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            var result = CardNumber.Create(value);

            // Then
            result.Value.Should().Be(value);
        }

        [Test]
        public void Given_AboveMaximumLengthCardNumber_When_Validated_Should_FailValidation([Range(CardNumber.MaximumLength + 1, CardNumber.MaximumLength + 5)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<CardNumber> action = () => CardNumber.Create(value);

            // Then
            action.Should().Throw<InvalidCardNumberException>()
                .WithMessage($"'{nameof(CardNumber)}' must be between {CardNumber.MinimumLength} and {CardNumber.MaximumLength} characters. You entered {length} characters.");
        }

        [Test]
        public void Given_BelowMinimumLengthCardNumber_When_Validated_Should_FailValidation([Range(1, CardNumber.MinimumLength - 1)] int length)
        {
            // Given
            var value = CreateStringOfLength(length);

            // When
            Func<CardNumber> action = () => CardNumber.Create(value);

            // Then
            action.Should().Throw<InvalidCardNumberException>()
                .WithMessage($"'{nameof(CardNumber)}' must be between {CardNumber.MinimumLength} and {CardNumber.MaximumLength} characters. You entered {length} characters.");
        }
    }
}