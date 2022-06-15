using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private const string ApiKey = "123";

        public Task<bool> IsAuthenticated(string apiKey)
        {
            return Task.FromResult(true);
        }
    }
}
