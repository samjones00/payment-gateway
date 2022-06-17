using System.Text;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Providers
{
    public class WeakEncryptionProvider : IEncryptionProvider
    {
        public string Encode(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

        public string Decode(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value  ));
    }
}