using PaymentGateway.Core.Providers;
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
        private readonly IEncryptionProvider _encryptionProvider;

        public PaymentFactory(IDateTimeProvider dateTimeProvider, IEncryptionProvider encryptionProvider)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _encryptionProvider = encryptionProvider;
        }

        public Payment Create(SubmitPaymentCommand command)
        {
            var payment = new Payment
            {
                PaymentReference = ShopperReference.Create(command.PaymentReference),
                PaymentCard = new PaymentCard
                {
                    CardNumber = CardNumber.Create(command.CardNumber, _encryptionProvider),
                    ExpiryDate = ExpiryDate.Create(command.ExpiryDateMonth, command.ExpiryDateYear, DateOnly.FromDateTime(_dateTimeProvider.UtcNow()))
                },
                PaymentStatus = PaymentStatus.Pending
            };

            return payment;
        }
    }
}
