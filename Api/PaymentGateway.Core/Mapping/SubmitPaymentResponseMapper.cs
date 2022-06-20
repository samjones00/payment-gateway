using AutoMapper;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Mapping
{
    public class SubmitPaymentResponseMapper : Profile
    {
        public SubmitPaymentResponseMapper()
        {
            CreateMap<Payment, SubmitPaymentResponse>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference.Value))
                .ForMember(dest => dest.ProcessedOn, opt => opt.MapFrom(src => src.ProcessedOn))
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference.Value));
        }
    }
}