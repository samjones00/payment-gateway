﻿namespace PaymentGateway.Domain.Models.Card
{
    public class CardHolder
    {
        public const int MinimumLength = 2;
        public const int MaximumLength = 40;

        public string Value { get; private set; }

        public static CardHolder Create(string cardNumber)
        {
            var result = new CardHolder
            {
                Value = cardNumber
            };

            //var validationResult = new Validator().Validate(result);

            //if (!validationResult.IsValid && validationResult.Errors.Any())
            //{
            //    throw new InvalidCardNumberException(validationResult.Errors.First().ErrorMessage);
            //}

            return result;
        }

        //private class Validator : AbstractValidator<CardHolder>
        //{
        //    public Validator()
        //    {
        //        RuleFor(x => x.Value)
        //            .NotEmpty()
        //            .Length(MinimumLength, MaximumLength)
        //            .WithMessage($"Must be between {MinimumLength} and {MaximumLength} characters.")
        //            .Matches(RegexPatterns.LettersAndSpacesOnly)
        //            .WithMessage($"Must contain only letters and spaces.")
        //            .WithName(nameof(CardHolder));
        //    }
        //}
    }
}