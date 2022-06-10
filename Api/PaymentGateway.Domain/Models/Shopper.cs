using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Models
{
    public class Shopper
    {
        public Guid ShopperId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}
