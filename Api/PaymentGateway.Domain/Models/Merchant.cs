using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Models
{
    public class Merchant
    {
        //public Merchant(Guid merchantId, string name)
        //{
        //    MerchantId = merchantId;
        //    Name = name;
        //}

        public Guid MerchantId { get; init; }
        public string Name { get; init; }
    }
}
