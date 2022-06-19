namespace PaymentGateway.Domain.Interfaces
{
    public interface IEncryptionProvider
    {
        string Decrypt(string value);
        string Encrypt(string value);
    }
}