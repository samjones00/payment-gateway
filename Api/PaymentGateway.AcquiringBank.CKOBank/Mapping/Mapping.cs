using AutoMapper;
using PaymentGateway.AcquiringBank.CKO.Models;

namespace PaymentGateway.AcquiringBank.CKO.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Models.Payment, Request>()
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference.Value))
                .ForMember(dest => dest.MerchantReference, opt => opt.MapFrom(src => src.MerchantReference.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Value))
                .ForMember(dest => dest.PaymentCard, opt => opt.MapFrom(src => src.PaymentCard));

            CreateMap<Domain.Models.PaymentCard, PaymentCard>()
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber.Value))
                .ForMember(dest => dest.CVV, opt => opt.MapFrom(src => src.CVV.Value))
                .ForMember(dest => dest.CardHolder, opt => opt.MapFrom(src => src.CardHolder.Value))
                .ForMember(dest => dest.ExpiryDateMonth, opt => opt.MapFrom(src => src.ExpiryDate.Value.Month))
                .ForMember(dest => dest.ExpiryDateYear, opt => opt.MapFrom(src => src.ExpiryDate.Value.Year));
        }
    }
}