using System.Runtime.CompilerServices;

namespace PaymentGateway.Domain.Extensions
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Throws if the value is null or whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void ThrowIfNullOrWhiteSpace(this string value, [CallerArgumentExpression("value")] string? argumentName = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Throw if the value is zero
        /// </summary>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void ThrowIfZero(this int value, [CallerArgumentExpression("value")] string? argumentName = null)
        {
            if (value == Decimal.Zero)
            {
                throw new ArgumentException(argumentName);
            }
        }
    }
}