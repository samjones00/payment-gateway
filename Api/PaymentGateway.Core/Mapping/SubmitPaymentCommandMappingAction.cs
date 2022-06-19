using AutoMapper;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Mapping
{
    public class SubmitPaymentCommandMappingAction : IMappingAction<SubmitPaymentCommand, Payment>
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SubmitPaymentCommandMappingAction(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public void Process(SubmitPaymentCommand source, Payment destination, ResolutionContext context)
        {
            destination.ProcessedOn = _dateTimeProvider.UtcNow();
        }
    }
}