using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Repositories
{
    public class InMemoryRepository : IRepository<Payment>
    {
        private readonly IMemoryCache _payments;

        public InMemoryRepository(IMemoryCache memoryCache)
        {
            _payments = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public void Delete(string merchantReference, string paymentReference)
        {
            var (_, exists, key) = TryGet(merchantReference, paymentReference);

            if (!exists)
            {
                throw new PaymentNotFoundException($"Payment not found.", paymentReference, merchantReference);
            }

            _payments.Remove(key);
        }

        public Payment Get(string merchantReference, string paymentReference)
        {
            var (payment, exists, _) = TryGet(merchantReference, paymentReference);

            if (!exists)
            {
                throw new PaymentNotFoundException($"Payment not found.", paymentReference, merchantReference);
            }

            return payment;
        }

        public void Insert(Payment entity)
        {
            var merchantReference = entity.MerchantReference.Value;
            var paymentReference = entity.PaymentReference.Value;
            var (_, exists, key) = TryGet(merchantReference, paymentReference);

            if (exists)
            {
                throw new PaymentAlreadyExistsException($"Payment already exists.", paymentReference, merchantReference);
            }

            _payments.Set(key, entity);
        }

        public Payment Update(Payment entity)
        {
            var merchantReference = entity.MerchantReference.Value;
            var paymentReference = entity.PaymentReference.Value;
            var (_, exists, key) = TryGet(merchantReference, paymentReference);

            if (!exists)
            {
                throw new PaymentNotFoundException($"Payment not found.", paymentReference, merchantReference);
            }

            _payments.Set(key, entity);

            return entity;
        }

        private (Payment payment, bool exists, string key) TryGet(string merchantReference, string paymentReference)
        {
            var key = GenerateKey(merchantReference, paymentReference);
            var exists = _payments.TryGetValue<Payment>(key, out var payment);
            return (payment, exists, key);
        }

        private static string GenerateKey(string merchantReference, string paymentReference) => $"{merchantReference}-{paymentReference}";
    }
}
