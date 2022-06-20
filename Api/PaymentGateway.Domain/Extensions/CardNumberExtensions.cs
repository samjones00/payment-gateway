using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Extensions
{
    public static class CardNumberExtensions
    {
        public static string ToMaskedValue(this CardNumber source)
        {
            if (string.IsNullOrEmpty(source.Value) || CardNumber.UnmaskedDigitCount >= source.Value.Length)
            {
                return string.Empty;
            }

            string? masked = new(CardNumber.MaskCharacter, source.Value.Length - CardNumber.UnmaskedDigitCount);
            var unmasked = source.Value.Remove(0, source.Value.Length - CardNumber.UnmaskedDigitCount);

            return $"{masked}{unmasked}";
        }
    }
}