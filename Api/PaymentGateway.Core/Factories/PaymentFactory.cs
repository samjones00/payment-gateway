using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Core.Factories
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public PaymentFactory(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public Payment Create(ProcessPaymentCommand command)
        {
            var payment = new Payment
            {
                PaymentReference = PaymentReference.Create(command.PaymentReference),
                PaymentCard = new PaymentCard
                {
                    CardNumber = CardNumber.Create(command.CardNumber),
                    ExpiryDate = ExpiryDate.Create(command.ExpiryDateMonth, command.ExpiryDateYear, DateOnly.FromDateTime(_dateTimeProvider.UtcNow()))
                },
                PaymentStatus = PaymentStatus.Pending
            };

            return payment;
        }
    }
}
