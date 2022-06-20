using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Domain.Extensions
{
    public static class CardNumberExtensions
    {
        public static string ToMaskedString(this string source)
        {
            if (string.IsNullOrEmpty(source) || CardNumber.UnmaskedDigitCount >= source.Length)
            {
                return string.Empty;
            }

            var masked = new string(CardNumber.MaskCharacter, source.Length - CardNumber.UnmaskedDigitCount);
            var unmasked = source.Remove(0, source.Length - CardNumber.UnmaskedDigitCount);

            return $"{masked}{unmasked}";
        }
    }
}