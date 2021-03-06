using AutoMapper;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Extensions;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Mapping
{
    public class PaymentDetailsResponseMapper : Profile
    {
        public PaymentDetailsResponseMapper()
        {
            CreateMap<Payment, PaymentDetailsResponse>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Value))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference.Value))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.PaymentCard.CardNumber.Value.ToMaskedString()))
                .ForMember(dest => dest.ProcessedOn, opt => opt.MapFrom(src => src.ProcessedOn))
                .ForMember(dest => dest.IsAuthorised, opt => opt.MapFrom(src => src.PaymentStatus == PaymentStatus.Successful));
        }
    }
}