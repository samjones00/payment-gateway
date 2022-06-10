using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Core.Factories
{
    public class PaymentFactory
    {
        public Payment Create()
        {
            var payment = new Payment
            {

            };

            payment.PaymentStatus = PaymentStatus.Pending;

            return payment;
        }
    }
}
