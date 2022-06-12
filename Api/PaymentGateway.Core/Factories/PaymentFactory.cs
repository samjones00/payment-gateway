using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Factories
{
    public static class PaymentFactory
    {
        public static Payment Create(string paymentReference)
        {
            var payment = new Payment
            {
                PaymentReference = new PaymentReference(paymentReference)
            };

            payment.PaymentStatus = PaymentStatus.Pending;

            return payment;
        }
    }
}
