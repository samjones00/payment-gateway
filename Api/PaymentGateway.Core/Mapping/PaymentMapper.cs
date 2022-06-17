using AutoMapper;
using PaymentGateway.Domain.Commands;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Core.Mapping
{
    public class PaymentMapper : Profile
    {
        public PaymentMapper()
        {
            CreateMap<SubmitPaymentCommand, Payment>()
                .ForMember(dest => dest.PaymentReference, opt => opt.MapFrom(src => ShopperReference.Create(src.PaymentReference)))
                .ForMember(dest => dest.Merchant, opt => opt.MapFrom(src => Merchant.Create(src.MerchantReference)))
                .ForMember(dest => dest.ShopperReference, opt => opt.MapFrom(src => ShopperReference.Create(src.ShopperReference)))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => Amount.Create(src.Amount)))
                .ForMember(dest => dest.PaymentCard, opt => opt.MapFrom(src => src));

            CreateMap<SubmitPaymentCommand, PaymentCard>()
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => CardNumber.Create(src.CardNumber)))
                .ForMember(dest => dest.CardHolder, opt => opt.MapFrom(src => CardHolder.Create(src.CardHolder)))
                .ForMember(dest => dest.CVV, opt => opt.MapFrom(src => CVV.Create(src.CVV)))
                .AfterMap<PaymentCardMappingAction>();
        }
    }
}