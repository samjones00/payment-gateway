﻿using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Tests.Shared
{
    public static class Fakes
    {
        public static SubmitPaymentCommand ValidSubmitPaymentCommand()
        {
            return new SubmitPaymentCommand
            {
                PaymentReference = CreateStringOfLength(PaymentReference.Length),
                Amount = 12.34m,
                CardNumber = CreateStringOfLength(CardNumber.MinimumLength),
                CVV = CreateStringOfLength(CVV.MinimumLength),
                MerchantReference = CreateStringOfLength(20),
                Currency = "GBP",
                CardHolder = "Joe Bloggs",
                ExpiryDateMonth = 12,
                ExpiryDateYear = 2022
            };
        }

        public static Payment ValidPayment()
        {
            return new Payment
            {
                PaymentReference = PaymentReference.Create(CreateStringOfLength(PaymentReference.Length)),
                Amount = Amount.Create(12.34m),
                PaymentCard = new PaymentCard
                {
                    CardNumber = CardNumber.Create(CreateStringOfLength(CardNumber.MinimumLength)),
                    CVV = CVV.Create(CreateStringOfLength(CVV.MinimumLength)),
                    ExpiryDate = ExpiryDate.Create(1, DateTime.Now.Year),
                    CardHolder = CardHolder.Create(CreateStringOfLength(CardHolder.MinimumLength)),
                },
                MerchantReference = MerchantReference.Create(CreateStringOfLength(10)),
                Currency = ISOCurrency.Create("GBP"),
                PaymentStatus = Domain.Enums.PaymentStatus.Successful,
                ProcessedOn = DateTime.Now
            };
        }

        private static string CreateStringOfLength(int length) => new('0', length);
    }
}