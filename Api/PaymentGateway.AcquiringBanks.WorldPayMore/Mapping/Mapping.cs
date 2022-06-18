using AutoMapper;

namespace PaymentGateway.AcquiringBanks.CKO.Mapping
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Models.Payment, CKO.Models.Request>()
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => src.PaymentReference.Value))
                .ForMember(dest => dest.MerchantReference, opt => opt.MapFrom(src => src.MerchantReference.Value))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Amount.Currency))
                .ForMember(dest => dest.PaymentCard, opt => opt.MapFrom(src => src.PaymentCard));

            CreateMap<Domain.Models.PaymentCard, CKO.Models.PaymentCard>()
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber.Value))
                .ForMember(dest => dest.CVV, opt => opt.MapFrom(src => src.CVV.Value))
                .ForMember(dest => dest.CardHolder, opt => opt.MapFrom(src => src.CardHolder))
                .ForMember(dest => dest.ExpiryDateMonth, opt => opt.MapFrom(src => src.ExpiryDate.Value.Month))
                .ForMember(dest => dest.ExpiryDateYear, opt => opt.MapFrom(src => src.ExpiryDate.Value.Year));
        }
    }
}