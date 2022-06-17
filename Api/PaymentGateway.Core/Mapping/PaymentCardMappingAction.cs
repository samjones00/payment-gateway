using AutoMapper;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Core.Mapping
{
    public class PaymentCardMappingAction : IMappingAction<SubmitPaymentCommand, PaymentCard>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public PaymentCardMappingAction(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public void Process(SubmitPaymentCommand source, PaymentCard destination, ResolutionContext context)
        {
            var todaysDate = DateOnly.FromDateTime(_dateTimeProvider.UtcNow());

            destination.ExpiryDate = ExpiryDate.Create(source.ExpiryDateMonth, source.ExpiryDateYear, todaysDate);
        }
    }
}
