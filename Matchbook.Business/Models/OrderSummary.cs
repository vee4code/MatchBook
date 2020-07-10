using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Matchbook.Business.Models
{
    public class OrderSummary
    {
        public int Id { get; set; }
        public string ProductSymbol { get; set; }
        public decimal? Price { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public int SubAccount { get; set; }
    }
}
