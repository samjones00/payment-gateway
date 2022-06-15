namespace PaymentGateway.Domain
{
    public static class ApiRoutes
    {
        public const string Path = "/payment";
        public const string SubmitPayment = $"{Path}/submit";
        public const string GetPaymentDetails = $"{Path}/details";
    }
}