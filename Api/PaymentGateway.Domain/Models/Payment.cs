﻿using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public PaymentReference PaymentReference { get; set; }

        public MerchantReference MerchantReference { get; init; }

        public PaymentCard PaymentCard { get; init; }

        public Amount Amount { get; init; }
        public DateTime ProcessedOn { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
