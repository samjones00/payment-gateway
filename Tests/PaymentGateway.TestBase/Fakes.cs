using PaymentGateway.Domain.Commands;
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

        private static string CreateStringOfLength(int length) => new('0', length);
    }
}