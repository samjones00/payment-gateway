using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        Payment Get(string merchantReference, string paymentReference);
        void Insert(Payment entity);
        Payment Update(Payment entity);
    }
}
