namespace PaymentGateway.Domain.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsAuthenticated(string apiKey);
    }
}
