namespace PaymentGateway.Domain.Models
{
    public class PaymentReference
    {
        public const int Length = 36;
        public string Value;

        private PaymentReference(string code)
        {
            Value = code;
        }

        public static PaymentReference Create(string code)
        {
            return new PaymentReference(code);
        }
    }
}