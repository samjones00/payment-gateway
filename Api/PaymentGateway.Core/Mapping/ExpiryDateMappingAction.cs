using AutoMapper;
using PaymentGateway.Domain.Dto;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Core.Mapping
{
    public class SetExpiryDateAction : IMappingAction<SubmitPaymentCommand, PaymentCard>
    {
        private readonly IDateTimeProvider _httpContextAccessor;

        public SetExpiryDateAction(IDateTimeProvider httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Process(SubmitPaymentCommand source, PaymentCard destination, ResolutionContext context)
        {
            var todaysDate = DateOnly.FromDateTime(DateTime.UtcNow);

            destination.ExpiryDate = ExpiryDate.Create(source.ExpiryDateMonth, source.ExpiryDateYear, todaysDate);
        }
    }
}
