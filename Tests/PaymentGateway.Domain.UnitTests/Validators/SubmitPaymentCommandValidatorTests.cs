using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Validators;
using PaymentGateway.Tests.Shared;

namespace PaymentGateway.Domain.UnitTests.Validators
{
    public class SubmitPaymentCommandValidatorTests : TestBase
    {
        private Mock<IDateTimeProvider> _mockDateTimeProvider;

        [SetUp]
        public void SetUp()
        {
            _mockDateTimeProvider = _mockRepository.Create<IDateTimeProvider>();
        }

        [Test]
        public void Given_Validator_Should_Validate()
        {
            // Given
            _mockDateTimeProvider.Setup(x => x.UtcNow()).Returns(new DateTime(2022, 01, 01));

            var command = Fakes.ValidSubmitPaymentCommand();
            command.CVV = "ABC";
            
            var sut = new SubmitPaymentCommandValidator(_mockDateTimeProvider.Object);

            // When
            var result = sut.Validate(command);

            //Then
            var errorMessages = result.Errors.Select(x => x.ErrorMessage);

            errorMessages.Should().Contain("'CVV' is not in the correct format.");
        }
    }
}
