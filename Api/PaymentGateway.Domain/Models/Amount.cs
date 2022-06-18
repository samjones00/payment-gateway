namespace PaymentGateway.Domain.Models
{
    public class Amount
    {
        public decimal Value { get; private set; }
        public string Currency { get; private set; }

        public static Amount Create(decimal paymentReference, string currency)
        {
            var result = new Amount
            {
                Value = paymentReference,
                Currency = currency
            };

            //var validationResult = new Validator().Validate(result);

            //if (!validationResult.IsValid && validationResult.Errors.Any())
            //{
            //    throw new InvalidMerchantException(validationResult.Errors.First().ErrorMessage);
            //}

            return result;
        }

        //private class Validator : AbstractValidator<Amount>
        //{
        //    public Validator()
        //    {
        //        RuleFor(x => x.Value)
        //            .NotEmpty()
        //            .GreaterThan(0.0m)
        //            .WithName(nameof(Amount));
        //    }
        //}
    }
}
