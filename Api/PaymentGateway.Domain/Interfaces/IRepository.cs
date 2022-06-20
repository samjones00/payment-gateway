using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IRepository<TEntity>
    {
        void Delete(string merchantReference, string paymentReference);
        Payment Get(string merchantReference, string paymentReference);
        void Insert(Payment entity);
        Payment Update(Payment entity);
    }
}
