using AutoMapper;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;
using PaymentGateway.Domain.Responses;

namespace PaymentGateway.Core.Mapping
{
    public class PaymentDetailsResponseMapper : Profile
    {
        public PaymentDetailsResponseMapper()
        {
            CreateMap<Payment, PaymentDetailsResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Amount.Currency))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference.Value))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.PaymentCard.CardNumber.ToMaskedValue()))
                .ForMember(dest => dest.ProcessedOn, opt => opt.MapFrom(src => src.ProcessedOn))
                .ForMember(dest => dest.IsAuthorised, opt => opt.MapFrom(src => src.PaymentStatus == PaymentStatus.Successful));
        }
    }
}