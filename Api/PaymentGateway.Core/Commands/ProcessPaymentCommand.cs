using MediatR;
using PaymentGateway.Core.Responses;

namespace PaymentGateway.Domain.Dto
{
    public class ProcessPaymentCommand : IRequest<ProcessPaymentResponse>
    {
        public string PaymentReference { get; set; }
    }
}
