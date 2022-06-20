using PaymentGateway.Domain.Extensions;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Models.Card;

namespace PaymentGateway.Core.Repositories
{
    public class CreditCardEncryptionDecorator : IRepository<Payment>
    {
        private readonly IRepository<Payment> _repository;
        private readonly IEncryptionProvider _encryptionProvider;

        public CreditCardEncryptionDecorator(IRepository<Payment> repository, IEncryptionProvider encryptionProvider)
        {
            _repository = repository ?? throw new NullReferenceException(nameof(repository));
            _encryptionProvider = encryptionProvider ?? throw new NullReferenceException(nameof(encryptionProvider));
        }

        public void Delete(string merchantReference, string paymentReference)
        {
            _repository.Delete(merchantReference, paymentReference);
        }

        public Payment Get(string merchantReference, string paymentReference)
        {
            var payment = _repository.Get(merchantReference, paymentReference);

            var result = payment.Clone();

            result.PaymentCard.CardNumber = DecryptCardNumber(result.PaymentCard.CardNumber.Value);

            return result;
        }

        public void Insert(Payment entity)
        {
            entity.PaymentCard.CardNumber = EncryptCardNumber(entity.PaymentCard.CardNumber.Value);

            _repository.Insert(entity);
        }

        public Payment Update(Payment entity)
        {
            entity.PaymentCard.CardNumber = EncryptCardNumber(entity.PaymentCard.CardNumber.Value);

            var payment = _repository.Update(entity);

            return payment;
        }

        private CardNumber EncryptCardNumber(string cardNumber) => CardNumber.Create(_encryptionProvider.Encrypt(cardNumber));
        private CardNumber DecryptCardNumber(string cardNumber) => CardNumber.Create(_encryptionProvider.Decrypt(cardNumber));
    }
}