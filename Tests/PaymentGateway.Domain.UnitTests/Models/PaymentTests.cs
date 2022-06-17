using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;
using PaymentGateway.Tests.Shared;

namespace PaymentGateway.Domain.UnitTests.Models
{
    public class PaymentTests : TestBase
    {
        [Test]
        public void ShouldNotThrowValidationError()
        {
            var paymentRequest = new SubmitPaymentCommand
            {
                ShopperReference = CreateStringOfLength(ShopperReference.Length),
                PaymentReference = CreateStringOfLength(PaymentReference.Length),
                Amount = 12.34m,
                CardNumber = CreateStringOfLength(CardNumber.MinimumLength),
                CVV = CreateStringOfLength(CVV.MinimumLength),
                MerchantReference = CreateStringOfLength(20),
                Currency = "GBP",
                CardHolder = "Sam Jones",
                ExpiryDateMonth = 12,
                ExpiryDateYear = 2022
            };

        }
    }
}
