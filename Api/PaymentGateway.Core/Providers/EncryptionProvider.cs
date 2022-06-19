using System.Text;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Providers
{
    /// <summary>
    /// Not really an encryption provider, mainly for demonstration purposes.
    /// </summary>
    public class WeakEncryptionProvider : IEncryptionProvider
    {
        public string Encrypt(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

        public string Decrypt(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));
    }
}